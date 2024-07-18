using Hippocampus.Domain.Repository;
using Hippocampus.Domain.Services;
using Hippocampus.Tests.Common;
using Hippocampus.Tests.Common.Builders;
using NSubstitute;
using NSubstitute.ClearExtensions;

namespace Hippocampus.Test.Unit.Specs.Services;

public class PumpControlServiceTests : BaseTest, IDisposable
{
    private readonly IWaterTankRepository _waterTankRepositoryFake;
    private readonly IWaterTankLogService _waterTankLogServiceFake;
    private readonly IPumpControlService _sut;

    public PumpControlServiceTests()
    {
        _waterTankRepositoryFake = Substitute.For<IWaterTankRepository>();
        _waterTankLogServiceFake = Substitute.For<IWaterTankLogService>();
        _sut = new PumpControlService(_waterTankRepositoryFake, _waterTankLogServiceFake);
    }

    public void Dispose()
    {
        _waterTankRepositoryFake.ClearSubstitute();
    }

    [Fact]
    public async Task ControlPumpTurningOn_AverageLevelWaterTank_ShouldUpdateWaterTankWithPumpingTrue()
    {
        var waterTank = new WaterTankBuilder().WithOtherWaterTankToPumpTo().WithAverageLevel().Generate();

        _waterTankRepositoryFake.Get(waterTank.WaterTankId).Returns(waterTank);

        await _sut.ControlPump(waterTank.WaterTankId, PumpPower.On);

        var expected = waterTank with { PumpingWater = true, };

        await _waterTankRepositoryFake.Received(1).Update(Arg.Is(expected));
    }

    [Fact]
    public async Task ControlPumpTurningOn_FullLevelWaterTank_ShouldUpdateWaterTankWithPumpingFalse()
    {
        var waterTank = new WaterTankBuilder().WithFullLevel().Generate();

        _waterTankRepositoryFake.Get(waterTank.WaterTankId).Returns(waterTank);

        await _sut.ControlPump(waterTank.WaterTankId, PumpPower.On);

        var expected = waterTank with { PumpingWater = true, };

        await _waterTankRepositoryFake.Received(1).Update(Arg.Is(expected));
    }

    [Fact]
    public async Task ControlPumpTurningOn_EmptyLevelWaterTank_ShouldUpdateWaterTankWithPumpingFalse()
    {
        var waterTank = new WaterTankBuilder().WithEmptyLevel().Generate();

        _waterTankRepositoryFake.Get(waterTank.WaterTankId).Returns(waterTank);

        await _sut.ControlPump(waterTank.WaterTankId, PumpPower.On);

        var expected = waterTank with { PumpingWater = false, };

        await _waterTankRepositoryFake.Received(1).Update(Arg.Is(expected));
    }
}
