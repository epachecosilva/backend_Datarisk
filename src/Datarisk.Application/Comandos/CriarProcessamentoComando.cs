using MediatR;
using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;

namespace Datarisk.Application.Comandos;

public record CriarProcessamentoComando : IRequest<Processamento>
{
    public int ScriptId { get; init; }
    public string DadosEntrada { get; init; } = string.Empty;
}

public class CriarProcessamentoComandoHandler : IRequestHandler<CriarProcessamentoComando, Processamento>
{
    private readonly IRepositorioScript _repositorioScript;
    private readonly IRepositorioProcessamento _repositorioProcessamento;
    private readonly IServicoExecucaoScript _servicoExecucaoScript;

    public CriarProcessamentoComandoHandler(
        IRepositorioScript repositorioScript,
        IRepositorioProcessamento repositorioProcessamento,
        IServicoExecucaoScript servicoExecucaoScript)
    {
        _repositorioScript = repositorioScript;
        _repositorioProcessamento = repositorioProcessamento;
        _servicoExecucaoScript = servicoExecucaoScript;
    }

    public async Task<Processamento> Handle(CriarProcessamentoComando request, CancellationToken cancellationToken)
    {
        // Verificar se o script existe
        var script = await _repositorioScript.ObterPorIdAsync(request.ScriptId);
        if (script == null)
        {
            throw new InvalidOperationException($"Script com ID {request.ScriptId} não encontrado.");
        }

        // Criar registro de processamento
        var processamento = new Processamento
        {
            ScriptId = request.ScriptId,
            DadosEntrada = request.DadosEntrada,
            Status = StatusProcessamento.Pendente
        };

        var processamentoCriado = await _repositorioProcessamento.CriarAsync(processamento);

        // Iniciar processamento em background
        _ = Task.Run(async () =>
        {
            try
            {
                // Atualizar status para executando
                processamentoCriado.Status = StatusProcessamento.Executando;
                processamentoCriado.IniciadoEm = DateTime.UtcNow;
                await _repositorioProcessamento.AtualizarAsync(processamentoCriado);

                // Executar script
                var resultado = await _servicoExecucaoScript.ExecutarScriptAsync(script.Codigo, request.DadosEntrada);

                // Atualizar com sucesso
                processamentoCriado.Status = StatusProcessamento.Concluido;
                processamentoCriado.DadosSaida = resultado;
                processamentoCriado.ConcluidoEm = DateTime.UtcNow;
                await _repositorioProcessamento.AtualizarAsync(processamentoCriado);
            }
            catch (Exception ex)
            {
                // Atualizar com erro
                processamentoCriado.Status = StatusProcessamento.Falhou;
                processamentoCriado.MensagemErro = ex.Message;
                processamentoCriado.ConcluidoEm = DateTime.UtcNow;
                await _repositorioProcessamento.AtualizarAsync(processamentoCriado);
            }
        }, cancellationToken);

        return processamentoCriado;
    }
}
