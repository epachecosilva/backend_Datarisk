using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;
using MediatR;

namespace Datarisk.Application.Commands;

public class CreateScriptExecutionCommand : IRequest<ScriptExecution>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ScriptCode { get; set; } = string.Empty;
    public string TestData { get; set; } = string.Empty;
    public string? ExpectedResult { get; set; }
    public string Category { get; set; } = string.Empty;
}

public class CreateScriptExecutionCommandHandler : IRequestHandler<CreateScriptExecutionCommand, ScriptExecution>
{
    private readonly IScriptExecutionRepository _scriptExecutionRepository;
    private readonly IScriptExecutionService _scriptExecutionService;

    public CreateScriptExecutionCommandHandler(
        IScriptExecutionRepository scriptExecutionRepository,
        IScriptExecutionService scriptExecutionService)
    {
        _scriptExecutionRepository = scriptExecutionRepository;
        _scriptExecutionService = scriptExecutionService;
    }

    public async Task<ScriptExecution> Handle(CreateScriptExecutionCommand request, CancellationToken cancellationToken)
    {
        // Validar o script
        var isValid = await _scriptExecutionService.ValidateScriptAsync(request.ScriptCode);
        if (!isValid)
        {
            throw new InvalidOperationException("Script inválido");
        }

        // Obter próxima versão
        var nextVersion = await _scriptExecutionRepository.GetNextVersionAsync(request.Name);

        var scriptExecution = new ScriptExecution
        {
            Name = request.Name,
            Description = request.Description,
            ScriptCode = request.ScriptCode,
            TestData = request.TestData,
            ExpectedResult = request.ExpectedResult,
            Category = request.Category,
            Version = nextVersion,
            IsActive = true
        };

        return await _scriptExecutionRepository.CreateAsync(scriptExecution);
    }
}
