using MediatR;
using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;

namespace Datarisk.Application.Consultas;

public record ObterScriptQuery(int Id) : IRequest<Script?>;

public class ObterScriptQueryHandler : IRequestHandler<ObterScriptQuery, Script?>
{
    private readonly IRepositorioScript _repositorioScript;

    public ObterScriptQueryHandler(IRepositorioScript repositorioScript)
    {
        _repositorioScript = repositorioScript;
    }

    public async Task<Script?> Handle(ObterScriptQuery request, CancellationToken cancellationToken)
    {
        return await _repositorioScript.ObterPorIdAsync(request.Id);
    }
}

public record ObterTodosScriptsQuery : IRequest<IEnumerable<Script>>;

public class ObterTodosScriptsQueryHandler : IRequestHandler<ObterTodosScriptsQuery, IEnumerable<Script>>
{
    private readonly IRepositorioScript _repositorioScript;

    public ObterTodosScriptsQueryHandler(IRepositorioScript repositorioScript)
    {
        _repositorioScript = repositorioScript;
    }

    public async Task<IEnumerable<Script>> Handle(ObterTodosScriptsQuery request, CancellationToken cancellationToken)
    {
        return await _repositorioScript.ObterTodosAsync();
    }
}
