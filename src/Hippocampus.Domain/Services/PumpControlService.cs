using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Operations;
using Hippocampus.Domain.Repository;
using Hippocampus.Domain.Services.ApplicationValues;

namespace Hippocampus.Domain.Services;

public interface IPumpControlService
{
    Task<ServiceResult> ControlPump(WaterTankId waterTankId, bool setPumpPower);
}

public class PumpControlService(IWaterTankRepository _waterTankRepository, WaterTankLogService _waterTankLogService)
    : IPumpControlService
{
    public async Task<ServiceResult> ControlPump(WaterTankId waterTankId, bool setPumpPower)
    {
        var waterTank = await _waterTankRepository.Get(waterTankId);
        if (waterTank is null) return ServiceResult.Error("Não foi possível encontrar o reservatório");

        if (setPumpPower)
            waterTank = new PumpManager(waterTank).TurnPumpOn();
        else
            waterTank = new PumpManager(waterTank).TurnPumpOff();

        await _waterTankRepository.Update(waterTank);
        await _waterTankLogService.Log(waterTank);

        return ServiceResult.Success();
    }
}