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

    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        var query = new ObterTodasExecucoesScriptQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var query = new ObterExecucaoScriptQuery { Id = id };
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    [HttpGet("categoria/{categoria}")]
    public async Task<IActionResult> ObterPorCategoria(string categoria)
    {
        var query = new ObterExecucoesScriptPorCategoriaQuery { Categoria = categoria };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

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
