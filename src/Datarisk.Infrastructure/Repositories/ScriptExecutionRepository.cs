using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;
using Datarisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Datarisk.Infrastructure.Repositories;

public class ScriptExecutionRepository : IScriptExecutionRepository
{
    private readonly DatariskDbContext _context;

    public ScriptExecutionRepository(DatariskDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ScriptExecution>> GetAllAsync()
    {
        return await _context.ScriptExecutions
            .Include(se => se.Processing)
            .OrderByDescending(se => se.CreatedAt)
            .ToListAsync();
    }

    public async Task<ScriptExecution?> GetByIdAsync(int id)
    {
        return await _context.ScriptExecutions
            .Include(se => se.Processing)
            .FirstOrDefaultAsync(se => se.Id == id);
    }

    public async Task<IEnumerable<ScriptExecution>> GetByCategoryAsync(string category)
    {
        return await _context.ScriptExecutions
            .Include(se => se.Processing)
            .Where(se => se.Category == category)
            .OrderByDescending(se => se.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ScriptExecution>> GetActiveAsync()
    {
        return await _context.ScriptExecutions
            .Include(se => se.Processing)
            .Where(se => se.IsActive)
            .OrderByDescending(se => se.CreatedAt)
            .ToListAsync();
    }

    public async Task<ScriptExecution> CreateAsync(ScriptExecution scriptExecution)
    {
        scriptExecution.CreatedAt = DateTime.UtcNow;
        _context.ScriptExecutions.Add(scriptExecution);
        await _context.SaveChangesAsync();
        return scriptExecution;
    }

    public async Task<ScriptExecution> UpdateAsync(ScriptExecution scriptExecution)
    {
        _context.ScriptExecutions.Update(scriptExecution);
        await _context.SaveChangesAsync();
        return scriptExecution;
    }

    public async Task DeleteAsync(int id)
    {
        var scriptExecution = await _context.ScriptExecutions.FindAsync(id);
        if (scriptExecution != null)
        {
            _context.ScriptExecutions.Remove(scriptExecution);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> GetNextVersionAsync(string name)
    {
        var maxVersion = await _context.ScriptExecutions
            .Where(se => se.Name == name)
            .MaxAsync(se => (int?)se.Version) ?? 0;
        
        return maxVersion + 1;
    }
}
