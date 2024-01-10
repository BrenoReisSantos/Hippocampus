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
            MaxHeight = recipientInsert.MaxHeight,
            MinHeight = recipientInsert.MinHeight,
            UpdatedAt = null
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
        var recipientInsert = new RecipientMonitorBuilder().Generate();

        var insertedRecipientMonitor = await _recipientMonitorRepository.InsertRecipientMonitor(recipientInsert);

        var subject =
            await Context.RecipientMonitors.SingleOrDefaultAsync(
                rm =>
                    rm.RecipientMonitorId == insertedRecipientMonitor.RecipientMonitorId);

        var expected = new RecipientMonitor()
        {
            Name = recipientInsert.Name,
            CreatedAt = Clock.Now.ToUniversalTime(),
            MacAddress = recipientInsert.MacAddress,
            IsActive = true,
            RecipientType = recipientInsert.RecipientType,
            MaxHeight = insertedRecipientMonitor.MaxHeight,
            MinHeight = insertedRecipientMonitor.MinHeight,
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
            MaxHeight = monitor.MaxHeight,
            MinHeight = monitor.MinHeight,
            RecipientMonitorId = monitor.RecipientMonitorId,
            UpdatedAt = null,
            MonitorLinkedTo = new RecipientMonitor
            {
                Name = linkedMonitor.Name,
                CreatedAt = linkedMonitor.CreatedAt,
                MacAddress = linkedMonitor.MacAddress,
                IsActive = true,
                RecipientType = linkedMonitor.RecipientType,
                MinHeight = linkedMonitor.MinHeight,
                MaxHeight = linkedMonitor.MaxHeight,
                RecipientMonitorId = linkedMonitor.RecipientMonitorId,
                UpdatedAt = null
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

    [Test]
    public async Task
        GetAllRecipientMonitorsWithLinkedMonitor_Should_Return_All_RecipientMonitors_From_Database_With_Each_One_Linked_Monitor()
    {
        var linkedMonitors = new RecipientMonitorBuilder().Generate(5);
        Context.AddRange(linkedMonitors);

        await Context.SaveChangesAsync();

        var monitors = new RecipientMonitorBuilder().Generate(5);

        foreach (var (linkedMonitor, monitor) in linkedMonitors.Zip(monitors))
        {
            monitor.MonitorLinkedTo = linkedMonitor;
            linkedMonitor.MonitorLinkedTo = monitor;
            Context.Add(monitor);
        }

        await Context.SaveChangesAsync();

        var subject = await _recipientMonitorRepository.GetAllRecipientMonitorsWithLinkedMonitor();

        var expected = monitors.Concat(linkedMonitors);

        subject.Should().BeEquivalentTo(expected, config => config.IgnoringCyclicReferences());
    }

    [Test]
    public async Task
        GetAllRecipientMonitorsWithLinkedMonitor_Should_Have_Most_Recent_RecipientLog()
    {
        var monitors = new RecipientMonitorBuilder().Generate(1);
        Context.AddRange(monitors);

        var mostRecentDate = Faker.Date.Recent().ToUniversalTime();

        var monitorMostRecentLog = new RecipientLogBuilder().WithLogDate(mostRecentDate)
            .WithRecipientMonitor(monitors[0]).Generate();
        Context.Add(monitorMostRecentLog);

        var othersLogs = new RecipientLogBuilder().WithRegisterDateBefore(mostRecentDate)
            .WithRecipientMonitor(monitors[0]).Generate(10);
        Context.AddRange(othersLogs);

        await Context.SaveChangesAsync();

        var returnedValue = await _recipientMonitorRepository.GetAllRecipientMonitorsWithLinkedMonitor();

        var subject = returnedValue.ToArray()[0].RecipientLogs[0];

        subject.Should().BeEquivalentTo(monitorMostRecentLog,
            config => config.Excluding(logSubject => logSubject.RecipientMonitor).IgnoringCyclicReferences());
    }

    [Test]
    public async Task UpdateRecipientMonitor_Should_Update_Existing_RecipientMonitor()
    {
        var monitor = new RecipientMonitorBuilder().Generate();

        Context.Add(monitor);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();

        var monitorUpdated = new RecipientMonitorBuilder().WithId(monitor.RecipientMonitorId).Generate();

        monitor.Name = Faker.Random.Words(3);
        monitor.MinHeight = Faker.Random.Int(0, 50);
        monitor.MaxHeight = Faker.Random.Int(51, 100);

        var subject = await _recipientMonitorRepository.UpdateRecipientMonitor(monitorUpdated);

        var expected = new RecipientMonitor
        {
            Name = monitorUpdated.Name,
            CreatedAt = monitor.CreatedAt,
            MacAddress = monitor.MacAddress,
            IsActive = monitor.IsActive,
            MaxHeight = monitorUpdated.MaxHeight,
            MinHeight = monitorUpdated.MinHeight,
            UpdatedAt = Clock.Now.ToUniversalTime(),
            RecipientType = monitorUpdated.RecipientType,
            MonitorLinkedTo = null,
            RecipientMonitorId = monitor.RecipientMonitorId
        };

        subject.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task UpdateRecipientMonitor_Should_Update_Existing_RecipientMonitor_On_Database()
    {
        var monitor = new RecipientMonitorBuilder().Generate();

        Context.Add(monitor);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();

        var monitorUpdated = new RecipientMonitorBuilder().WithId(monitor.RecipientMonitorId).Generate();

        monitor.Name = Faker.Random.Words(3);
        monitor.MinHeight = Faker.Random.Int(0, 50);
        monitor.MaxHeight = Faker.Random.Int(51, 100);

        var result = await _recipientMonitorRepository.UpdateRecipientMonitor(monitorUpdated);

        var subject = await Context.RecipientMonitors.FindAsync(monitor.RecipientMonitorId);

        var expected = new RecipientMonitor
        {
            Name = monitorUpdated.Name,
            CreatedAt = monitor.CreatedAt,
            MacAddress = monitor.MacAddress,
            IsActive = monitor.IsActive,
            MaxHeight = monitorUpdated.MaxHeight,
            MinHeight = monitorUpdated.MinHeight,
            UpdatedAt = Clock.Now.ToUniversalTime(),
            RecipientType = monitorUpdated.RecipientType,
            MonitorLinkedTo = null,
            RecipientMonitorId = monitor.RecipientMonitorId
        };

        subject.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task DeleteRecipientMonitor_Should_Set_IsActive_Collumn_To_False()
    {
        var monitor = new RecipientMonitorBuilder().Generate();

        Context.Add(monitor);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();

        await _recipientMonitorRepository.DeleteRecipientMonitor(monitor.RecipientMonitorId);

        var subject = await Context.RecipientMonitors.FindAsync(monitor.RecipientMonitorId);

        var expected = new RecipientMonitor
        {
            Name = monitor.Name,
            CreatedAt = monitor.CreatedAt,
            MacAddress = monitor.MacAddress,
            IsActive = false,
            MaxHeight = monitor.MaxHeight,
            MinHeight = monitor.MinHeight,
            UpdatedAt = monitor.UpdatedAt,
            RecipientType = monitor.RecipientType,
            MonitorLinkedTo = null,
            RecipientMonitorId = monitor.RecipientMonitorId
        };

        subject.Should().BeEquivalentTo(expected);
    }
}