using Datarisk.Core.Entities;

namespace Datarisk.Core.Interfaces;

public interface IRepositorioProcessamento
{
    Task<IEnumerable<Processamento>> ObterTodosAsync();
    Task<Processamento?> ObterPorIdAsync(int id);
            Task<IEnumerable<Processamento>> ObterPorScriptIdAsync(int scriptId);
    Task<Processamento> CriarAsync(Processamento processamento);
    Task<Processamento> AtualizarAsync(Processamento processamento);
    Task<bool> ExisteAsync(int id);
}
