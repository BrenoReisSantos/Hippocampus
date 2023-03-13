using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Repository;
using Hippocampus.Tests.Common.Builders;
using Hippocampus.Tests.Integration.TestUtils.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Tests.Integration.Specs.Repository;

public class RecipientMonitorRepositoryTests : DatabaseFixture
{
    private IRecipientMonitorRepository _recipientMonitorRepository = null!;

    [SetUp]
    public void RecipientMonitorRepositoryTestsSetUp()
    {
        _recipientMonitorRepository = GetService<IRecipientMonitorRepository>();
    }

    [Test]
    public async Task InsertRecipientMonitor_Should_Return_Created_RecipientMonitor()
    {
        var recipientInsert = new RecipientMonitorBuilder().Generate();

        var subject = await _recipientMonitorRepository.InsertRecipientMonitor(recipientInsert);

        var expected = new RecipientMonitor()
        {
            Name = recipientInsert.Name,
            CreatedAt = Clock.Now.ToUniversalTime(),
            MacAddress = recipientInsert.MacAddress,
            IsActive = true,
            RecipientBoundary = recipientInsert.RecipientBoundary,
            UpdatedAt = null,
        };

        subject
            .Should()
            .BeEquivalentTo(
                expected,
                config =>
                    config.Excluding(r => r.RecipientMonitorId));
    }

    [Test]
    public async Task InsertRecipientMonitor_Should_Save_RecipientMonitor_In_Database()
    {
        var recipientToinsert = new RecipientMonitorBuilder().Generate();

        var insertedRecipientMonitor = await _recipientMonitorRepository.InsertRecipientMonitor(recipientToinsert);

        var subject =
            await Context.RecipientMonitors.SingleOrDefaultAsync(
                rm =>
                    rm.RecipientMonitorId == insertedRecipientMonitor.RecipientMonitorId);

        var expected = new RecipientMonitor()
        {
            Name = recipientToinsert.Name,
            CreatedAt = insertedRecipientMonitor.CreatedAt,
            MacAddress = recipientToinsert.MacAddress,
            IsActive = true,
            RecipientBoundary = insertedRecipientMonitor.RecipientBoundary,
            RecipientMonitorId = insertedRecipientMonitor.RecipientMonitorId,
            UpdatedAt = null
        };

        subject
            .Should()
            .BeEquivalentTo(
                expected);
    }
}