using Datarisk.Core.Entities;

namespace Datarisk.Core.Interfaces;

public interface IRepositorioExecucaoScript
{
    Task<IEnumerable<ExecucaoScript>> ObterTodosAsync();
    Task<ExecucaoScript?> ObterPorIdAsync(int id);
    Task<IEnumerable<ExecucaoScript>> ObterPorCategoriaAsync(string categoria);
    Task<IEnumerable<ExecucaoScript>> ObterAtivosAsync();
    Task<ExecucaoScript> CriarAsync(ExecucaoScript execucaoScript);
    Task<ExecucaoScript> AtualizarAsync(ExecucaoScript execucaoScript);
    Task DeletarAsync(int id);
    Task<int> ObterProximaVersaoAsync(string nome);
}
