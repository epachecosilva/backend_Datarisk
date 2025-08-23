using MediatR;
using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;

namespace Datarisk.Application.Commands;

public record CreateScriptCommand : IRequest<Script>
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Code { get; init; } = string.Empty;
}

public class CreateScriptCommandHandler : IRequestHandler<CreateScriptCommand, Script>
{
    private readonly IScriptRepository _scriptRepository;
    private readonly IScriptExecutionService _scriptExecutionService;

    public CreateScriptCommandHandler(IScriptRepository scriptRepository, IScriptExecutionService scriptExecutionService)
    {
        _scriptRepository = scriptRepository;
        _scriptExecutionService = scriptExecutionService;
    }

    public async Task<Script> Handle(CreateScriptCommand request, CancellationToken cancellationToken)
    {
        // Validate script syntax
        var isValid = await _scriptExecutionService.ValidateScriptAsync(request.Code);
        if (!isValid)
        {
            throw new InvalidOperationException("Invalid JavaScript code. Script must contain a 'process' function.");
        }

        var script = new Script
        {
            Name = request.Name,
            Description = request.Description,
            Code = request.Code
        };

        return await _scriptRepository.CreateAsync(script);
    }
}
