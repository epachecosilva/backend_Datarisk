using Datarisk.Core.Entities;

namespace Datarisk.Core.Interfaces;

public interface IScriptExecutionRepository
{
    Task<IEnumerable<ScriptExecution>> GetAllAsync();
    Task<ScriptExecution?> GetByIdAsync(int id);
    Task<IEnumerable<ScriptExecution>> GetByCategoryAsync(string category);
    Task<IEnumerable<ScriptExecution>> GetActiveAsync();
    Task<ScriptExecution> CreateAsync(ScriptExecution scriptExecution);
    Task<ScriptExecution> UpdateAsync(ScriptExecution scriptExecution);
    Task DeleteAsync(int id);
    Task<int> GetNextVersionAsync(string name);
}
