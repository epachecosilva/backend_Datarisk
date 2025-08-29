using Datarisk.Application.Comandos;
using Datarisk.Application.Consultas;
using Datarisk.Core.Entities;
using Datarisk.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Datarisk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProcessamentosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProcessamentosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obter todos os processamentos
    /// </summary>
    /// <response code="200">Lista de processamentos retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProcessamentoResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProcessamentoResponse>>> ObterProcessamentos()
    {
        var processamentos = await _mediator.Send(new ObterTodosProcessamentosQuery());
        var response = processamentos.Select(p => new ProcessamentoResponse
        {
            Id = p.Id,
            ScriptId = p.ScriptId,
            DadosEntrada = ObterJsonFormatado(p.DadosEntrada),
            DadosSaida = ObterJsonFormatado(p.DadosSaida),
            MensagemErro = p.MensagemErro,
            Status = p.Status.ToString(),
            CriadoEm = p.CriadoEm,
            IniciadoEm = p.IniciadoEm,
            ConcluidoEm = p.ConcluidoEm,
            Script = p.Script != null ? new ScriptResponse
            {
                Id = p.Script.Id,
                Nome = p.Script.Nome,
                Descricao = p.Script.Descricao,
                Codigo = p.Script.Codigo,
                CriadoEm = p.Script.CriadoEm,
                AtualizadoEm = p.Script.AtualizadoEm
            } : null
        });
        return Ok(response);
    }

    private static object? ObterJsonFormatado(string? jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
            return null;

        try
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            
            return System.Text.Json.JsonSerializer.Deserialize<object>(jsonString, options);
        }
        catch (System.Text.Json.JsonException)
        {
            return jsonString;
        }
        catch
        {
            return jsonString;
        }
    }
    /// <summary>
    /// Obter processamento por ID
    /// </summary>
    /// <response code="200">Processamento encontrado com sucesso</response>
    /// <response code="404">Processamento não encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ObterProcessamento(int id)
    {
        var processamento = await _mediator.Send(new ObterProcessamentoQuery(id));
        if (processamento == null)
            return NotFound();

        var dadosEntrada = ObterJsonFormatado(processamento.DadosEntrada);
        var dadosSaida = ObterJsonFormatado(processamento.DadosSaida);

        var result = new
        {
            id = processamento.Id,
            scriptId = processamento.ScriptId,
            dadosEntrada = dadosEntrada,
            dadosSaida = dadosSaida,
            mensagemErro = processamento.MensagemErro,
            status = processamento.Status.ToString(),
            criadoEm = processamento.CriadoEm,
            iniciadoEm = processamento.IniciadoEm,
            concluidoEm = processamento.ConcluidoEm
        };

        return Ok(result);
    }

    /// <summary>
    /// Obter processamentos por script ID
    /// </summary>
    /// <response code="200">Lista de processamentos do script retornada com sucesso</response>
    /// <response code="404">Script não encontrado</response>
    [HttpGet("script/{scriptId}")]
    [ProducesResponseType(typeof(IEnumerable<ProcessamentoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProcessamentoResponse>>> ObterProcessamentosPorScript(int scriptId)
    {
        var processamentos = await _mediator.Send(new ObterProcessamentosPorScriptQuery(scriptId));
        var response = processamentos.Select(p => new ProcessamentoResponse
        {
            Id = p.Id,
            ScriptId = p.ScriptId,
            DadosEntrada = ObterJsonFormatado(p.DadosEntrada),
            DadosSaida = ObterJsonFormatado(p.DadosSaida),
            MensagemErro = p.MensagemErro,
            Status = p.Status.ToString(),
            CriadoEm = p.CriadoEm,
            IniciadoEm = p.IniciadoEm,
            ConcluidoEm = p.ConcluidoEm,
            Script = p.Script != null ? new ScriptResponse
            {
                Id = p.Script.Id,
                Nome = p.Script.Nome,
                Descricao = p.Script.Descricao,
                Codigo = p.Script.Codigo,
                CriadoEm = p.Script.CriadoEm,
                AtualizadoEm = p.Script.AtualizadoEm
            } : null
        });
        return Ok(response);
    }

    /// <summary>
    /// Criar novo processamento
    /// </summary>
    /// <response code="201">Processamento criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Script não encontrado</response>
    [HttpPost]
    [ProducesResponseType(typeof(Processamento), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProcessamentoResponse>> CriarProcessamento([FromBody] CriarProcessamentoRequest request)
    {
        try
        {
            var dadosEntradaString = request.DadosEntrada is string s
                ? s
                : JsonSerializer.Serialize(request.DadosEntrada);

            var cmd = new CriarProcessamentoComando
            {
                ScriptId = request.ScriptId,
                DadosEntrada = dadosEntradaString
            };

            var p = await _mediator.Send(cmd);

            var resp = new ProcessamentoResponse
            {
                Id = p.Id,
                ScriptId = p.ScriptId,
                DadosEntrada = ParseJsonOrNull(p.DadosEntrada),
                DadosSaida = ParseJsonOrNull(p.DadosSaida),
                MensagemErro = p.MensagemErro,
                Status = p.Status.ToString(),
                CriadoEm = p.CriadoEm,
                IniciadoEm = p.IniciadoEm,
                ConcluidoEm = p.ConcluidoEm
            };

            return CreatedAtAction(nameof(ObterProcessamento), new { id = p.Id }, resp);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Listar arquivos de teste disponíveis
    /// </summary>
    /// <response code="200">Lista de arquivos retornada com sucesso</response>
    /// <response code="400">Erro ao listar arquivos</response>
    /// <response code="404">Pasta test-data não encontrada</response>
    [HttpGet("arquivos-teste")]
    [ProducesResponseType(typeof(IEnumerable<ArquivoTeste>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<ArquivoTeste>> ListarArquivosTeste()
    {
        try
        {
            // Caminho dentro do container Docker
            var testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "test-data");
            
            // Fallback para desenvolvimento local
            if (!System.IO.Directory.Exists(testDataPath))
            {
                testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "test-data");
            }
            
            if (!System.IO.Directory.Exists(testDataPath))
            {
                return NotFound(new { error = "Pasta test-data não encontrada" });
            }

            var arquivos = System.IO.Directory.GetFiles(testDataPath, "*.json")
                .Select(filePath => new ArquivoTeste
                {
                    Nome = Path.GetFileName(filePath),
                    Caminho = filePath,
                    Tamanho = new FileInfo(filePath).Length,
                    ModificadoEm = System.IO.File.GetLastWriteTime(filePath)
                })
                .OrderBy(f => f.Nome)
                .ToList();

            return Ok(arquivos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"Erro ao listar arquivos: {ex.Message}" });
        }
    }

    /// <summary>
    /// Criar processamento de teste com dados do Banco Central
    /// </summary>
    /// <response code="201">Processamento criado com sucesso</response>
    /// <response code="400">Dados inválidos ou arquivo não encontrado</response>
    /// <response code="404">Script não encontrado</response>
    [HttpPost("teste-banco-central")]
    [ProducesResponseType(typeof(Processamento), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Processamento>> CriarProcessamentoTesteBancoCentral([FromBody] CriarProcessamentoTesteRequest request)
    {
        try
        {
            // Carregar dados de teste do arquivo
            var dadosTestePath = Path.Combine(Directory.GetCurrentDirectory(), "test-data", "banco-central-payment-data.json");
            
            // Fallback para desenvolvimento local
            if (!System.IO.File.Exists(dadosTestePath))
            {
                dadosTestePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "test-data", "banco-central-payment-data.json");
            }
            
            if (!System.IO.File.Exists(dadosTestePath))
            {
                return BadRequest(new { error = "Arquivo de dados de teste não encontrado" });
            }

            var dadosTeste = await System.IO.File.ReadAllTextAsync(dadosTestePath);

            var comando = new CriarProcessamentoComando
            {
                ScriptId = request.ScriptId,
                DadosEntrada = dadosTeste
            };

            var processamento = await _mediator.Send(comando);
            return CreatedAtAction(nameof(ObterProcessamento), new { id = processamento.Id }, processamento);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Criar processamento de teste com arquivo específico
    /// </summary>
    /// <response code="201">Processamento criado com sucesso</response>
    /// <response code="400">Dados inválidos ou arquivo não encontrado</response>
    /// <response code="404">Script não encontrado</response>
    [HttpPost("teste-com-arquivo")]
    [ProducesResponseType(typeof(Processamento), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Processamento>> CriarProcessamentoComArquivo([FromBody] CriarProcessamentoComArquivoRequest request)
    {
        try
        {
            // Validar nome do arquivo
            if (string.IsNullOrWhiteSpace(request.NomeArquivo))
            {
                return BadRequest(new { error = "Nome do arquivo é obrigatório" });
            }

            // Construir caminho do arquivo
            var testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "test-data");
            var dadosTestePath = Path.Combine(testDataPath, request.NomeArquivo);

            // Fallback para desenvolvimento local
            if (!System.IO.File.Exists(dadosTestePath))
            {
                testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "test-data");
                dadosTestePath = Path.Combine(testDataPath, request.NomeArquivo);
            }

            // Validar se o arquivo existe
            if (!System.IO.File.Exists(dadosTestePath))
            {
                return BadRequest(new { error = $"Arquivo '{request.NomeArquivo}' não encontrado na pasta test-data" });
            }

            // Validar extensão
            if (!request.NomeArquivo.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { error = "Apenas arquivos .json são permitidos" });
            }

            // Carregar dados do arquivo
            var dadosTeste = await System.IO.File.ReadAllTextAsync(dadosTestePath);

            var comando = new CriarProcessamentoComando
            {
                ScriptId = request.ScriptId,
                DadosEntrada = dadosTeste
            };

            var processamento = await _mediator.Send(comando);
            return CreatedAtAction(nameof(ObterProcessamento), new { id = processamento.Id }, processamento);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"Erro ao processar arquivo: {ex.Message}" });
        }
    }
    private static object? ParseJsonOrNull(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        try
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.Clone();
        }
        catch
        {
            return json;
        }
    }
}

public record CriarProcessamentoRequest
{
    public int ScriptId { get; init; }
    public object? DadosEntrada { get; init; }
}

public record CriarProcessamentoTesteRequest
{
    public int ScriptId { get; init; }
}

public record CriarProcessamentoComArquivoRequest
{
    public int ScriptId { get; init; }
    public string NomeArquivo { get; init; } = string.Empty;
}

public class ArquivoTeste
{
    public string Nome { get; set; } = string.Empty;
    public string Caminho { get; set; } = string.Empty;
    public long Tamanho { get; set; }
    public DateTime ModificadoEm { get; set; }
}

