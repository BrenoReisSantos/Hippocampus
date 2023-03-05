using Hippocampus.Models.Entities;
using Hippocampus.Models.Values;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Models.Context;

public class HippocampusContext : DbContext
{
    public DbSet<RecipientLog> RecipientLogs => Set<RecipientLog>();

    public HippocampusContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<MacAddress>();
        modelBuilder.Ignore<LevelPercentage>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HippocampusContext).Assembly);
    }
}