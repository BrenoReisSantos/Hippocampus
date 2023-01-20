using Hippocampus.Models.Entities;
using Hippocampus.Models.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hippocampus.Models.Context.Configurations;

public class RecipientLogConfiguration : IEntityTypeConfiguration<RecipientLog>
{
    public static readonly ValueConverter<Entities.RecipientLogId, Guid> recipientLogIdConverter = new(
        recipientLogId => recipientLogId.Value,
        guid => new(guid)
    );

    static readonly ValueConverter<State, string> recipientLogStateConverter = new(
        state => state.ToString(),
        stateString => Enum.Parse<State>(stateString)
    );

    static readonly ValueConverter<MacAddress, string> recipientLogMacAddressConverter = new(
        macAddress => macAddress.ToString(MacAddress.Mask.Colon),
        stringMacAddress => new MacAddress(stringMacAddress));

    public void Configure(EntityTypeBuilder<RecipientLog> builder)
    {
        builder.Property(rlog => rlog.RecipientLogId).HasConversion<Entities.RecipientLogId.EfCoreValueConverter>();
        builder.Property(rlog => rlog.State).HasConversion(recipientLogStateConverter);
        builder.Property(rlog => rlog.MacAddress).HasConversion(recipientLogMacAddressConverter);
        builder.HasIndex(rlog => rlog.MacAddress).IsUnique();
    }
}