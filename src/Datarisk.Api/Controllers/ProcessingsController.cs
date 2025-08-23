using Datarisk.Application.Commands;
using Datarisk.Application.Queries;
using Datarisk.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Datarisk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProcessingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProcessingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all processings
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Processing>>> GetProcessings()
    {
        var processings = await _mediator.Send(new GetAllProcessingsQuery());
        return Ok(processings);
    }

    /// <summary>
    /// Get processing by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Processing>> GetProcessing(int id)
    {
        var processing = await _mediator.Send(new GetProcessingQuery(id));
        if (processing == null)
            return NotFound();

        return Ok(processing);
    }

    /// <summary>
    /// Get processings by script ID
    /// </summary>
    [HttpGet("script/{scriptId}")]
    public async Task<ActionResult<IEnumerable<Processing>>> GetProcessingsByScript(int scriptId)
    {
        var processings = await _mediator.Send(new GetProcessingsByScriptQuery(scriptId));
        return Ok(processings);
    }

    /// <summary>
    /// Create a new processing
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Processing>> CreateProcessing([FromBody] CreateProcessingRequest request)
    {
        try
        {
            var command = new CreateProcessingCommand
            {
                ScriptId = request.ScriptId,
                InputData = request.InputData
            };

            var processing = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProcessing), new { id = processing.Id }, processing);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public record CreateProcessingRequest
{
    public int ScriptId { get; init; }
    public string InputData { get; init; } = string.Empty;
}
