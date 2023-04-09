using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Domain.Repository.Context;

public class HippocampusContext : DbContext
{
    public DbSet<RecipientLog> RecipientLogs => Set<RecipientLog>();
    public DbSet<RecipientMonitor> RecipientMonitors => Set<RecipientMonitor>();

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