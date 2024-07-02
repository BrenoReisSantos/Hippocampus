using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Repository;

namespace Hippocampus.Domain.Services;

public interface IWaterTankLogService
{
    Task Log(WaterTank waterTank);
}

public class WaterTankLogService(WaterTankLogRepository _waterTankLogRepository)
    : IWaterTankLogService
{
    public async Task Log(WaterTank waterTank)
    {
        var log = new WaterTankLog
        {
            WaterTankId = waterTank.WaterTankId,
            PumpingWater = waterTank.PumpingWater,
            Level = waterTank.CurrentLevel,
            BypassMode = waterTank.BypassMode,
        };

        await _waterTankLogRepository.Insert(log);
    }
}
