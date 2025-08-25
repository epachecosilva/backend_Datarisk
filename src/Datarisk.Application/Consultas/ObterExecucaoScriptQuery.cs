using MediatR;
using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;

namespace Datarisk.Application.Consultas;

public record ObterExecucaoScriptQuery : IRequest<ExecucaoScript?>
{
    public int Id { get; set; }
}

public class ObterExecucaoScriptQueryHandler : IRequestHandler<ObterExecucaoScriptQuery, ExecucaoScript?>
{
    private readonly IRepositorioExecucaoScript _repositorioExecucaoScript;

    public ObterExecucaoScriptQueryHandler(IRepositorioExecucaoScript repositorioExecucaoScript)
    {
        _repositorioExecucaoScript = repositorioExecucaoScript;
    }

    public async Task<ExecucaoScript?> Handle(ObterExecucaoScriptQuery request, CancellationToken cancellationToken)
    {
        return await _repositorioExecucaoScript.ObterPorIdAsync(request.Id);
    }
}

public record ObterTodasExecucoesScriptQuery : IRequest<IEnumerable<ExecucaoScript>>;

public class ObterTodasExecucoesScriptQueryHandler : IRequestHandler<ObterTodasExecucoesScriptQuery, IEnumerable<ExecucaoScript>>
{
    private readonly IRepositorioExecucaoScript _repositorioExecucaoScript;

    public ObterTodasExecucoesScriptQueryHandler(IRepositorioExecucaoScript repositorioExecucaoScript)
    {
        _repositorioExecucaoScript = repositorioExecucaoScript;
    }

    public async Task<IEnumerable<ExecucaoScript>> Handle(ObterTodasExecucoesScriptQuery request, CancellationToken cancellationToken)
    {
        return await _repositorioExecucaoScript.ObterTodosAsync();
    }
}

public record ObterExecucoesScriptPorCategoriaQuery : IRequest<IEnumerable<ExecucaoScript>>
{
    public string Categoria { get; set; } = string.Empty;
}

public class ObterExecucoesScriptPorCategoriaQueryHandler : IRequestHandler<ObterExecucoesScriptPorCategoriaQuery, IEnumerable<ExecucaoScript>>
{
    private readonly IRepositorioExecucaoScript _repositorioExecucaoScript;

    public ObterExecucoesScriptPorCategoriaQueryHandler(IRepositorioExecucaoScript repositorioExecucaoScript)
    {
        _repositorioExecucaoScript = repositorioExecucaoScript;
    }

    public async Task<IEnumerable<ExecucaoScript>> Handle(ObterExecucoesScriptPorCategoriaQuery request, CancellationToken cancellationToken)
    {
        return await _repositorioExecucaoScript.ObterPorCategoriaAsync(request.Categoria);
    }
}
