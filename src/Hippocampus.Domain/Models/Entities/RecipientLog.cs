using System.Text.Json.Serialization;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Models.Entities;

public record RecipientLog
{
    public int RecipientLogId { get; init; }
    public int LevelHeight { get; init; } = new();
    public RecipientState RecipientState { get; init; } = RecipientState.Average;
    public DateTime RegisterDate { get; init; }
    public RecipientMonitorId RecipientMonitorId { get; init; } = RecipientMonitorId.Empty;
    public RecipientMonitor? RecipientMonitor { get; init; } = new();
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RecipientState
{
    Empty,
    Average,
    Full
}