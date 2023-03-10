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
        builder.Property(rlog => rlog.State).HasConversion<string>();
        builder.HasAlternateKey(rlog => new { RecipientId = rlog.RecipientMonitorId, rlog.RegisterDate });
    }
}