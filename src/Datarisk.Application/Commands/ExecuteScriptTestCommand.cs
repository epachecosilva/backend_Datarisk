using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;
using MediatR;

namespace Datarisk.Application.Commands;

public class ExecuteScriptTestCommand : IRequest<ScriptExecution>
{
    public int ScriptExecutionId { get; set; }
}

public class ExecuteScriptTestCommandHandler : IRequestHandler<ExecuteScriptTestCommand, ScriptExecution>
{
    private readonly IScriptExecutionRepository _scriptExecutionRepository;
    private readonly IScriptExecutionService _scriptExecutionService;

    public ExecuteScriptTestCommandHandler(
        IScriptExecutionRepository scriptExecutionRepository,
        IScriptExecutionService scriptExecutionService)
    {
        _scriptExecutionRepository = scriptExecutionRepository;
        _scriptExecutionService = scriptExecutionService;
    }

    public async Task<ScriptExecution> Handle(ExecuteScriptTestCommand request, CancellationToken cancellationToken)
    {
        var scriptExecution = await _scriptExecutionRepository.GetByIdAsync(request.ScriptExecutionId);
        if (scriptExecution == null)
        {
            throw new InvalidOperationException("Execução de script não encontrada");
        }

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        try
        {
            // Executar o script com os dados de teste
            var result = await _scriptExecutionService.ExecuteScriptAsync(scriptExecution.ScriptCode, scriptExecution.TestData);
            
            stopwatch.Stop();
            
            // Atualizar a execução
            scriptExecution.ActualResult = result;
            scriptExecution.IsSuccessful = true;
            scriptExecution.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
            scriptExecution.ExecutedAt = DateTime.UtcNow;
            
            return await _scriptExecutionRepository.UpdateAsync(scriptExecution);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            
            scriptExecution.IsSuccessful = false;
            scriptExecution.ErrorMessage = ex.Message;
            scriptExecution.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
            scriptExecution.ExecutedAt = DateTime.UtcNow;
            
            return await _scriptExecutionRepository.UpdateAsync(scriptExecution);
        }
    }
}
