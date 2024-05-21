using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Diplomat.HttpOut;

public class WaterTankForTableDto
{
    public WaterTankId WaterTankId { get; init; } = WaterTankId.Empty;
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; init; } = string.Empty;
    public int? RecipientLevelPercentage { get; init; }
    public int MaxHeight { get; init; }
    public int MinHeight { get; init; }
    public MacAddress? LinkedRecipientMonitorMacAddress { get; init; }
}