using AutoBogus;
using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Tests.Common.Builders;

public sealed class RecipientMonitorPostDtoBuilder : AutoFaker<RecipientMonitorPostDto>
{
    public RecipientMonitorPostDtoBuilder()
    {
        RuleFor(r => r.Name, faker => faker.Random.Words(5));
        RuleFor(r => r.MacAddress, faker => new(faker.Internet.Mac()));
        RuleFor(r => r.MinHeight, faker => faker.Random.Float(0, 50));
        RuleFor(r => r.MaxHeight, faker => faker.Random.Float(51, 100));
        RuleFor(r => r.WifiSsid, faker => faker.Random.Word());
        RuleFor(r => r.WifiPassword, faker => faker.Random.AlphaNumeric(10));
        RuleFor(r => r.RecipientType, faker => faker.PickRandom<RecipientType>());
    }

    public RecipientMonitorPostDtoBuilder WithInvalidMaxAndMinHeight()
    {
        RuleFor(r => r.MaxHeight, faker => faker.Random.Float(0, 50));
        RuleFor(r => r.MinHeight, faker => faker.Random.Float(51, 100));
        return this;
    }
}