using Hippocampus.Models.Entities;
using Hippocampus.Models.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hippocampus.Models.Context.Configurations;

public class RecipientLogConfiguration : IEntityTypeConfiguration<RecipientLog>
{
    static readonly ValueConverter<MacAddress, string> recipientLogMacAddressConverter = new(
        macAddress => macAddress.ToString(MacAddress.Mask.Colon),
        stringMacAddress => new MacAddress(stringMacAddress));

    public void Configure(EntityTypeBuilder<RecipientLog> builder)
    {
        builder.Property(rlog => rlog.RecipientLogId).HasConversion<RecipientLogId.EfCoreValueConverter>();
        builder.Property(rlog => rlog.State).HasConversion<string>();
        builder.Property(rlog => rlog.MacAddress).HasConversion(recipientLogMacAddressConverter);
    }
}