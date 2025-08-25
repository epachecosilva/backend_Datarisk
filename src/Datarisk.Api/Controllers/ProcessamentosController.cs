using Datarisk.Application.Comandos;
using Datarisk.Application.Consultas;
using Datarisk.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Processamento>>> ObterProcessamentos()
    {
        var processamentos = await _mediator.Send(new ObterTodosProcessamentosQuery());
        return Ok(processamentos);
    }

    /// <summary>
    /// Obter processamento por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Processamento>> ObterProcessamento(int id)
    {
        var processamento = await _mediator.Send(new ObterProcessamentoQuery(id));
        if (processamento == null)
            return NotFound();

        return Ok(processamento);
    }

    /// <summary>
    /// Obter processamentos por script ID
    /// </summary>
    [HttpGet("script/{scriptId}")]
    public async Task<ActionResult<IEnumerable<Processamento>>> ObterProcessamentosPorScript(int scriptId)
    {
        var processamentos = await _mediator.Send(new ObterProcessamentosPorScriptQuery(scriptId));
        return Ok(processamentos);
    }

    /// <summary>
    /// Criar novo processamento
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Processamento>> CriarProcessamento([FromBody] CriarProcessamentoRequest request)
    {
        try
        {
            var comando = new CriarProcessamentoComando
            {
                ScriptId = request.ScriptId,
                DadosEntrada = request.DadosEntrada
            };

            var processamento = await _mediator.Send(comando);
            return CreatedAtAction(nameof(ObterProcessamento), new { id = processamento.Id }, processamento);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public record CriarProcessamentoRequest
{
    public int ScriptId { get; init; }
    public string DadosEntrada { get; init; } = string.Empty;
}
