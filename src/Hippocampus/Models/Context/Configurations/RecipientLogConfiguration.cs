using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hippocampus.Models.Context.Configuration;

public class RecipientLogConfiguraton : IEntityTypeConfiguration<RecipientLog>
{
    public static readonly ValueConverter<RecipientLogId, Guid> RecipientLogIdConverter = new(
        recipientLogId => recipientLogId.Value,
        guid => new(guid)
    );

    static readonly ValueConverter<State, string> RecipientLogStateConverter = new(
        state => state.ToString(),
        stateString => Enum.Parse<State>(stateString)
    );

    public void Configure(EntityTypeBuilder<RecipientLog> builder)
    {
        builder.HasKey(rlog => rlog.Id);
        builder.Property(rlog => rlog.Id).HasConversion(RecipientLogIdConverter);

        builder.Property(rlog => rlog.State).HasConversion(RecipientLogStateConverter);
    }
}
