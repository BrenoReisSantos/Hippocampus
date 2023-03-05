using Hippocampus.Models.Values;
using StronglyTypedIds;

namespace Hippocampus.Models.Entities;

[StronglyTypedId]
public partial struct RecipientId
{
    public static bool TryParse(string value, out RecipientId id)
    {
        var couldConvert = Guid.TryParse(value, out var guid);
        id = new RecipientId(guid);
        return couldConvert;
    }
}

public class Recipient
{
    public RecipientId RecipientId { get; init; } = RecipientId.New();
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; init; } = string.Empty;
    public RecipientLevelLimit RecipientLevelLimit { get; init; } = new();
    public bool IsActive { get; init; } = true;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; init; }
    public IEnumerable<RecipientLog> RecipientLogs { get; init; } = Enumerable.Empty<RecipientLog>();
}