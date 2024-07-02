using AutoBogus;
using Bogus;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Tests.Common.Builders;

public sealed class WaterTankLogBuilder : AutoFaker<WaterTankLog>
{
    public WaterTankLogBuilder()
    {
        Ignore(recipientLog => recipientLog.WaterTankLogId);
        RuleFor(
            recipientLog => recipientLog.LogDate,
            faker => faker.Date.Recent().ToUniversalTime()
        );
        RuleFor(recipientLog => recipientLog.Level, faker => faker.Random.Int(0, 1000));
        Ignore(recipientLog => recipientLog.WaterTank);
        Ignore(recipientLog => recipientLog.WaterTankId);
    }

    public WaterTankLogBuilder WithLogDate(DateTime registerDate)
    {
        RuleFor(recipientLog => recipientLog.LogDate, registerDate);
        return this;
    }

    public WaterTankLogBuilder WithRegisterDateBefore(DateTime referenceDate)
    {
        RuleFor(
            recipientLog => recipientLog.LogDate,
            faker => faker.Date.Past(refDate: referenceDate)
        );
        return this;
    }

    public WaterTankLogBuilder WithRecipientMonitor(WaterTank waterTank)
    {
        RuleFor(recipientLog => recipientLog.WaterTank, waterTank);
        RuleFor(recipientLog => recipientLog.WaterTankId, waterTank.WaterTankId);
        return this;
    }
}
