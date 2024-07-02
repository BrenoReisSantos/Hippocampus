using AutoBogus;
using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Tests.Common.Builders;

public sealed class RecipientMonitorPostDtoBuilder : AutoFaker<WaterTankCreateDto>
{
    public RecipientMonitorPostDtoBuilder()
    {
        RuleFor(r => r.Name, faker => faker.Random.Words(5));
        RuleFor(r => r.LevelWhenEmpty, faker => faker.Random.Int(0, 50));
        RuleFor(r => r.LevelWhenFull, faker => faker.Random.Int(51, 100));
        Ignore(r => r.PumpsToId);
    }

    public RecipientMonitorPostDtoBuilder WithInvalidMaxAndMinHeight()
    {
        RuleFor(r => r.LevelWhenFull, faker => faker.Random.Int(0, 50));
        RuleFor(r => r.LevelWhenEmpty, faker => faker.Random.Int(51, 100));
        return this;
    }
}
