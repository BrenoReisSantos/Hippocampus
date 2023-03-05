using Hippocampus.Models.Entities;
using Hippocampus.Models.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hippocampus.Models.Context.Configurations;

public class RecipientConfiguration : IEntityTypeConfiguration<Recipient>
{
    private static readonly ValueConverter<MacAddress, string> RecipientLogMacAddressConverter = new(
        macAddress => macAddress.ToString(MacAddress.Mask.Colon),
        stringMacAddress => new MacAddress(stringMacAddress));

    public void Configure(EntityTypeBuilder<Recipient> builder)
    {
        // builder.HasMany(r => r.RecipientLogs).WithOne(rlog => rlog.Recipient);
        builder.OwnsOne<RecipientLevelLimit>(r => r.RecipientLevelLimit);
        builder.Property(r => r.RecipientId).HasConversion<RecipientId.EfCoreValueConverter>();
        builder.Property(r => r.MacAddress).HasConversion(RecipientLogMacAddressConverter);
        builder.Property(r => r.Name).HasMaxLength(100);
        builder.HasIndex(r => r.MacAddress).IsUnique();
    }
}