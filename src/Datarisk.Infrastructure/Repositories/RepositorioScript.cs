using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;
using Datarisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Datarisk.Infrastructure.Repositories;

public class RepositorioScript : IRepositorioScript
{
    private readonly ContextoDatarisk _context;

    public RepositorioScript(ContextoDatarisk context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Script>> ObterTodosAsync()
    {
        return await _context.Scripts
            .OrderByDescending(s => s.CriadoEm)
            .ToListAsync();
    }

    public async Task<Script?> ObterPorIdAsync(int id)
    {
        return await _context.Scripts
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Script> CriarAsync(Script script)
    {
        _context.Scripts.Add(script);
        await _context.SaveChangesAsync();
        return script;
    }

    public async Task<Script> AtualizarAsync(Script script)
    {
        _context.Scripts.Update(script);
        await _context.SaveChangesAsync();
        return script;
    }

    public async Task DeletarAsync(int id)
    {
        var script = await _context.Scripts.FindAsync(id);
        if (script != null)
        {
            _context.Scripts.Remove(script);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Scripts.AnyAsync(s => s.Id == id);
    }
}
