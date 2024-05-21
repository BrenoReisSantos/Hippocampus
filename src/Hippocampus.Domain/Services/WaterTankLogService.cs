using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Repository;

namespace Hippocampus.Domain.Services;

public class WaterTankLogService(WaterTankLogRepository _waterTankLogRepository)
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