using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Repository;
using Hippocampus.Domain.Services;
using Hippocampus.Domain.Services.ApplicationValues;
using Hippocampus.Tests.Common;
using Hippocampus.Tests.Common.Builders;
using NSubstitute.ReturnsExtensions;

namespace Hippocampus.Test.Unit.Specs.Services;

public class WaterTankStateUpdateProcessorTests : BaseTest
{
    private readonly WaterTankStateUpdateProcessor _sut;
    private readonly IWaterTankRepository _waterTankRepositoryFake;
    private readonly IWaterTankLogService _waterTankLogServiceFake;

    public WaterTankStateUpdateProcessorTests()
    {
        _waterTankRepositoryFake = Substitute.For<IWaterTankRepository>();
        _waterTankLogServiceFake = Substitute.For<IWaterTankLogService>();
        _sut = new WaterTankStateUpdateProcessor(_waterTankRepositoryFake, _waterTankLogServiceFake);
    }

    [Fact]
    public async Task UpdateWaterTankStateForWaterLevel_WhenWaterTankExists_ShouldReturnServiceResultSuccess()
    {
        var levelWhenEmpty = 25;
        var levelWhenFull = 75;
        var waterTank = new WaterTankBuilder().WithLevelWhenEmpty(levelWhenEmpty).WithLevelWhenFull(levelWhenFull)
            .WithBypassMode(false)
            .Generate();

        _waterTankRepositoryFake.Get(Arg.Is(waterTank.WaterTankId)).Returns(waterTank);

        var waterTankLevelUpdateDtoInput = new WaterTankLevelUpdateDto { WaterLevel = 200 };
        var current = await _sut.UpdateWaterTankStateForWaterLevel(waterTank.WaterTankId, waterTankLevelUpdateDtoInput);

        var expected = ServiceResult.Success();
        current.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpdateWaterTankStateForWaterLevel_WhenWaterTankExists_ShouldCallRepositoryToUpdateWaterTank()
    {
        var levelWhenEmpty = 25;
        var levelWhenFull = 75;
        var waterTank = new WaterTankBuilder().WithLevelWhenEmpty(levelWhenEmpty).WithLevelWhenFull(levelWhenFull)
            .WithBypassMode(false)
            .Generate();

        _waterTankRepositoryFake.Get(Arg.Is(waterTank.WaterTankId)).Returns(waterTank);

        var waterTankLevelUpdateDtoInput = new WaterTankLevelUpdateDto { WaterLevel = 200 };
        await _sut.UpdateWaterTankStateForWaterLevel(waterTank.WaterTankId, waterTankLevelUpdateDtoInput);

        var expected = waterTank with { CurrentLevel = waterTankLevelUpdateDtoInput.WaterLevel };
        await _waterTankRepositoryFake.Received(1).Update(Arg.Is(expected));
    }

    [Fact]
    public async Task
        UpdateWaterTankStateForWaterLevel_WhenWaterTankIsBypassingPumpRules_ShouldReturnServiceResultSuccess()
    {
        var levelWhenEmpty = 25;
        var levelWhenFull = 75;
        var waterTank = new WaterTankBuilder().WithLevelWhenEmpty(levelWhenEmpty).WithLevelWhenFull(levelWhenFull)
            .WithBypassMode(true)
            .Generate();

        _waterTankRepositoryFake.Get(Arg.Is(waterTank.WaterTankId)).Returns(waterTank);

        var waterTankLevelUpdateDtoInput = new WaterTankLevelUpdateDto { WaterLevel = 200 };
        var current = await _sut.UpdateWaterTankStateForWaterLevel(waterTank.WaterTankId, waterTankLevelUpdateDtoInput);

        var expected = ServiceResult.Success();
        current.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task
        UpdateWaterTankStateForWaterLevel_WhenWaterTankIsBypassingPumpRules_ShouldCallRepositoryToUpdateWaterTank()
    {
        var levelWhenEmpty = 25;
        var levelWhenFull = 75;
        var waterTank = new WaterTankBuilder().WithLevelWhenEmpty(levelWhenEmpty).WithLevelWhenFull(levelWhenFull)
            .WithBypassMode(true)
            .Generate();

        _waterTankRepositoryFake.Get(Arg.Is(waterTank.WaterTankId)).Returns(waterTank);

        var waterTankLevelUpdateDtoInput = new WaterTankLevelUpdateDto { WaterLevel = 200 };
        await _sut.UpdateWaterTankStateForWaterLevel(waterTank.WaterTankId, waterTankLevelUpdateDtoInput);

        var expected = waterTank with { CurrentLevel = waterTankLevelUpdateDtoInput.WaterLevel };
        await _waterTankRepositoryFake.Received(1).Update(Arg.Is(expected));
    }

    [Fact]
    public async Task UpdateWaterTankStateForWaterLevel_WhenWaterTankNotExists_ShouldReturnServiceResultError()
    {
        _waterTankRepositoryFake.Get(Arg.Any<WaterTankId>()).ReturnsNull();

        WaterTankId.TryParse(Faker.Random.Guid().ToString(), out var anyWaterTankId);
        var waterTankLevelUpdateDtoInput = new WaterTankLevelUpdateDto { WaterLevel = 200 };
        var current = await _sut.UpdateWaterTankStateForWaterLevel(anyWaterTankId, waterTankLevelUpdateDtoInput);

        var expected = ServiceResult.Error($"Reservatório de água não encontrado para o Id {anyWaterTankId}");
        current.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task
        UpdateWaterTankStateForWaterLevel_WhenPumpedWaterTankIsEmptyAndSelfWaterLevelDoesNotBecomeEmpty_ShouldTurnPumpOn()
    {
        var levelWhenEmpty = 25;
        var levelWhenFull = 75;
        var pumpedToWaterTank = new WaterTankBuilder().WithLevelWhenEmpty(levelWhenEmpty)
            .WithLevelWhenFull(levelWhenFull)
            .WithBypassMode(false)
            .WithLevel(0)
            .Generate();
        var waterTank = new WaterTankBuilder().WithLevelWhenEmpty(levelWhenEmpty).WithLevelWhenFull(levelWhenFull)
            .WithBypassMode(false)
            .WithPumpsTo(pumpedToWaterTank)
            .Generate();

        _waterTankRepositoryFake.Get(Arg.Is(waterTank.WaterTankId)).Returns(waterTank);

        var waterTankLevelUpdateDtoInput = new WaterTankLevelUpdateDto { WaterLevel = 200 };
        await _sut.UpdateWaterTankStateForWaterLevel(waterTank.WaterTankId, waterTankLevelUpdateDtoInput);

        var expected = waterTank with { CurrentLevel = waterTankLevelUpdateDtoInput.WaterLevel, PumpingWater = true };
        await _waterTankRepositoryFake.Received(1).Update(Arg.Is(expected));
    }

    [Fact]
    public async Task
        UpdateWaterTankStateForWaterLevel_WhenPumpedWaterTankIsEmptyAndSelfWaterLevelBecomesEmpty_ShouldNotTurnPumpOn()
    {
        var levelWhenEmpty = 25;
        var levelWhenFull = 75;
        var pumpedToWaterTank = new WaterTankBuilder().WithLevelWhenEmpty(levelWhenEmpty)
            .WithLevelWhenFull(levelWhenFull)
            .WithBypassMode(false)
            .WithLevel(0)
            .Generate();
        var waterTank = new WaterTankBuilder().WithLevelWhenEmpty(levelWhenEmpty).WithLevelWhenFull(levelWhenFull)
            .WithBypassMode(false)
            .WithPumpsTo(pumpedToWaterTank)
            .WithPumpingWater(false)
            .WithLevel(0)
            .Generate();

        _waterTankRepositoryFake.Get(Arg.Is(waterTank.WaterTankId)).Returns(waterTank);

        var waterTankLevelUpdateDtoInput = new WaterTankLevelUpdateDto { WaterLevel = 15 };
        await _sut.UpdateWaterTankStateForWaterLevel(waterTank.WaterTankId, waterTankLevelUpdateDtoInput);

        var expected = waterTank with { CurrentLevel = waterTankLevelUpdateDtoInput.WaterLevel, PumpingWater = false };
        await _waterTankRepositoryFake.Received(1).Update(Arg.Is(expected));
    }

    [Fact]
    public async Task
        UpdateWaterTankStateForWaterLevel_WhenPumpedWaterTankIsFullAndSelfWaterLevelDoesBecomeAboveEmpty_ShouldTurnPumpOff()
    {
        var levelWhenEmpty = 25;
        var levelWhenFull = 75;
        var pumpedToWaterTank = new WaterTankBuilder().WithLevelWhenEmpty(levelWhenEmpty)
            .WithLevelWhenFull(levelWhenFull)
            .WithBypassMode(false)
            .WithLevel(80)
            .Generate();
        var waterTank = new WaterTankBuilder().WithLevelWhenEmpty(levelWhenEmpty).WithLevelWhenFull(levelWhenFull)
            .WithBypassMode(false)
            .WithPumpsTo(pumpedToWaterTank)
            .WithPumpingWater(true)
            .WithLevel(50)
            .Generate();

        _waterTankRepositoryFake.Get(Arg.Is(waterTank.WaterTankId)).Returns(waterTank);

        var waterTankLevelUpdateDtoInput = new WaterTankLevelUpdateDto { WaterLevel = 200 };
        await _sut.UpdateWaterTankStateForWaterLevel(waterTank.WaterTankId, waterTankLevelUpdateDtoInput);

        var expected = waterTank with { CurrentLevel = waterTankLevelUpdateDtoInput.WaterLevel, PumpingWater = false };
        await _waterTankRepositoryFake.Received(1).Update(Arg.Is(expected));
    }
}
