using Datarisk.Core.Entities;

namespace Datarisk.Core.Interfaces;

public interface IScriptRepository
{
    Task<Script?> GetByIdAsync(int id);
    Task<IEnumerable<Script>> GetAllAsync();
    Task<Script> CreateAsync(Script script);
    Task<Script> UpdateAsync(Script script);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
