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
        });

        // Processing configuration
        modelBuilder.Entity<Processing>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InputData).IsRequired();
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.Script)
                  .WithMany(s => s.Processings)
                  .HasForeignKey(e => e.ScriptId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
