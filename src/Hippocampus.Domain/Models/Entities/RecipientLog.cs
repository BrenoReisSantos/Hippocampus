using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Models.Entities;

public class RecipientLog
{
    public int RecipientLogId { get; init; }
    public LevelPercentage LevelPercentage { get; init; } = new();
    public State State { get; init; } = State.Average;
    public DateTime RegisterDate { get; init; }
    public RecipientMonitorId RecipientMonitorId { get; init; } = RecipientMonitorId.Empty;
    public RecipientMonitor RecipientMonitor { get; init; } = new();
}

public enum State
{
    Empty,
    Average,
    Full
}