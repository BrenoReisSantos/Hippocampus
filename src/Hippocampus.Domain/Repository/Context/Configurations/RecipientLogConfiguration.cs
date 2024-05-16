using Hippocampus.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hippocampus.Domain.Repository.Context.Configurations;

public class RecipientLogConfiguration : IEntityTypeConfiguration<RecipientLog>
{
    public void Configure(EntityTypeBuilder<RecipientLog> builder)
    {
        builder.Property(rlog => rlog.RecipientLogId).ValueGeneratedOnAdd();
        builder.Property(rlog => rlog.RecipientState).HasConversion<string>();
        builder.HasAlternateKey(rlog => new { rlog.RecipientMonitorId, rlog.RegisterDate });
        builder.HasIndex(rlog => rlog.RegisterDate).IsDescending();
    }
}