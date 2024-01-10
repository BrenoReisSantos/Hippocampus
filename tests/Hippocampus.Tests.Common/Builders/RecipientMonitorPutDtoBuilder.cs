using AutoBogus;
using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Tests.Common.Builders;

public sealed class RecipientMonitorPutDtoBuilder : AutoFaker<RecipientMonitorPutDto>
{
    public RecipientMonitorPutDtoBuilder()
    {
        RuleFor(r => r.RecipientMonitorId, RecipientMonitorId.New());
        RuleFor(r => r.Name, faker => faker.Random.Word());
        RuleFor(r => r.MinHeight, faker => faker.Random.Int(0, 50));
        RuleFor(r => r.MaxHeight, faker => faker.Random.Int(51, 100));
        RuleFor(r => r.RecipientType, faker => faker.PickRandom<RecipientType>());
        RuleFor(r => r.RecipientMonitorLinkedToMacAddress, faker => null);
    }

    public RecipientMonitorPutDtoBuilder WithRecipientMonitorId(RecipientMonitorId recipientMonitorId)
    {
        RuleFor(r => r.RecipientMonitorId, recipientMonitorId);
        return this;
    }

    public RecipientMonitorPutDtoBuilder WithRecipientMonitorLinkedToMacAddress(
        MacAddress monitorLinkedToMacAddress)
    {
        RuleFor(r => r.RecipientMonitorLinkedToMacAddress, monitorLinkedToMacAddress);
        return this;
    }

    public RecipientMonitorPutDtoBuilder WithRecipientType(RecipientType recipientType)
    {
        RuleFor(r => r.RecipientType, recipientType);
        return this;
    }

    public RecipientMonitorPutDtoBuilder WithInvalidMaxAndMinHeight()
    {
        RuleFor(r => r.MaxHeight, faker => faker.Random.Int(0, 50));
        RuleFor(r => r.MinHeight, faker => faker.Random.Int(51, 100));
        return this;
    }
}