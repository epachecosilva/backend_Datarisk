using Datarisk.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Datarisk.Infrastructure.Data;

public class DatariskDbContext : DbContext
{
    public DatariskDbContext(DbContextOptions<DatariskDbContext> options) : base(options)
    {
    }

    public DbSet<Script> Scripts { get; set; }
    public DbSet<Processing> Processings { get; set; }
    public DbSet<ScriptExecution> ScriptExecutions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Script configuration
        modelBuilder.Entity<Script>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Code).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");
            
            // Configurar conversores de DateTime para garantir UTC
            entity.Property(e => e.CreatedAt).HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            entity.Property(e => e.UpdatedAt).HasConversion(
                v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);
        });

        // Processing configuration
        modelBuilder.Entity<Processing>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InputData).IsRequired();
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.StartedAt).HasColumnType("timestamp with time zone");
            entity.Property(e => e.CompletedAt).HasColumnType("timestamp with time zone");
            
            // Configurar conversores de DateTime para garantir UTC
            entity.Property(e => e.CreatedAt).HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            entity.Property(e => e.StartedAt).HasConversion(
                v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);
            entity.Property(e => e.CompletedAt).HasConversion(
                v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);
            
            entity.HasOne(e => e.Script)
                  .WithMany(s => s.Processings)
                  .HasForeignKey(e => e.ScriptId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ScriptExecution configuration
        modelBuilder.Entity<ScriptExecution>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ScriptCode).IsRequired();
            entity.Property(e => e.TestData).IsRequired();
            entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Version).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.ExecutionTimeMs).IsRequired(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ExecutedAt).HasColumnType("timestamp with time zone");
            
            // Configurar conversores de DateTime para garantir UTC
            entity.Property(e => e.CreatedAt).HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            entity.Property(e => e.ExecutedAt).HasConversion(
                v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);
            
            entity.HasOne(e => e.Processing)
                .WithMany()
                .HasForeignKey(e => e.ProcessingId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
