using Hippocampus.Models.Values;
using StronglyTypedIds;

namespace Hippocampus.Models.Entities;

public class RecipientLog
{
    public int RecipientLogId { get; }
    public LevelPercentage LevelPercentage { get; private set; } = new();
    public State State { get; private set; } = State.Average;

    public DateTime RegisterDate { get; private set; }
    public RecipientId RecipientId { get; private set; } = RecipientId.Empty;

    public RecipientLog()
    {
    }

    public RecipientLog(int recipientLogId, LevelPercentage levelPercentage, State state,
        DateTime registerDate, RecipientId recipientId)
    {
        RecipientLogId = recipientLogId;
        LevelPercentage = levelPercentage;
        State = state;
        RegisterDate = registerDate;
        RecipientId = recipientId;
    }

    public RecipientLog(int recipientLogId, LevelPercentage levelPercentage, State state,
        DateTime registerDate)
    {
        RecipientLogId = recipientLogId;
        LevelPercentage = levelPercentage;
        State = state;
        RegisterDate = registerDate;
    }
}

public enum State
{
    Empty,
    Average,
    Full
}