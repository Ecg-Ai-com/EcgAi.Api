using Microsoft.EntityFrameworkCore;

namespace EcgAi.Api.Data.Database.Cosmos;

public class TrainingDbContext : DbContext
{

    public TrainingDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<EcgRecord>? EcgRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EcgRecord>()
            .HasNoDiscriminator()
            .ToContainer(nameof(EcgRecords))
            .HasKey(d => d.Id);
    }
}