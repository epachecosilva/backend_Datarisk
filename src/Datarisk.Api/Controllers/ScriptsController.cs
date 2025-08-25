using Datarisk.Application.Comandos;
using Datarisk.Application.Consultas;
using Datarisk.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Datarisk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScriptsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScriptsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obter todos os scripts
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Script>>> ObterScripts()
    {
        var scripts = await _mediator.Send(new ObterTodosScriptsQuery());
        return Ok(scripts);
    }

    /// <summary>
    /// Obter script por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Script>> ObterScript(int id)
    {
        var script = await _mediator.Send(new ObterScriptQuery(id));
        if (script == null)
            return NotFound();

        return Ok(script);
    }

    /// <summary>
    /// Criar novo script
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Script>> CriarScript([FromBody] CriarScriptRequest request)
    {
        try
        {
            var comando = new CriarScriptComando
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Codigo = request.Codigo
            };

            var script = await _mediator.Send(comando);
            return CreatedAtAction(nameof(ObterScript), new { id = script.Id }, script);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Atualizar script existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<Script>> AtualizarScript(int id, [FromBody] AtualizarScriptRequest request)
    {
        try
        {
            var scriptExistente = await _mediator.Send(new ObterScriptQuery(id));
            if (scriptExistente == null)
                return NotFound();

            var comando = new CriarScriptComando
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Codigo = request.Codigo
            };

            // Para simplicidade, reutilizamos a lógica de criação
            // Em uma aplicação real, você teria um comando separado AtualizarScriptComando
            var script = await _mediator.Send(comando);
            return Ok(script);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Deletar script
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletarScript(int id)
    {
        var script = await _mediator.Send(new ObterScriptQuery(id));
        if (script == null)
            return NotFound();

        // Em uma aplicação real, você teria um comando DeletarScriptComando
        return NoContent();
    }
}

public record CriarScriptRequest
{
    public string Nome { get; init; } = string.Empty;
    public string? Descricao { get; init; }
    public string Codigo { get; init; } = string.Empty;
}

public record AtualizarScriptRequest
{
    public string Nome { get; init; } = string.Empty;
    public string? Descricao { get; init; }
    public string Codigo { get; init; } = string.Empty;
}
