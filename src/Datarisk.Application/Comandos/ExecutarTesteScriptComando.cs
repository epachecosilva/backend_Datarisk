using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;
using MediatR;

namespace Datarisk.Application.Comandos;

public class ExecutarTesteScriptComando : IRequest<ExecucaoScript>
{
    public int ExecucaoScriptId { get; set; }
}

public class ExecutarTesteScriptComandoHandler : IRequestHandler<ExecutarTesteScriptComando, ExecucaoScript>
{
    private readonly IRepositorioExecucaoScript _repositorioExecucaoScript;
    private readonly IServicoExecucaoScript _servicoExecucaoScript;

    public ExecutarTesteScriptComandoHandler(
        IRepositorioExecucaoScript repositorioExecucaoScript,
        IServicoExecucaoScript servicoExecucaoScript)
    {
        _repositorioExecucaoScript = repositorioExecucaoScript;
        _servicoExecucaoScript = servicoExecucaoScript;
    }

    public async Task<ExecucaoScript> Handle(ExecutarTesteScriptComando request, CancellationToken cancellationToken)
    {
        var execucaoScript = await _repositorioExecucaoScript.ObterPorIdAsync(request.ExecucaoScriptId);
        if (execucaoScript == null)
        {
            throw new InvalidOperationException($"Execução de script com ID {request.ExecucaoScriptId} não encontrada.");
        }

        var inicioExecucao = DateTime.UtcNow;
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            // Executar o script
            var resultado = await _servicoExecucaoScript.ExecutarScriptAsync(
                execucaoScript.CodigoScript, 
                execucaoScript.DadosTeste);

            stopwatch.Stop();

            // Atualizar com sucesso
            execucaoScript.Sucesso = true;
            execucaoScript.ResultadoReal = resultado;
            execucaoScript.TempoExecucaoMs = stopwatch.ElapsedMilliseconds;
            execucaoScript.ExecutadoEm = DateTime.UtcNow;
            execucaoScript.MensagemErro = null;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            // Atualizar com erro
            execucaoScript.Sucesso = false;
            execucaoScript.ResultadoReal = null;
            execucaoScript.TempoExecucaoMs = stopwatch.ElapsedMilliseconds;
            execucaoScript.ExecutadoEm = DateTime.UtcNow;
            execucaoScript.MensagemErro = ex.Message;
        }

        return await _repositorioExecucaoScript.AtualizarAsync(execucaoScript);
    }
}
