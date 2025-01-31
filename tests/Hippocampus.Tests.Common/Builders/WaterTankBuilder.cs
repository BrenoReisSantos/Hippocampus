﻿using System.Data;
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
        RuleFor(r => r.BypassMode, f => f.Random.Bool());
        RuleFor(r => r.IsActive, true);
        Ignore(r => r.WaterTankLogs);
        Ignore(r => r.PumpsTo);
        Ignore(w => w.PumpedFrom);
    }

    public WaterTankBuilder WithPumpsTo(WaterTank waterTank)
    {
        RuleFor(r => r.PumpsTo, waterTank);
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

    public WaterTankBuilder WithLevel(int level)
    {
        RuleFor(x => x.CurrentLevel, level);
        return this;
    }

    public WaterTankBuilder WithAverageLevel()
    {
        RuleFor(w => w.LevelWhenFull, 100);
        RuleFor(w => w.LevelWhenEmpty, 0);
        RuleFor(w => w.CurrentLevel, faker => faker.Random.Int(1, 99));
        return this;
    }

    public WaterTankBuilder WithEmptyLevel()
    {
        RuleFor(w => w.LevelWhenEmpty, 50);
        RuleFor(w => w.CurrentLevel, faker => faker.Random.Int(0, 49));
        return this;
    }

    public WaterTankBuilder WithLevelWhenEmpty(int level)
    {
        RuleFor(w => w.LevelWhenEmpty, level);
        return this;
    }

    public WaterTankBuilder WithFullLevel()
    {
        RuleFor(w => w.LevelWhenFull, 100);
        RuleFor(w => w.CurrentLevel, faker => faker.Random.Int(101, 150));
        return this;
    }

    public WaterTankBuilder WithLevelWhenFull(int level)
    {
        RuleFor(w => w.LevelWhenFull, level);
        return this;
    }

    public WaterTankBuilder WithOtherWaterTankToPumpTo()
    {
        RuleFor(r => r.PumpsTo, new WaterTankBuilder().WithAverageLevel().Generate());
        return this;
    }

    public WaterTankBuilder WithBypassMode(bool bypassMode)
    {
        RuleFor(x => x.BypassMode, bypassMode);
        return this;
    }

    public WaterTankBuilder WithPumpingWater(bool pumpingWater)
    {
        RuleFor(x => x.PumpingWater, pumpingWater);
        return this;
    }
}
