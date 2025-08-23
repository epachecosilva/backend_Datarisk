using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;
using MediatR;

namespace Datarisk.Application.Queries;

public class GetScriptExecutionQuery : IRequest<ScriptExecution?>
{
    public int Id { get; set; }
}

public class GetScriptExecutionQueryHandler : IRequestHandler<GetScriptExecutionQuery, ScriptExecution?>
{
    private readonly IScriptExecutionRepository _scriptExecutionRepository;

    public GetScriptExecutionQueryHandler(IScriptExecutionRepository scriptExecutionRepository)
    {
        _scriptExecutionRepository = scriptExecutionRepository;
    }

    public async Task<ScriptExecution?> Handle(GetScriptExecutionQuery request, CancellationToken cancellationToken)
    {
        return await _scriptExecutionRepository.GetByIdAsync(request.Id);
    }
}

public class GetAllScriptExecutionsQuery : IRequest<IEnumerable<ScriptExecution>>
{
}

public class GetAllScriptExecutionsQueryHandler : IRequestHandler<GetAllScriptExecutionsQuery, IEnumerable<ScriptExecution>>
{
    private readonly IScriptExecutionRepository _scriptExecutionRepository;

    public GetAllScriptExecutionsQueryHandler(IScriptExecutionRepository scriptExecutionRepository)
    {
        _scriptExecutionRepository = scriptExecutionRepository;
    }

    public async Task<IEnumerable<ScriptExecution>> Handle(GetAllScriptExecutionsQuery request, CancellationToken cancellationToken)
    {
        return await _scriptExecutionRepository.GetAllAsync();
    }
}

public class GetScriptExecutionsByCategoryQuery : IRequest<IEnumerable<ScriptExecution>>
{
    public string Category { get; set; } = string.Empty;
}

public class GetScriptExecutionsByCategoryQueryHandler : IRequestHandler<GetScriptExecutionsByCategoryQuery, IEnumerable<ScriptExecution>>
{
    private readonly IScriptExecutionRepository _scriptExecutionRepository;

    public GetScriptExecutionsByCategoryQueryHandler(IScriptExecutionRepository scriptExecutionRepository)
    {
        _scriptExecutionRepository = scriptExecutionRepository;
    }

    public async Task<IEnumerable<ScriptExecution>> Handle(GetScriptExecutionsByCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _scriptExecutionRepository.GetByCategoryAsync(request.Category);
    }
}
