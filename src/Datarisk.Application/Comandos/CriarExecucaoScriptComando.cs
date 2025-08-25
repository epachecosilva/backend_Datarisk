using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;
using MediatR;

namespace Datarisk.Application.Comandos;

public class CriarExecucaoScriptComando : IRequest<ExecucaoScript>
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string CodigoScript { get; set; } = string.Empty;
    public string DadosTeste { get; set; } = string.Empty;
    public string? ResultadoEsperado { get; set; }
    public string Categoria { get; set; } = string.Empty;
}

public class CriarExecucaoScriptComandoHandler : IRequestHandler<CriarExecucaoScriptComando, ExecucaoScript>
{
    private readonly IRepositorioExecucaoScript _repositorioExecucaoScript;
    private readonly IServicoExecucaoScript _servicoExecucaoScript;

    public CriarExecucaoScriptComandoHandler(
        IRepositorioExecucaoScript repositorioExecucaoScript,
        IServicoExecucaoScript servicoExecucaoScript)
    {
        _repositorioExecucaoScript = repositorioExecucaoScript;
        _servicoExecucaoScript = servicoExecucaoScript;
    }

    public async Task<ExecucaoScript> Handle(CriarExecucaoScriptComando request, CancellationToken cancellationToken)
    {
        // Validar o script
        var ehValido = await _servicoExecucaoScript.ValidarScriptAsync(request.CodigoScript);
        if (!ehValido)
        {
            throw new InvalidOperationException("Script inválido");
        }

        // Obter próxima versão
        var proximaVersao = await _repositorioExecucaoScript.ObterProximaVersaoAsync(request.Nome);

        var execucaoScript = new ExecucaoScript
        {
            Nome = request.Nome,
            Descricao = request.Descricao,
            CodigoScript = request.CodigoScript,
            DadosTeste = request.DadosTeste,
            ResultadoEsperado = request.ResultadoEsperado,
            Categoria = request.Categoria,
            Versao = proximaVersao,
            Ativo = true
        };

        return await _repositorioExecucaoScript.CriarAsync(execucaoScript);
    }
}
