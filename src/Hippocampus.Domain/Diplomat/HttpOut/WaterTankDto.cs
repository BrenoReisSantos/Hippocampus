using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Diplomat.HttpOut;

public class WaterTankDto
{
    public WaterTankId WaterTankId { get; init; } = WaterTankId.New();
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; set; } = string.Empty;
    public int MinHeight { get; set; }
    public int MaxHeight { get; set; }
    public WaterTankId? WaterTankLinkedToId { get; set; }
}