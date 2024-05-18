using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Domain.Repository.Context;

public class HippocampusContext : DbContext
{
    public DbSet<WaterTankLog> WaterTankLog => Set<WaterTankLog>();
    public DbSet<WaterTank> WaterTank => Set<WaterTank>();

    public HippocampusContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HippocampusContext).Assembly);
    }
}