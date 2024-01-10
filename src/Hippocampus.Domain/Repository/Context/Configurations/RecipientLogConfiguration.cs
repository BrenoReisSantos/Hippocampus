using Hippocampus.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hippocampus.Domain.Repository.Context.Configurations;

public class RecipientLogConfiguration : IEntityTypeConfiguration<RecipientLog>
{
    public void Configure(EntityTypeBuilder<RecipientLog> builder)
    {
        // builder.HasOne(r => r.Recipient).WithMany(r => r.RecipientLogs);
        builder.Property(rlog => rlog.RecipientLogId).ValueGeneratedOnAdd();
        builder.Property(rlog => rlog.RecipientState).HasConversion<string>();
        builder.OwnsOne(recipientLog => recipientLog.LevelPercentage,
            navigationBuilder =>
            {
                navigationBuilder.Property(level => level.Value).HasColumnName("LevelPercentage");
            });
        builder.HasAlternateKey(rlog => new { RecipientId = rlog.RecipientMonitorId, rlog.RegisterDate });
        builder.HasIndex(rlog => rlog.RegisterDate).IsDescending();
    }
}