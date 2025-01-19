using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Diplomat.HttpOut;

public class WaterTankCreatedDto
{
    public WaterTankId WaterTankId { get; init; } = WaterTankId.Empty;
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; init; } = string.Empty;
    public int MaxHeight { get; init; }
    public int MinHeight { get; init; }
    public DateTime CreatedAt { get; init; }
    public RecipientMonitorLinkedToCreatedDto? RecipientMonitorLinkedTo { get; init; }
}

public class RecipientMonitorLinkedToCreatedDto
{
    public WaterTankId WaterTankId { get; init; } = WaterTankId.Empty;
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; init; } = string.Empty;
    public int MaxHeight { get; init; }
    public int MinHeight { get; init; }
}
