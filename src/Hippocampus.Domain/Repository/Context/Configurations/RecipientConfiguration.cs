using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hippocampus.Domain.Repository.Context.Configurations;

public class RecipientConfiguration : IEntityTypeConfiguration<RecipientMonitor>
{
    private static readonly ValueConverter<MacAddress, string> RecipientLogMacAddressConverter = new(
        macAddress => macAddress.ToString(MacAddress.Mask.Colon),
        stringMacAddress => new MacAddress(stringMacAddress));

    public void Configure(EntityTypeBuilder<RecipientMonitor> builder)
    {
        // builder.HasMany(r => r.RecipientLogs).WithOne(rlog => rlog.Recipient);
        builder.OwnsOne<RecipientBoundary>(r => r.RecipientBoundary);
        builder.Property(r => r.RecipientMonitorId).HasConversion<RecipientMonitorId.EfCoreValueConverter>();
        builder.Property(r => r.MacAddress).HasConversion(RecipientLogMacAddressConverter);
        builder.Property(r => r.Name).HasMaxLength(100);
        builder.HasIndex(r => r.MacAddress).IsUnique();
    }
}