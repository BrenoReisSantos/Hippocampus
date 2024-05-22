using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Repository;
using Hippocampus.Domain.Services.ApplicationValues;

namespace Hippocampus.Domain.Services;

public interface IWaterTankLevelUpdateProcessor
{
    Task<ServiceResult> Update(WaterTankId waterTankId, int level);
}

public class WaterTankLevelUpdateProcessor(
    WaterTankRepository _waterTankRepository,
    WaterTankLogService _waterTankLogService)
{
    public async Task<ServiceResult> Update(WaterTankId waterTankId, int level)
    {
        var waterTank = await _waterTankRepository.Get(waterTankId);
        if (waterTank is null) return ServiceResult.Success();

        waterTank = waterTank with
        {
            CurrentLevel = level,
        };

        if (IsBypassingPumpRules(waterTank))
        {
            return ServiceResult.Success();
        }

        if (MustPump(waterTank))
            waterTank = waterTank with
            {
                PumpingWater = true,
            };

        if (CantPump(waterTank))
            waterTank = waterTank with
            {
                PumpingWater = false,
            };

        await _waterTankRepository.Update(waterTank);
        await _waterTankLogService.Log(waterTank);
        return ServiceResult.Success();
    }

    private bool CantPump(WaterTank waterTank)
    {
        if (waterTank.PumpsTo is null) return false;

        return waterTank.PumpsTo.CurrentLevel >= waterTank.PumpsTo.LevelWhenFull ||
               waterTank.CurrentLevel <= waterTank.LevelWhenEmpty;
    }

    private static bool IsBypassingPumpRules(WaterTank waterTank)
    {
        return waterTank.BypassMode is not null && waterTank.BypassMode.Value;
    }

    private bool MustPump(WaterTank waterTank)
    {
        if (waterTank.PumpsTo is null) return false;

        return waterTank.PumpsTo.CurrentLevel <= waterTank.PumpsTo.LevelWhenEmpty &&
               waterTank.CurrentLevel > waterTank.LevelWhenEmpty;
    }
}