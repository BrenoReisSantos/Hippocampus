using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Domain.Operations;

public class PumpManager(WaterTank _waterTank)
{
    public WaterTank TurnPumpOn()
    {
        if (IsBypassingPumpRules())
            return _waterTank with { PumpingWater = true };
        if (CanPump())
            return _waterTank with { PumpingWater = true };
        return _waterTank with { PumpingWater = false };
    }

    public WaterTank TurnPumpOff() => _waterTank with { PumpingWater = false };

    private bool CanPump()
    {
        if (_waterTank.PumpsTo is null)
            return false;
        return _waterTank.CurrentLevel > _waterTank.LevelWhenEmpty
            && _waterTank.PumpsTo.CurrentLevel < _waterTank.PumpsTo.LevelWhenFull;
    }

    private bool IsBypassingPumpRules() =>
        _waterTank.BypassMode is not null && _waterTank.BypassMode.Value;
}
