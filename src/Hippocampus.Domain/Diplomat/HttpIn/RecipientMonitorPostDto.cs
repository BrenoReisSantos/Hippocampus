using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Diplomat.HttpIn;

public class RecipientMonitorPostDto
{
    public string Name { get; init; } = string.Empty;
    public MacAddress MacAddress { get; init; } = new();
    public float MinHeight { get; init; }
    public float MaxHeight { get; init; }
    public RecipientType RecipientType { get; init; }
    public MacAddress RecipientMonitorLinkedToMacAddress { get; init; } = new();
}