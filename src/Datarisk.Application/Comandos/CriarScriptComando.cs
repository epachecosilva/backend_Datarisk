using MediatR;
using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;

namespace Datarisk.Application.Comandos;

public record CriarScriptComando : IRequest<Script>
{
    public string Nome { get; init; } = string.Empty;
    public string? Descricao { get; init; }
    public string Codigo { get; init; } = string.Empty;
}

public class CriarScriptComandoHandler : IRequestHandler<CriarScriptComando, Script>
{
    private readonly IRepositorioScript _repositorioScript;
    private readonly IServicoExecucaoScript _servicoExecucaoScript;

    public CriarScriptComandoHandler(IRepositorioScript repositorioScript, IServicoExecucaoScript servicoExecucaoScript)
    {
        _repositorioScript = repositorioScript;
        _servicoExecucaoScript = servicoExecucaoScript;
    }

    public async Task<Script> Handle(CriarScriptComando request, CancellationToken cancellationToken)
    {
        // Validar sintaxe do script
        var ehValido = await _servicoExecucaoScript.ValidarScriptAsync(request.Codigo);
        if (!ehValido)
        {
            throw new InvalidOperationException("Código JavaScript inválido. O script deve conter uma função 'process'.");
        }

        var script = new Script
        {
            Nome = request.Nome,
            Descricao = request.Descricao,
            Codigo = request.Codigo
        };

        return await _repositorioScript.CriarAsync(script);
    }
}
