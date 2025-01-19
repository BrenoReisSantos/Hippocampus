using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Operations;
using Hippocampus.Domain.Repository;
using Hippocampus.Domain.Services.ApplicationValues;

namespace Hippocampus.Domain.Services;

public interface IWaterTankStateUpdateProcessor
{
    Task<ServiceResult> UpdateWaterTankStateForWaterLevel(WaterTankId waterTankId,
        WaterTankLevelUpdateDto waterTankLevelUpdateDto);
}

public class WaterTankStateUpdateProcessor(
    IWaterTankRepository waterTankRepository,
    IWaterTankLogService waterTankLogService
) : IWaterTankStateUpdateProcessor
{
    public async Task<ServiceResult> UpdateWaterTankStateForWaterLevel(WaterTankId waterTankId,
        WaterTankLevelUpdateDto waterTankLevelUpdateDto)
    {
        var waterTank = await waterTankRepository.Get(waterTankId);
        if (waterTank is null)
            return ServiceResult.Error($"Reservatório de água não encontrado para o Id {waterTankId}");

        waterTank = waterTank with { CurrentLevel = waterTankLevelUpdateDto.WaterLevel };

        waterTank = ControlWaterTankPumpingState(waterTank);

        await waterTankRepository.Update(waterTank);
        await waterTankLogService.Log(waterTank);
        return ServiceResult.Success();
    }

    private WaterTank ControlWaterTankPumpingState(WaterTank waterTank)
    {
        var pumpManager = new PumpManager(waterTank);
        if (MustPump(waterTank))
            waterTank = pumpManager.TurnPumpOn();
        if (CantPump(waterTank))
            waterTank = pumpManager.TurnPumpOff();
        return waterTank;
    }

    private bool CantPump(WaterTank waterTank)
    {
        if (waterTank.PumpsTo is null)
            return false;

        return waterTank.PumpsTo.CurrentLevel >= waterTank.PumpsTo.LevelWhenFull
               || waterTank.CurrentLevel <= waterTank.LevelWhenEmpty;
    }

    private bool MustPump(WaterTank waterTank)
    {
        if (waterTank.PumpsTo is null)
            return false;

        return waterTank.PumpsTo.CurrentLevel <= waterTank.PumpsTo.LevelWhenEmpty
               && waterTank.CurrentLevel > waterTank.LevelWhenEmpty;
    }

    private static bool IsBypassingPumpRules(WaterTank waterTank) =>
        waterTank.BypassMode is not null && waterTank.BypassMode.Value;
}
