using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Operations;
using Hippocampus.Domain.Repository;
using Hippocampus.Domain.Services.ApplicationValues;

namespace Hippocampus.Domain.Services;

public interface IPumpControlService
{
    Task<ServiceResult> ControlPump(WaterTankId waterTankId, PumpPower pumpPower);
}

public class PumpControlService(
    IWaterTankRepository _waterTankRepository,
    IWaterTankLogService _waterTankLogService
) : IPumpControlService
{
    public async Task<ServiceResult> ControlPump(WaterTankId waterTankId, PumpPower pumpPower)
    {
        var waterTank = await _waterTankRepository.Get(waterTankId);
        if (waterTank is null)
            return ServiceResult.Error("Não foi possível encontrar o reservatório");

        if (pumpPower == PumpPower.On)
            waterTank = new PumpManager(waterTank).TurnPumpOn();
        else
            waterTank = new PumpManager(waterTank).TurnPumpOff();

        await _waterTankRepository.Update(waterTank);
        await _waterTankLogService.Log(waterTank);

        return ServiceResult.Success();
    }
}

public enum PumpPower
{
    Off,
    On,
}
