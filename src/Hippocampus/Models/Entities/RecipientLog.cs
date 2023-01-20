using Hippocampus.Models.Values;
using StronglyTypedIds;

namespace Hippocampus.Models.Entities;

[StronglyTypedId]
public partial struct RecipientLogId
{
    public static bool TryParse(string value, out RecipientLogId id)
    {
        var couldConvert = Guid.TryParse(value, out var guid);
        id = new RecipientLogId(guid);
        return couldConvert;
    }
}

public enum State
{
    Empty,
    Average,
    Full
}

public class RecipientLog
{
    public RecipientLogId RecipientLogId { get; init; } = RecipientLogId.New();
    public required MacAddress MacAddress { get; init; }
    public required RecipientLevel Level { get; init; }
    public required State State { get; init; }
    public required DateTime RegisterDate { get; init; }
}