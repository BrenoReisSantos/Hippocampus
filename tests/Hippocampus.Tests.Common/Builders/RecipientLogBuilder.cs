using AutoBogus;
using Bogus;
using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Tests.Common.Builders;

public sealed class RecipientLogBuilder : AutoFaker<RecipientLog>
{
    public RecipientLogBuilder()
    {
        Ignore(rlog => rlog.RecipientLogId);
        RuleFor(rlog => rlog.State, faker => faker.PickRandom<State>());
        RuleFor(rlog => rlog.RegisterDate, faker => faker.Date.Recent().ToUniversalTime());
        Ignore(rlog => rlog.RecipientMonitor);
        Ignore(rlog => rlog.RecipientMonitorId);
    }

    public RecipientLogBuilder WithRegisterDate(DateTime registerDate)
    {
        RuleFor(rlog => rlog.RegisterDate, registerDate);
        return this;
    }
    
    public RecipientLogBuilder WithRegisterDateBefore(DateTime referenceDate)
    {
        RuleFor(rlog => rlog.RegisterDate, faker => faker.Date.Past(refDate: referenceDate));
        return this;
    }

    public RecipientLogBuilder WithRecipientMonitor(RecipientMonitor recipientMonitor)
    {
        RuleFor(rlog => rlog.RecipientMonitor, recipientMonitor);
        return this;
    }
}