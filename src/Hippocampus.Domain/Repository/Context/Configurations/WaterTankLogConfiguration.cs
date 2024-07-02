using Hippocampus.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hippocampus.Domain.Repository.Context.Configurations;

public class WaterTankLogConfiguration : IEntityTypeConfiguration<WaterTankLog>
{
    public void Configure(EntityTypeBuilder<WaterTankLog> builder)
    {
        builder.Property(wlog => wlog.WaterTankLogId).ValueGeneratedOnAdd();
        builder.HasAlternateKey(wlog => new { wlog.WaterTankId, wlog.LogDate });
        builder.HasIndex(wlog => wlog.LogDate).IsDescending();
        builder.HasOne<WaterTank>(wlog => wlog.WaterTank);
    }
}
