using Datarisk.Core.Entities;
using Datarisk.Core.Interfaces;
using Datarisk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Datarisk.Infrastructure.Repositories;

public class RepositorioExecucaoScript : IRepositorioExecucaoScript
{
    private readonly ContextoDatarisk _context;

    public RepositorioExecucaoScript(ContextoDatarisk context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExecucaoScript>> ObterTodosAsync()
    {
        return await _context.ExecucoesScript
            .Include(es => es.Processamento)
            .OrderByDescending(es => es.CriadoEm)
            .ToListAsync();
    }

    public async Task<ExecucaoScript?> ObterPorIdAsync(int id)
    {
        return await _context.ExecucoesScript
            .Include(es => es.Processamento)
            .FirstOrDefaultAsync(es => es.Id == id);
    }

    public async Task<IEnumerable<ExecucaoScript>> ObterPorCategoriaAsync(string categoria)
    {
        return await _context.ExecucoesScript
            .Include(es => es.Processamento)
            .Where(es => es.Categoria == categoria)
            .OrderByDescending(es => es.CriadoEm)
            .ToListAsync();
    }

    public async Task<IEnumerable<ExecucaoScript>> ObterAtivosAsync()
    {
        return await _context.ExecucoesScript
            .Include(es => es.Processamento)
            .Where(es => es.Ativo)
            .OrderByDescending(es => es.CriadoEm)
            .ToListAsync();
    }

    public async Task<ExecucaoScript> CriarAsync(ExecucaoScript execucaoScript)
    {
        execucaoScript.CriadoEm = DateTime.UtcNow;
        _context.ExecucoesScript.Add(execucaoScript);
        await _context.SaveChangesAsync();
        return execucaoScript;
    }

    public async Task<ExecucaoScript> AtualizarAsync(ExecucaoScript execucaoScript)
    {
        _context.ExecucoesScript.Update(execucaoScript);
        await _context.SaveChangesAsync();
        return execucaoScript;
    }

    public async Task DeletarAsync(int id)
    {
        var execucaoScript = await _context.ExecucoesScript.FindAsync(id);
        if (execucaoScript != null)
        {
            _context.ExecucoesScript.Remove(execucaoScript);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> ObterProximaVersaoAsync(string nome)
    {
        var ultimaVersao = await _context.ExecucoesScript
            .Where(es => es.Nome == nome)
            .MaxAsync(es => (int?)es.Versao);

        return (ultimaVersao ?? 0) + 1;
    }
}
