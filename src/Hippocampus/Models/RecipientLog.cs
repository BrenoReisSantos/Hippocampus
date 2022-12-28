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

    public string MacAddress { get; init; } = string.Empty;

    public int Level { get; init; }

    public State State { get; init; }

    public DateTime RegisterDate { get; init; }
}
