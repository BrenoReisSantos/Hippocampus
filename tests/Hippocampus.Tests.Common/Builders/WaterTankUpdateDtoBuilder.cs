using AutoBogus;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Tests.Common.Builders;

public sealed class WaterTankUpdateDtoBuilder : AutoFaker<WaterTankUpdatedDto>
{
    public WaterTankUpdateDtoBuilder()
    {
        RuleFor(r => r.WaterTankId, WaterTankId.New());
        RuleFor(r => r.Name, faker => faker.Random.Word());
        RuleFor(r => r.LevelWhenEmpty, faker => faker.Random.Int(0, 50));
        RuleFor(r => r.LevelWhenFull, faker => faker.Random.Int(51, 100));
        RuleFor(r => r.WaterTankLinkedTo, faker => null);
    }

    public WaterTankUpdateDtoBuilder WithWaterTankId(WaterTankId waterTankId)
    {
        RuleFor(r => r.WaterTankId, waterTankId);
        return this;
    }

    public WaterTankUpdateDtoBuilder WithInvalidFullAndEmptyValue()
    {
        RuleFor(r => r.LevelWhenFull, faker => faker.Random.Int(0, 50));
        RuleFor(r => r.LevelWhenEmpty, faker => faker.Random.Int(51, 100));
        return this;
    }
}