using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Diplomat.HttpOut;

public class RecipientMonitorCreatedDto
{
    public RecipientMonitorId RecipientMonitorId { get; init; } = RecipientMonitorId.Empty;
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; init; } = string.Empty;
    public RecipientBoundary RecipientBoundary { get; init; } = new();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}