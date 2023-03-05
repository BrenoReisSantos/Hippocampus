using Hippocampus.Models.Values;
using StronglyTypedIds;

namespace Hippocampus.Models.Entities;

public class RecipientLog
{
    public int RecipientLogId { get; }
    public LevelPercentage LevelPercentage { get; private set; } = new();
    public State State { get; private set; } = State.Average;

    public DateTime RegisterDate { get; private set; }
    public RecipientMonitorId RecipientMonitorId { get; private set; } = RecipientMonitorId.Empty;

    public RecipientLog()
    {
    }

    public RecipientLog(int recipientLogId, LevelPercentage levelPercentage, State state,
        DateTime registerDate, RecipientMonitorId recipientMonitorId)
    {
        RecipientLogId = recipientLogId;
        LevelPercentage = levelPercentage;
        State = state;
        RegisterDate = registerDate;
        RecipientMonitorId = recipientMonitorId;
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