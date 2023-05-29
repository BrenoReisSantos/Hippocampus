using System.Net.Http.Json;
using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Tests.Common.Builders;
using Hippocampus.Tests.Integration.TestUtils.Fixtures;

namespace Hippocampus.Tests.Integration.Specs.Routes;

public class RegisterMonitorEndpointsTests : ApiFixture
{
    private const string RouteUrl = "api/RecipientMonitors";

    [Test]
    public async Task CreateNewRecipientMonitor_Should_Return_200Ok_With_RecipientMonitorCreatedDto()
    {
        var recipientToCreate = new RecipientMonitorPostDtoBuilder().Generate();

        var subject = await Api.PostAsync(RouteUrl, JsonContent.Create(recipientToCreate));

        var expected = new RecipientMonitorCreatedDto()
        {
            Name = recipientToCreate.Name,
            CreatedAt = Clock.Now.ToUniversalTime(),
            MacAddress = recipientToCreate.MacAddress,
            MaxHeight = recipientToCreate.MaxHeight,
            MinHeight = recipientToCreate.MinHeight,
            RecipientType = recipientToCreate.RecipientType,
        };

        subject.Should().Be200Ok().And.BeAs(expected, config => config.Excluding(r => r.RecipientMonitorId));
    }

    [Test]
    public async Task CreateNewRecipientMonitor_Should_Return_400BadRequest_With_Error_Message()
    {
        var recipientToCreate = new RecipientMonitorPostDtoBuilder().WithInvalidMaxAndMinHeight().Generate();

        var subject = await Api.PostAsync(RouteUrl, JsonContent.Create(recipientToCreate));

        subject.Should().Be400BadRequest().And.HaveErrorMessage("Altura máxima não pode ser menor que altura mínima");
    }

    [Test]
    public async Task GetListOfRecipientMonitors_Should_Return_List_Of_All_RecipientMonitors()
    {
        var linkedMonitors = new RecipientMonitorBuilder().Generate(5);
        Context.AddRange(linkedMonitors);
        await Context.SaveChangesAsync();

        var monitors = new RecipientMonitorBuilder().Generate(5);

        foreach (var (linked, monitor) in linkedMonitors.Zip(monitors))
        {
            monitor.MonitorLinkedTo = linked;
            linked.MonitorLinkedTo = monitor;
            Context.Add(monitor);
        }

        await Context.SaveChangesAsync();

        var subject = await Api.GetAsync("api/RecipientMonitors/list");

        var expected = Enumerable.Concat(monitors, linkedMonitors).Select(m => new RecipientMonitorForMonitorsTableDto
        {
            RecipientType = m.RecipientType,
            MacAddress = m.MacAddress,
            MaxHeight = m.MaxHeight,
            MinHeight = m.MinHeight,
            RecipientMonitorId = m.RecipientMonitorId,
            Name = m.Name,
            LinkedRecipientMonitorMacAddress = m.MonitorLinkedTo?.MacAddress,
        }).ToList();

        subject.Should().Be200Ok().And.BeAs(expected);
    }

    [Test]
    public async Task PutRecipientMonitor_Should_Return_200_Ok_With_ServiceResult_With_Success_And_Updated_Monitor()
    {
        var monitor = new RecipientMonitorBuilder().Generate();

        Context.Add(monitor);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();

        var monitorUpdated = new RecipientMonitorPutDtoBuilder().WithRecipientMonitorId(monitor.RecipientMonitorId)
            .Generate();

        monitor.Name = Faker.Random.Words(3);
        monitor.MinHeight = Faker.Random.Int(0, 50);
        monitor.MaxHeight = Faker.Random.Int(51, 100);

        var subject = await Api.PutAsync($"{RouteUrl}/", JsonContent.Create(monitorUpdated));

        var expected = new RecipientMonitorUpdatedDto
        {
            Name = monitorUpdated.Name,
            MacAddress = monitor.MacAddress,
            MaxHeight = monitorUpdated.MaxHeight,
            MinHeight = monitorUpdated.MinHeight,
            RecipientType = monitorUpdated.RecipientType,
            RecipientMonitorId = monitor.RecipientMonitorId,
        };

        subject.Should().Be200Ok().And.BeAs(expected);
    }

    [Test]
    public async Task PutRecipientMonitor_Should_Return_400_BadRequest_With_ServiceResult_With_Failure()
    {
        var monitor = new RecipientMonitorBuilder().Generate();

        Context.Add(monitor);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();

        var monitorUpdated = new RecipientMonitorPutDtoBuilder().WithInvalidMaxAndMinHeight()
            .WithRecipientMonitorId(monitor.RecipientMonitorId)
            .Generate();

        monitor.Name = Faker.Random.Words(3);
        monitor.MinHeight = Faker.Random.Int(0, 50);
        monitor.MaxHeight = Faker.Random.Int(51, 100);

        var subject = await Api.PutAsync($"{RouteUrl}/", JsonContent.Create(monitorUpdated));

        subject.Should().Be400BadRequest().And.HaveErrorMessage("Altura máxima não pode ser menor que altura mínima");
    }
}