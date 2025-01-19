namespace Hippocampus.Domain.Diplomat.HttpIn;

public record WaterTankLevelUpdateDto
{
    public int WaterLevel { get; init; }
};
