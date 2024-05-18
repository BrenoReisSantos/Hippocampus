using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Domain.Operations;

public class RecipientMonitorLevelUpdateCalculator(WaterTank waterTank)
{
    public WaterTank UpdateLevel(int newLevel)
    {
        return waterTank with
        {
            CurrentLevelHeight = newLevel,
            WaterTankState = CalculateRecipientState(newLevel)
        };
    }

    private WaterTankState CalculateRecipientState(int newLevel)
    {
        if (newLevel >= waterTank.LevelWhenFull) return WaterTankState.Full;
        if (newLevel <= waterTank.LevelWhenEmpty) return WaterTankState.Empty;
        return WaterTankState.Average;
    }
}