using MediatR;
using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;

namespace Datarisk.Application.Queries;

public record GetScriptQuery(int Id) : IRequest<Script?>;

public class GetScriptQueryHandler : IRequestHandler<GetScriptQuery, Script?>
{
    private readonly IScriptRepository _scriptRepository;

    public GetScriptQueryHandler(IScriptRepository scriptRepository)
    {
        _scriptRepository = scriptRepository;
    }

    public async Task<Script?> Handle(GetScriptQuery request, CancellationToken cancellationToken)
    {
        return await _scriptRepository.GetByIdAsync(request.Id);
    }
}

public record GetAllScriptsQuery : IRequest<IEnumerable<Script>>;

public class GetAllScriptsQueryHandler : IRequestHandler<GetAllScriptsQuery, IEnumerable<Script>>
{
    private readonly IScriptRepository _scriptRepository;

    public GetAllScriptsQueryHandler(IScriptRepository scriptRepository)
    {
        _scriptRepository = scriptRepository;
    }

    public async Task<IEnumerable<Script>> Handle(GetAllScriptsQuery request, CancellationToken cancellationToken)
    {
        return await _scriptRepository.GetAllAsync();
    }
}
