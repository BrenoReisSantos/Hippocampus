using AutoBogus;
using Hippocampus.Domain.Diplomat.HttpIn;

namespace Hippocampus.Tests.Common.Builders;

public sealed class RecipientMonitorPostDtoBuilder : AutoFaker<RecipientMonitorPostDto>
{
    public RecipientMonitorPostDtoBuilder()
    {
        RuleFor(r => r.Name, faker => faker.Random.Words(5));
        RuleFor(r => r.MacAddress, faker => new(faker.Internet.Mac()));
        RuleFor(r => r.MinHeight, faker => faker.Random.Float(0, 50));
        RuleFor(r => r.MaxHeight, faker => faker.Random.Float(51, 100));
    }
}