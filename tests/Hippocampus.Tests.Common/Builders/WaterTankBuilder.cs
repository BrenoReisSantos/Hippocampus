using System.Data;
using AutoBogus;
using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Tests.Common.Builders;

public sealed class WaterTankBuilder : AutoFaker<WaterTank>
{
    public WaterTankBuilder()
    {
        RuleFor(r => r.WaterTankId, faker => new WaterTankId(faker.Random.Guid()));
        RuleFor(r => r.Name, faker => faker.Random.Word());
        RuleFor(r => r.CreatedAt, faker => faker.Date.Past().ToUniversalTime());
        RuleFor(r => r.UpdatedAt, faker => faker.Date.Recent().ToUniversalTime());
        RuleFor(w => w.PumpingWater, faker => faker.Random.Bool());
        RuleFor(w => w.BypassMode, faker => faker.Random.Bool());
        RuleFor(w => w.CurrentLevel, faker => faker.Random.Int(0, 100));
        RuleFor(r => r.LevelWhenFull, faker => faker.Random.Int(51, 100));
        RuleFor(r => r.LevelWhenEmpty, faker => faker.Random.Int(0, 50));
        RuleFor(r => r.IsActive, true);
        Ignore(r => r.WaterTankLogs);
        Ignore(r => r.PumpsTo);
        Ignore(w => w.PumpedFrom);
    }

    public WaterTankBuilder WithLinkedWaterTank(WaterTank linkedWaterTank)
    {
        RuleFor(r => r.PumpsTo, linkedWaterTank);
        return this;
    }

    public WaterTankBuilder WithId(WaterTankId waterTankId)
    {
        RuleFor(r => r.WaterTankId, waterTankId);
        return this;
    }

    public WaterTankBuilder WithLogs(IEnumerable<WaterTankLog> logs)
    {
        RuleFor(monitor => monitor.WaterTankLogs, logs);
        return this;
    }
}