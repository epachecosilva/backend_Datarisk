using Datarisk.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Datarisk.Infrastructure.Data;

public class ContextoDatarisk : DbContext
{
    public ContextoDatarisk(DbContextOptions<ContextoDatarisk> options) : base(options)
    {
    }

    public DbSet<Script> Scripts { get; set; }
    public DbSet<Processamento> Processamentos { get; set; }
    public DbSet<ExecucaoScript> ExecucoesScript { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Script configuration
        modelBuilder.Entity<Script>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).HasMaxLength(500);
            entity.Property(e => e.Codigo).IsRequired();
            entity.Property(e => e.CriadoEm).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.AtualizadoEm).HasColumnType("timestamp with time zone");
            
            // Configurar conversores de DateTime para garantir UTC
            entity.Property(e => e.CriadoEm).HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            entity.Property(e => e.AtualizadoEm).HasConversion(
                v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);
        });

        // Processamento configuration
        modelBuilder.Entity<Processamento>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DadosEntrada).IsRequired();
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.CriadoEm).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IniciadoEm).HasColumnType("timestamp with time zone");
            entity.Property(e => e.ConcluidoEm).HasColumnType("timestamp with time zone");
            
            // Configurar conversores de DateTime para garantir UTC
            entity.Property(e => e.CriadoEm).HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            entity.Property(e => e.IniciadoEm).HasConversion(
                v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);
            entity.Property(e => e.ConcluidoEm).HasConversion(
                v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);
            
            entity.HasOne(e => e.Script)
                  .WithMany(s => s.Processamentos)
                  .HasForeignKey(e => e.ScriptId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ExecucaoScript configuration
        modelBuilder.Entity<ExecucaoScript>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).HasMaxLength(500);
            entity.Property(e => e.CodigoScript).IsRequired();
            entity.Property(e => e.DadosTeste).IsRequired();
            entity.Property(e => e.Categoria).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Versao).IsRequired();
            entity.Property(e => e.Ativo).IsRequired();
            entity.Property(e => e.TempoExecucaoMs).IsRequired(false);
            entity.Property(e => e.CriadoEm).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ExecutadoEm).HasColumnType("timestamp with time zone");
            
            // Configurar conversores de DateTime para garantir UTC
            entity.Property(e => e.CriadoEm).HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            entity.Property(e => e.ExecutadoEm).HasConversion(
                v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);
            
            entity.HasOne(e => e.Processamento)
                .WithMany()
                .HasForeignKey(e => e.ProcessamentoId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
