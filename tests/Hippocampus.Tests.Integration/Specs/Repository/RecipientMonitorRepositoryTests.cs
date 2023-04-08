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
            RecipientType = recipientInsert.RecipientType,
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
            CreatedAt = Clock.Now.ToUniversalTime(),
            MacAddress = recipientToinsert.MacAddress,
            IsActive = true,
            RecipientType = recipientToinsert.RecipientType,
            RecipientBoundary = insertedRecipientMonitor.RecipientBoundary,
            RecipientMonitorId = insertedRecipientMonitor.RecipientMonitorId,
            UpdatedAt = null
        };

        subject
            .Should()
            .BeEquivalentTo(
                expected);
    }

    [Test]
    public async Task InsertRecipientMonitor_Should_Return_Created_RecipientMonitor_With_Linked_Monitor()
    {
        var linkedMonitor = new RecipientMonitorBuilder().Generate();
        Context.Add(linkedMonitor);
        await Context.SaveChangesAsync();

        var monitor = new RecipientMonitorBuilder().WithLinkedMonitor(linkedMonitor).Generate();

        var subject = await _recipientMonitorRepository.InsertRecipientMonitor(monitor);

        var expected = new RecipientMonitor()
        {
            Name = monitor.Name,
            CreatedAt = Clock.Now.ToUniversalTime(),
            MacAddress = monitor.MacAddress,
            IsActive = true,
            RecipientType = monitor.RecipientType,
            RecipientBoundary = monitor.RecipientBoundary,
            RecipientMonitorId = monitor.RecipientMonitorId,
            UpdatedAt = null,
            MonitorLinkedTo = new()
            {
                Name = linkedMonitor.Name,
                CreatedAt = linkedMonitor.CreatedAt,
                MacAddress = linkedMonitor.MacAddress,
                IsActive = true,
                RecipientType = linkedMonitor.RecipientType,
                RecipientBoundary = linkedMonitor.RecipientBoundary,
                RecipientMonitorId = linkedMonitor.RecipientMonitorId,
                UpdatedAt = null,
            }
        };

        subject
            .Should()
            .BeEquivalentTo(
                expected,
                config =>
                    config
                        .Excluding(r => r.RecipientMonitorId)
                        .Excluding(r => r.MonitorLinkedTo.MonitorLinkedTo)
                        .IgnoringCyclicReferences());
    }

    [Test]
    public async Task InsertRecipientMonitor_Should_Be_Created_With_LinkedMonitor()
    {
        var linkedMonitor = new RecipientMonitorBuilder().Generate();
        Context.Add(linkedMonitor);
        await Context.SaveChangesAsync();

        var monitor = new RecipientMonitorBuilder().WithLinkedMonitor(linkedMonitor).Generate();

        var subject = await _recipientMonitorRepository.InsertRecipientMonitor(monitor);

        Context.ChangeTracker.Clear();
        var expected =
            await Context.RecipientMonitors
                .AsSplitQuery()
                .Include(r => r.MonitorLinkedTo)
                .SingleOrDefaultAsync(
                    r => r.RecipientMonitorId == subject.RecipientMonitorId);

        subject
            .Should()
            .BeEquivalentTo(
                expected,
                config =>
                    config
                        .IgnoringCyclicReferences());
    }

    [Test]
    public async Task InsertRecipientMonitor_Should_Link_LinkedMonitor_With_Base_Monitor()
    {
        var linkedMonitor = new RecipientMonitorBuilder().Generate();
        Context.Add(linkedMonitor);
        await Context.SaveChangesAsync();

        var monitor = new RecipientMonitorBuilder().WithLinkedMonitor(linkedMonitor).Generate();

        var expected = await _recipientMonitorRepository.InsertRecipientMonitor(monitor);

        Context.ChangeTracker.Clear();
        var subject =
            await Context.RecipientMonitors
                .AsSplitQuery()
                .Include(r => r.MonitorLinkedTo)
                .SingleOrDefaultAsync(
                    r => r.RecipientMonitorId == linkedMonitor.RecipientMonitorId);

        subject.MonitorLinkedTo
            .Should()
            .BeEquivalentTo(
                expected,
                config =>
                    config
                        .IgnoringCyclicReferences());
    }
}