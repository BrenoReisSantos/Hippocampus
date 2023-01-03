using Hippocampus.Models.Values;
using StronglyTypedIds;

namespace Hippocampus.Models;

[StronglyTypedId]
public partial struct RecipientLogId { }

public enum State
{
    Empty,
    Average,
    Full
}

public class RecipientLog
{
    public RecipientLogId Id { get; init; } = RecipientLogId.New();

    public MacAddress MacAddress { get; init; } = new();

    public int Level { get; init; }

    public State State { get; init; }

    public DateTime RegisterDate { get; init; }
}
