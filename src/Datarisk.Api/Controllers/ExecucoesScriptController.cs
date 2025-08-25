using Datarisk.Application.Comandos;
using Datarisk.Application.Consultas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Datarisk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExecucoesScriptController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExecucoesScriptController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obter todas as execuções de script
    /// </summary>
    /// <response code="200">Lista de execuções retornada com sucesso</response>
    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        var query = new ObterTodasExecucoesScriptQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obter execução de script por ID
    /// </summary>
    /// <response code="200">Execução encontrada com sucesso</response>
    /// <response code="404">Execução não encontrada</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var query = new ObterExecucaoScriptQuery { Id = id };
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    /// <summary>
    /// Obter execuções de script por categoria
    /// </summary>
    /// <response code="200">Lista de execuções da categoria retornada com sucesso</response>
    [HttpGet("categoria/{categoria}")]
    public async Task<IActionResult> ObterPorCategoria(string categoria)
    {
        var query = new ObterExecucoesScriptPorCategoriaQuery { Categoria = categoria };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Criar nova execução de script
    /// </summary>
    /// <response code="201">Execução criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarExecucaoScriptComando comando)
    {
        try
        {
            var result = await _mediator.Send(comando);
            return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Executar teste de script por ID
    /// </summary>
    /// <response code="200">Teste executado com sucesso</response>
    /// <response code="400">Erro na execução do teste</response>
    [HttpPost("{id}/executar")]
    public async Task<IActionResult> Executar(int id)
    {
        try
        {
            var comando = new ExecutarTesteScriptComando { ExecucaoScriptId = id };
            var result = await _mediator.Send(comando);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
