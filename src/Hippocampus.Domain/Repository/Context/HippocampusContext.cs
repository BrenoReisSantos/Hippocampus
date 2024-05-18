using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Domain.Repository.Context;

public class HippocampusContext : DbContext
{
    public DbSet<WaterTankLog> RecipientLogs => Set<WaterTankLog>();
    public DbSet<WaterTank> RecipientMonitors => Set<WaterTank>();

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