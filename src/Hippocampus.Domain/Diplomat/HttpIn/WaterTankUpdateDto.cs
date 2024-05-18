using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Diplomat.HttpIn;

public class WaterTankUpdateDto
{
    public WaterTankId WaterTankId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int LevelWhenEmpty { get; init; }
    public int LevelWhenFull { get; init; }
    public WaterTankType WaterTankType { get; init; }
    public WaterTankId? PumpsToId { get; init; }
}