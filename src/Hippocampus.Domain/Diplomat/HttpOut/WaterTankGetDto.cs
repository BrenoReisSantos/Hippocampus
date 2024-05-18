using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Diplomat.HttpOut;

public class WaterTankGetDto
{
    public WaterTankId WaterTankId { get; init; } = WaterTankId.Empty;
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; init; } = string.Empty;
    public WaterTankType WaterTankType { get; init; }
    public int MaxHeight { get; init; }
    public int MinHeight { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; init; }
}