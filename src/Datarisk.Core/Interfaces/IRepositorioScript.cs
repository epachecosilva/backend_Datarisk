using Datarisk.Core.Entities;

namespace Datarisk.Core.Interfaces;

public interface IRepositorioScript
{
    Task<IEnumerable<Script>> ObterTodosAsync();
    Task<Script?> ObterPorIdAsync(int id);
    Task<Script> CriarAsync(Script script);
    Task<Script> AtualizarAsync(Script script);
    Task DeletarAsync(int id);
    Task<bool> ExisteAsync(int id);
}
