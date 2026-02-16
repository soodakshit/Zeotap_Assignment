using Microsoft.EntityFrameworkCore;
using IncidentTracker.Api.Models;

namespace IncidentTracker.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Incident> Incidents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Service)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Severity)
            .IsRequired()
            .HasConversion(
            v => v.ToString(),
            v => (Models.Severity)Enum.Parse(typeof(Models.Severity), v));

            entity.Property(e => e.Status)
    .IsRequired()
    .HasConversion(
        v => v.ToString(),
        v => (Models.Status)Enum.Parse(typeof(Models.Status), v));

            entity.Property(e => e.Owner)
                .HasMaxLength(100);

            entity.Property(e => e.Summary)
                .HasMaxLength(2000);

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Indexes for performance
            entity.HasIndex(e => e.Service);
            entity.HasIndex(e => e.Severity);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedAt);
        });
    }
}
