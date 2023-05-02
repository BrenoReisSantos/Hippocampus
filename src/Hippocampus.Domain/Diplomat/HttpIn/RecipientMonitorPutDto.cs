using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Diplomat.HttpIn;

public class RecipientMonitorPutDto
{
    public RecipientMonitorId RecipientMonitorId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int MinHeight { get; init; }
    public int MaxHeight { get; init; }
    public RecipientType RecipientType { get; init; }
    public MacAddress? RecipientMonitorLinkedToMacAddress { get; init; } = new();
}