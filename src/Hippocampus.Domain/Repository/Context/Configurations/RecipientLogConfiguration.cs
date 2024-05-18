using Hippocampus.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hippocampus.Domain.Repository.Context.Configurations;

public class RecipientLogConfiguration : IEntityTypeConfiguration<WaterTankLog>
{
    public void Configure(EntityTypeBuilder<WaterTankLog> builder)
    {
        builder.Property(rlog => rlog.WaterTankLogId).ValueGeneratedOnAdd();
        builder.Property(rlog => rlog.WaterTankState).HasConversion<string>();
        builder.HasAlternateKey(rlog => new { RecipientMonitorId = rlog.WaterTankId, RegisterDate = rlog.LogDate });
        builder.HasIndex(rlog => rlog.LogDate).IsDescending();
    }
}