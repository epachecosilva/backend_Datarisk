using MediatR;
using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;

namespace Datarisk.Application.Consultas;

public record ObterProcessamentoQuery(int Id) : IRequest<Processamento?>;

public class ObterProcessamentoQueryHandler : IRequestHandler<ObterProcessamentoQuery, Processamento?>
{
    private readonly IRepositorioProcessamento _repositorioProcessamento;

    public ObterProcessamentoQueryHandler(IRepositorioProcessamento repositorioProcessamento)
    {
        _repositorioProcessamento = repositorioProcessamento;
    }

    public async Task<Processamento?> Handle(ObterProcessamentoQuery request, CancellationToken cancellationToken)
    {
        return await _repositorioProcessamento.ObterPorIdAsync(request.Id);
    }
}

public record ObterTodosProcessamentosQuery : IRequest<IEnumerable<Processamento>>;

public class ObterTodosProcessamentosQueryHandler : IRequestHandler<ObterTodosProcessamentosQuery, IEnumerable<Processamento>>
{
    private readonly IRepositorioProcessamento _repositorioProcessamento;

    public ObterTodosProcessamentosQueryHandler(IRepositorioProcessamento repositorioProcessamento)
    {
        _repositorioProcessamento = repositorioProcessamento;
    }

    public async Task<IEnumerable<Processamento>> Handle(ObterTodosProcessamentosQuery request, CancellationToken cancellationToken)
    {
        return await _repositorioProcessamento.ObterTodosAsync();
    }
}

public record ObterProcessamentosPorScriptQuery(int ScriptId) : IRequest<IEnumerable<Processamento>>;

public class ObterProcessamentosPorScriptQueryHandler : IRequestHandler<ObterProcessamentosPorScriptQuery, IEnumerable<Processamento>>
{
    private readonly IRepositorioProcessamento _repositorioProcessamento;

    public ObterProcessamentosPorScriptQueryHandler(IRepositorioProcessamento repositorioProcessamento)
    {
        _repositorioProcessamento = repositorioProcessamento;
    }

    public async Task<IEnumerable<Processamento>> Handle(ObterProcessamentosPorScriptQuery request, CancellationToken cancellationToken)
    {
        return await _repositorioProcessamento.ObterPorScriptIdAsync(request.ScriptId);
    }
}
