using AutoBogus;
using Bogus;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Tests.Common.Builders;

public sealed class RecipientLogBuilder : AutoFaker<RecipientLog>
{
    public RecipientLogBuilder()
    {
        Ignore(recipientLog => recipientLog.RecipientLogId);
        RuleFor(recipientLog => recipientLog.RecipientState, faker => faker.PickRandom<RecipientState>());
        RuleFor(recipientLog => recipientLog.RegisterDate, faker => faker.Date.Recent().ToUniversalTime());
        RuleFor(recipientLog => recipientLog.LevelPercentage, faker => new LevelPercentage(faker.Random.Byte(0, 100)));
        Ignore(recipientLog => recipientLog.RecipientMonitor);
        Ignore(recipientLog => recipientLog.RecipientMonitorId);
    }

    public RecipientLogBuilder WithLogDate(DateTime registerDate)
    {
        RuleFor(recipientLog => recipientLog.RegisterDate, registerDate);
        return this;
    }

    public RecipientLogBuilder WithRegisterDateBefore(DateTime referenceDate)
    {
        RuleFor(recipientLog => recipientLog.RegisterDate, faker => faker.Date.Past(refDate: referenceDate));
        return this;
    }

    public RecipientLogBuilder WithRecipientMonitor(RecipientMonitor recipientMonitor)
    {
        RuleFor(recipientLog => recipientLog.RecipientMonitor, recipientMonitor);
        return this;
    }
}