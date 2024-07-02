using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Repository;
using Hippocampus.Tests.Common.Builders;
using Hippocampus.Tests.Integration.TestUtils.Fixtures;

namespace Hippocampus.Tests.Integration.Specs.Repository;

public class RecipientLogRepositoryTests : DatabaseFixture
{
    private IRecipientLogRepository _recipientLogRepository = null!;

    [SetUp]
    public void RecipientMonitorRepositoryTestsSetUp()
    {
        _recipientLogRepository = GetService<IRecipientLogRepository>();
    }

    [Test]
    public async Task GetMostRecentRecipientLogAsync_Should_Return_Most_Recent_Log_Of_A_RecipientMonitor()
    {
        var recipientMonitor = new WaterTankBuilder().Generate();
        Context.RecipientMonitors.Add(recipientMonitor);
        await Context.SaveChangesAsync();

        var mostRecentLogRegisterDate = Faker.Date.Recent().ToUniversalTime();
        var mostRecentLog = new WaterTankLogBuilder()
            .WithLogDate(mostRecentLogRegisterDate)
            .WithRecipientMonitor(recipientMonitor)
            .Generate();
        Context.RecipientLogs.Add(mostRecentLog);

        var otherLogs = new WaterTankLogBuilder()
            .WithRegisterDateBefore(mostRecentLogRegisterDate)
            .WithRecipientMonitor(recipientMonitor)
            .Generate(10);
        Context.AddRange(otherLogs);

        await Context.SaveChangesAsync();

        var subject = await _recipientLogRepository.GetMostRecentRecipientLogAsync(
            recipientMonitor.RecipientMonitorId
        );

        subject
            .Should()
            .BeEquivalentTo(
                mostRecentLog,
                config =>
                    config
                        .Excluding(recipientLog => recipientLog.RecipientMonitor)
                        .Excluding(recipientLog => recipientLog.RecipientLogId)
            );
    }

    [Test]
    public async Task InsertRecipientLog_Should_Return_Inserted_RecipientLog()
    {
        var recipient = new WaterTankBuilder().Generate();
        Context.Add(recipient);
        await Context.SaveChangesAsync();

        var recipientLog = new WaterTankLogBuilder()
            .WithLogDate(Clock.Now.ToUniversalTime())
            .WithRecipientMonitor(recipient)
            .Generate();

        var subject = await _recipientLogRepository.InsertRecipientLog(recipientLog);

        var expected = new RecipientLog
        {
            RecipientMonitor = recipient,
            RecipientLogId = 1,
            RecipientMonitorId = recipient.RecipientMonitorId,
            RecipientState = recipientLog.RecipientState,
            LevelHeight = recipientLog.LevelHeight,
            RegisterDate = recipientLog.RegisterDate
        };
        subject
            .Should()
            .BeEquivalentTo(
                expected,
                config => config.Excluding(rlog => rlog.RecipientMonitor.RecipientLogs)
            );
    }

    [Test]
    public async Task GetLogsForMonitorInAGivenDateRangeAsync_Should_Return_Logs_In_The_Given_Date_Range()
    {
        var recipient = new WaterTankBuilder().Generate();
        Context.Add(recipient);
        await Context.SaveChangesAsync();

        const int minutesDistance = 30;
        const int logsQuantity = 5;
        var baseDate = Faker.Date.Past().ToUniversalTime();

        var expectedRecipientLogs = new List<RecipientLog>();
        for (var i = 0; i < logsQuantity; i++)
        {
            var logDate = baseDate.AddMinutes(minutesDistance * i);
            var recipientLog = new WaterTankLogBuilder()
                .WithLogDate(logDate)
                .WithRecipientMonitor(recipient)
                .Generate();
            expectedRecipientLogs.Add(recipientLog);
            Context.Add(recipientLog);
            await Context.SaveChangesAsync();
        }

        var recoveredLogs = await _recipientLogRepository.GetLogsForMonitorInAGivenDateRangeAsync(
            recipient.RecipientMonitorId,
            baseDate,
            baseDate.AddMinutes(minutesDistance * (logsQuantity - 1))
        );

        recoveredLogs
            .Should()
            .BeEquivalentTo(
                expectedRecipientLogs,
                config => config.Excluding(r => r.RecipientMonitor)
            );
    }
}
