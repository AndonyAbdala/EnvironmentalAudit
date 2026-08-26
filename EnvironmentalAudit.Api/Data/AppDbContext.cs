using EnvironmentalAudit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnvironmentalAudit.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Audit> Audits => Set<Audit>();

    public DbSet<AuditData> AuditData => Set<AuditData>();

    public DbSet<AuditResult> AuditResults => Set<AuditResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audit>()
            .HasOne(a => a.Data)
            .WithOne(d => d.Audit)
            .HasForeignKey<AuditData>(d => d.AuditId);

        modelBuilder.Entity<Audit>()
            .HasOne(a => a.Result)
            .WithOne(r => r.Audit)
            .HasForeignKey<AuditResult>(r => r.AuditId);
    }
}