using MediatR;
using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;

namespace Datarisk.Application.Queries;

public record GetProcessingQuery(int Id) : IRequest<Processing?>;

public class GetProcessingQueryHandler : IRequestHandler<GetProcessingQuery, Processing?>
{
    private readonly IProcessingRepository _processingRepository;

    public GetProcessingQueryHandler(IProcessingRepository processingRepository)
    {
        _processingRepository = processingRepository;
    }

    public async Task<Processing?> Handle(GetProcessingQuery request, CancellationToken cancellationToken)
    {
        return await _processingRepository.GetByIdAsync(request.Id);
    }
}

public record GetAllProcessingsQuery : IRequest<IEnumerable<Processing>>;

public class GetAllProcessingsQueryHandler : IRequestHandler<GetAllProcessingsQuery, IEnumerable<Processing>>
{
    private readonly IProcessingRepository _processingRepository;

    public GetAllProcessingsQueryHandler(IProcessingRepository processingRepository)
    {
        _processingRepository = processingRepository;
    }

    public async Task<IEnumerable<Processing>> Handle(GetAllProcessingsQuery request, CancellationToken cancellationToken)
    {
        return await _processingRepository.GetAllAsync();
    }
}

public record GetProcessingsByScriptQuery(int ScriptId) : IRequest<IEnumerable<Processing>>;

public class GetProcessingsByScriptQueryHandler : IRequestHandler<GetProcessingsByScriptQuery, IEnumerable<Processing>>
{
    private readonly IProcessingRepository _processingRepository;

    public GetProcessingsByScriptQueryHandler(IProcessingRepository processingRepository)
    {
        _processingRepository = processingRepository;
    }

    public async Task<IEnumerable<Processing>> Handle(GetProcessingsByScriptQuery request, CancellationToken cancellationToken)
    {
        return await _processingRepository.GetByScriptIdAsync(request.ScriptId);
    }
}
