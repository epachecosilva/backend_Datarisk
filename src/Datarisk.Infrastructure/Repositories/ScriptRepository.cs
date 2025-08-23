using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;
using Datarisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Datarisk.Infrastructure.Repositories;

public class ScriptRepository : IScriptRepository
{
    private readonly DatariskDbContext _context;

    public ScriptRepository(DatariskDbContext context)
    {
        _context = context;
    }

    public async Task<Script?> GetByIdAsync(int id)
    {
        return await _context.Scripts.FindAsync(id);
    }

    public async Task<IEnumerable<Script>> GetAllAsync()
    {
        return await _context.Scripts.ToListAsync();
    }

    public async Task<Script> CreateAsync(Script script)
    {
        _context.Scripts.Add(script);
        await _context.SaveChangesAsync();
        return script;
    }

    public async Task<Script> UpdateAsync(Script script)
    {
        script.UpdatedAt = DateTime.UtcNow;
        _context.Scripts.Update(script);
        await _context.SaveChangesAsync();
        return script;
    }

    public async Task DeleteAsync(int id)
    {
        var script = await _context.Scripts.FindAsync(id);
        if (script != null)
        {
            _context.Scripts.Remove(script);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Scripts.AnyAsync(s => s.Id == id);
    }
}
