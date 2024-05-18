﻿using AutoBogus;
using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Tests.Common.Builders;

public sealed class WaterTankBuilder : AutoFaker<WaterTank>
{
    public WaterTankBuilder()
    {
        RuleFor(r => r.WaterTankId, faker => new WaterTankId(faker.Random.Guid()));
        RuleFor(r => r.Name, faker => faker.Random.Words(5));
        RuleFor(r => r.Type, faker => faker.PickRandom<WaterTankType>());
        RuleFor(r => r.CreatedAt, faker => faker.Date.Past().ToUniversalTime());
        RuleFor(r => r.UpdatedAt, faker => faker.Date.Recent());
        RuleFor(r => r.LevelWhenFull, faker => faker.Random.Int(51, 100));
        RuleFor(r => r.LevelWhenEmpty, faker => faker.Random.Int(0, 50));
        RuleFor(r => r.IsActive, true);
        Ignore(r => r.WaterTankLogs);
        Ignore(r => r.PumpsTo);
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

    public WaterTankBuilder WithWaterTankType(WaterTankType waterTankType)
    {
        RuleFor(r => r.Type, waterTankType);
        return this;
    }

    public WaterTankBuilder WithLogs(IEnumerable<WaterTankLog> logs)
    {
        RuleFor(monitor => monitor.WaterTankLogs, logs);
        return this;
    }
}