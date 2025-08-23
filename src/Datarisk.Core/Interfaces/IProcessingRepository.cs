using Datarisk.Core.Entities;

namespace Datarisk.Core.Interfaces;

public interface IProcessingRepository
{
    Task<Processing?> GetByIdAsync(int id);
    Task<IEnumerable<Processing>> GetAllAsync();
    Task<IEnumerable<Processing>> GetByScriptIdAsync(int scriptId);
    Task<Processing> CreateAsync(Processing processing);
    Task<Processing> UpdateAsync(Processing processing);
    Task<bool> ExistsAsync(int id);
}
