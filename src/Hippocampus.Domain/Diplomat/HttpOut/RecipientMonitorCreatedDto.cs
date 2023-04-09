using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Diplomat.HttpOut;

public class RecipientMonitorCreatedDto
{
    public RecipientMonitorId RecipientMonitorId { get; init; } = RecipientMonitorId.Empty;
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; init; } = string.Empty;
    public RecipientType RecipientType { get; init; }
    public int MaxHeight { get; init; }
    public int MinHeight { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public RecipientMonitorLinkedToCreatedDto? RecipientMonitorLinkedTo { get; init; }
}

public class RecipientMonitorLinkedToCreatedDto
{
    public RecipientMonitorId RecipientMonitorId { get; init; } = RecipientMonitorId.Empty;
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; init; } = string.Empty;
    public RecipientType RecipientType { get; init; }
    public int MaxHeight { get; init; }
    public int MinHeight { get; init; }
}