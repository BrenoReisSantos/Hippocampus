using Hippocampus.Models.Values;

namespace Hippocampus.Models;

public record RecipientLogId(Guid Value)
{
    public static RecipientLogId New() => new(Guid.NewGuid());
}

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
