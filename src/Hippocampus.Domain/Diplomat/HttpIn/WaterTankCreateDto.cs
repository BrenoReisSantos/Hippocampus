using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Diplomat.HttpIn;

public class WaterTankCreateDto
{
    public string Name { get; init; } = string.Empty;
    public int LevelWhenEmpty { get; init; }
    public int LevelWhenFull { get; init; }
    public WaterTankId? PumpsToId { get; init; }
}