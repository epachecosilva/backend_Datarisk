using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;
using Datarisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Datarisk.Infrastructure.Repositories;

public class RepositorioProcessamento : IRepositorioProcessamento
{
    private readonly ContextoDatarisk _context;

    public RepositorioProcessamento(ContextoDatarisk context)
    {
        _context = context;
    }

    public async Task<Processamento?> ObterPorIdAsync(int id)
    {
        return await _context.Processamentos
            .Include(p => p.Script)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Processamento>> ObterTodosAsync()
    {
        return await _context.Processamentos
            .Include(p => p.Script)
            .OrderByDescending(p => p.CriadoEm)
            .ToListAsync();
    }

    public async Task<IEnumerable<Processamento>> ObterPorScriptIdAsync(int scriptId)
    {
        return await _context.Processamentos
            .Include(p => p.Script)
            .Where(p => p.ScriptId == scriptId)
            .OrderByDescending(p => p.CriadoEm)
            .ToListAsync();
    }

    public async Task<Processamento> CriarAsync(Processamento processamento)
    {
        _context.Processamentos.Add(processamento);
        await _context.SaveChangesAsync();
        return processamento;
    }

    public async Task<Processamento> AtualizarAsync(Processamento processamento)
    {
        _context.Processamentos.Update(processamento);
        await _context.SaveChangesAsync();
        return processamento;
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Processamentos.AnyAsync(p => p.Id == id);
    }
}
