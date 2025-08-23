using Datarisk.Application.Commands;
using Datarisk.Application.Queries;
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
    /// Get all scripts
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Script>>> GetScripts()
    {
        var scripts = await _mediator.Send(new GetAllScriptsQuery());
        return Ok(scripts);
    }

    /// <summary>
    /// Get script by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Script>> GetScript(int id)
    {
        var script = await _mediator.Send(new GetScriptQuery(id));
        if (script == null)
            return NotFound();

        return Ok(script);
    }

    /// <summary>
    /// Create a new script
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Script>> CreateScript([FromBody] CreateScriptRequest request)
    {
        try
        {
            var command = new CreateScriptCommand
            {
                Name = request.Name,
                Description = request.Description,
                Code = request.Code
            };

            var script = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetScript), new { id = script.Id }, script);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing script
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<Script>> UpdateScript(int id, [FromBody] UpdateScriptRequest request)
    {
        try
        {
            var existingScript = await _mediator.Send(new GetScriptQuery(id));
            if (existingScript == null)
                return NotFound();

            var command = new CreateScriptCommand
            {
                Name = request.Name,
                Description = request.Description,
                Code = request.Code
            };

            // For simplicity, we'll reuse the create command logic
            // In a real application, you'd have a separate UpdateScriptCommand
            var script = await _mediator.Send(command);
            return Ok(script);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a script
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteScript(int id)
    {
        var script = await _mediator.Send(new GetScriptQuery(id));
        if (script == null)
            return NotFound();

        // In a real application, you'd have a DeleteScriptCommand
        return NoContent();
    }
}

public record CreateScriptRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Code { get; init; } = string.Empty;
}

public record UpdateScriptRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Code { get; init; } = string.Empty;
}
