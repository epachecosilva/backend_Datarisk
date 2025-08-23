using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;
using Datarisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Datarisk.Infrastructure.Repositories;

public class ProcessingRepository : IProcessingRepository
{
    private readonly DatariskDbContext _context;

    public ProcessingRepository(DatariskDbContext context)
    {
        _context = context;
    }

    public async Task<Processing?> GetByIdAsync(int id)
    {
        return await _context.Processings
            .Include(p => p.Script)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Processing>> GetAllAsync()
    {
        return await _context.Processings
            .Include(p => p.Script)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Processing>> GetByScriptIdAsync(int scriptId)
    {
        return await _context.Processings
            .Include(p => p.Script)
            .Where(p => p.ScriptId == scriptId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Processing> CreateAsync(Processing processing)
    {
        _context.Processings.Add(processing);
        await _context.SaveChangesAsync();
        return processing;
    }

    public async Task<Processing> UpdateAsync(Processing processing)
    {
        _context.Processings.Update(processing);
        await _context.SaveChangesAsync();
        return processing;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Processings.AnyAsync(p => p.Id == id);
    }
}
