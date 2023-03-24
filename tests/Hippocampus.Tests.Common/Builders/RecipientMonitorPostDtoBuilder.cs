using AutoBogus;
using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Tests.Common.Builders;

public sealed class RecipientMonitorPostDtoBuilder : AutoFaker<RecipientMonitorPostDto>
{
    public RecipientMonitorPostDtoBuilder()
    {
        RuleFor(r => r.Name, faker => faker.Random.Words(5));
        RuleFor(r => r.MacAddress, faker => new(faker.Internet.Mac()));
        RuleFor(r => r.MinHeight, faker => faker.Random.Float(0, 50));
        RuleFor(r => r.MaxHeight, faker => faker.Random.Float(51, 100));
        RuleFor(r => r.RecipientType, faker => faker.PickRandom<RecipientType>());
        RuleFor(r => r.RecipientMonitorLinkedToMacAddress, faker => null);
    }

    public RecipientMonitorPostDtoBuilder WithInvalidMaxAndMinHeight()
    {
        RuleFor(r => r.MaxHeight, faker => faker.Random.Float(0, 50));
        RuleFor(r => r.MinHeight, faker => faker.Random.Float(51, 100));
        return this;
    }

    public RecipientMonitorPostDtoBuilder WithRecipientMonitorLinkedToMacAddress(MacAddress monitorLinkedToMacAddress)
    {
        RuleFor(r => r.RecipientMonitorLinkedToMacAddress, monitorLinkedToMacAddress);
        return this;
    }

    public RecipientMonitorPostDtoBuilder WithRecipientType(RecipientType recipientType)
    {
        RuleFor(r => r.RecipientType, recipientType);
        return this;
    }
}