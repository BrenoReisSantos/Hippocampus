using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Domain.Models.Dto;

public record WaterTankGetDto()
{
    public WaterTankId WaterTankId { get; init; } = WaterTankId.New();
    public string Name { get; set; } = string.Empty;
    public int CurrentLevel { get; init; }
    public int LevelWhenEmpty { get; init; }
    public int LevelWhenFull { get; init; }
    public bool? PumpingWater { get; init; }
    public bool IsActive { get; set; }
    public bool? BypassMode { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; init; }
    public SimplifiedWaterTankGetDto? PumpsTo { get; init; }
    public SimplifiedWaterTankGetDto? PumpedFrom { get; init; }
}

public record SimplifiedWaterTankGetDto()
{
    public WaterTankId WaterTankId { get; init; } = WaterTankId.New();
    public string Name { get; set; } = string.Empty;
}
