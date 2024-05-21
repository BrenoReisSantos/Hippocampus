using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Diplomat.HttpOut;

public class WaterTankUpdatedDto
{
    public WaterTankId WaterTankId { get; init; } = WaterTankId.Empty;
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; init; } = string.Empty;
    public int LevelWhenFull { get; init; }
    public int LevelWhenEmpty { get; init; }
    public RecipientMonitorLinkedToUpdatedDto? WaterTankLinkedTo { get; init; }
}

public class RecipientMonitorLinkedToUpdatedDto
{
    public WaterTankId WaterTankId { get; init; } = WaterTankId.Empty;
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; init; } = string.Empty;
    public int MaxHeight { get; init; }
    public int MinHeight { get; init; }
}