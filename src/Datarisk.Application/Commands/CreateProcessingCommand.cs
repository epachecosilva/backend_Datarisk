using MediatR;
using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;

namespace Datarisk.Application.Commands;

public record CreateProcessingCommand : IRequest<Processing>
{
    public int ScriptId { get; init; }
    public string InputData { get; init; } = string.Empty;
}

public class CreateProcessingCommandHandler : IRequestHandler<CreateProcessingCommand, Processing>
{
    private readonly IScriptRepository _scriptRepository;
    private readonly IProcessingRepository _processingRepository;
    private readonly IScriptExecutionService _scriptExecutionService;

    public CreateProcessingCommandHandler(
        IScriptRepository scriptRepository,
        IProcessingRepository processingRepository,
        IScriptExecutionService scriptExecutionService)
    {
        _scriptRepository = scriptRepository;
        _processingRepository = processingRepository;
        _scriptExecutionService = scriptExecutionService;
    }

    public async Task<Processing> Handle(CreateProcessingCommand request, CancellationToken cancellationToken)
    {
        // Verify script exists
        var script = await _scriptRepository.GetByIdAsync(request.ScriptId);
        if (script == null)
        {
            throw new InvalidOperationException($"Script with ID {request.ScriptId} not found.");
        }

        // Create processing record
        var processing = new Processing
        {
            ScriptId = request.ScriptId,
            InputData = request.InputData,
            Status = ProcessingStatus.Pending
        };

        var createdProcessing = await _processingRepository.CreateAsync(processing);

        // Start background processing
        _ = Task.Run(async () =>
        {
            try
            {
                // Update status to running
                createdProcessing.Status = ProcessingStatus.Running;
                createdProcessing.StartedAt = DateTime.UtcNow;
                await _processingRepository.UpdateAsync(createdProcessing);

                // Execute script
                var result = await _scriptExecutionService.ExecuteScriptAsync(script.Code, request.InputData);

                // Update with success
                createdProcessing.Status = ProcessingStatus.Completed;
                createdProcessing.OutputData = result;
                createdProcessing.CompletedAt = DateTime.UtcNow;
                await _processingRepository.UpdateAsync(createdProcessing);
            }
            catch (Exception ex)
            {
                // Update with error
                createdProcessing.Status = ProcessingStatus.Failed;
                createdProcessing.ErrorMessage = ex.Message;
                createdProcessing.CompletedAt = DateTime.UtcNow;
                await _processingRepository.UpdateAsync(createdProcessing);
            }
        }, cancellationToken);

        return createdProcessing;
    }
}
