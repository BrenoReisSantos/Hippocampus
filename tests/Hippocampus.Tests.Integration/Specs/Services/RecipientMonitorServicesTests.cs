﻿using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Hippocampus.Domain.Services;
using Hippocampus.Domain.Services.ApplicationValues;
using Hippocampus.Tests.Common.Builders;
using Hippocampus.Tests.Integration.TestUtils.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Tests.Integration.Specs.Services;

public class RecipientMonitorServicesTests : DatabaseFixture
{
    private IRecipientMonitorServices _recipientMonitorServices = null!;

    [SetUp]
    public void RecipientMonitorServicesTestsSetUp()
    {
        _recipientMonitorServices = GetService<IRecipientMonitorServices>();
    }

    [Test]
    public async Task InsertNewRecipientMonitor_Should_Return_RecipientMonitorCreatedDto()
    {
        var recipientMonitorPostDto = new RecipientMonitorPostDtoBuilder().Generate();

        var subject = await _recipientMonitorServices.InsertNewRecipientMonitor(recipientMonitorPostDto);

        var expectedResult = new RecipientMonitorCreatedDto()
        {
            Name = recipientMonitorPostDto.Name,
            CreatedAt = Clock.Now.ToUniversalTime(),
            MaxHeight = recipientMonitorPostDto.MaxHeight,
            MinHeight = recipientMonitorPostDto.MinHeight,
            RecipientType = recipientMonitorPostDto.RecipientType,
            MacAddress = recipientMonitorPostDto.MacAddress,
            RecipientMonitorLinkedTo = null,
        };

        var expected = ServiceResult<RecipientMonitorCreatedDto>.Success(expectedResult);

        subject
            .Should()
            .BeEquivalentTo(
                expected,
                config =>
                    config
                        .Excluding(r => r.Result!.RecipientMonitorId));
    }

    [Test]
    public async Task InsertNewRecipientMonitor_Should_Return_Error_For_MinHeight_Bigger_Than_MaxHeight()
    {
        var recipientMonitorPostDto = new RecipientMonitorPostDtoBuilder().WithInvalidMaxAndMinHeight().Generate();

        var subject = await _recipientMonitorServices.InsertNewRecipientMonitor(recipientMonitorPostDto);

        var expected = ServiceResult<RecipientMonitorCreatedDto>.Error(
            "Altura máxima não pode ser menor que altura mínima");

        subject.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task
        InsertNewRecipientMonitor_Should_Return_Error_For_PostDto_Has_Linked_Monitor_MacAddress_But_Linked_Monitor_Not_Found()
    {
        var recipientMonitorPostDto = new RecipientMonitorPostDtoBuilder()
            .WithRecipientMonitorLinkedToMacAddress(new MacAddress(Faker.Internet.Mac()))
            .Generate();

        var subject = await _recipientMonitorServices.InsertNewRecipientMonitor(recipientMonitorPostDto);

        var expected = ServiceResult<RecipientMonitorCreatedDto>.Error("Monitor Relacionado não encontrado");

        subject
            .Should()
            .BeEquivalentTo(
                expected);
    }

    [Test]
    public async Task
        InsertNewRecipientMonitor_Should_Return_Error_For_Linked_Monitor_Has_Same_RecipientType_As_Monitor_Being_Inserted()
    {
        var linkedMonitor = new RecipientMonitorBuilder().Generate();
        Context.Add(linkedMonitor);
        await Context.SaveChangesAsync();

        var recipientMonitorPostDto = new RecipientMonitorPostDtoBuilder()
            .WithRecipientMonitorLinkedToMacAddress(linkedMonitor.MacAddress)
            .WithRecipientType(linkedMonitor.RecipientType)
            .Generate();

        var subject =
            await _recipientMonitorServices
                .InsertNewRecipientMonitor(recipientMonitorPostDto);

        var expected = ServiceResult<RecipientMonitorCreatedDto>.Error(
            "o Monitor cadastrado sendo cadastrado não pode pertencer ao mesmo tipo de recipient que o monitor conectado");

        subject
            .Should()
            .BeEquivalentTo(expected);
    }

    [Test]
    public async Task
        InsertNewRecipientMonitor_Should_Return_Error_For_Trying_To_Link_With_A_Monitor_Already_Linked()
    {
        var otherMonitor = new RecipientMonitorBuilder().Generate();
        Context.Add(otherMonitor);
        await Context.SaveChangesAsync();

        var linkedMonitor = new RecipientMonitorBuilder().Generate();
        linkedMonitor.MonitorLinkedTo = otherMonitor;
        Context.Add(linkedMonitor);

        otherMonitor.MonitorLinkedTo = linkedMonitor;

        await Context.SaveChangesAsync();

        var recipientMonitorPostDto = new RecipientMonitorPostDtoBuilder()
            .WithRecipientMonitorLinkedToMacAddress(linkedMonitor.MacAddress)
            .WithRecipientType(Faker.PickRandomWithout(linkedMonitor.RecipientType))
            .Generate();

        var subject =
            await _recipientMonitorServices
                .InsertNewRecipientMonitor(recipientMonitorPostDto);

        var expected = ServiceResult<RecipientMonitorCreatedDto>.Error(
            $"O monitor a se conectar já está conectado com um outro. ({otherMonitor.Name} with Macaddress {otherMonitor.MacAddress})");

        subject
            .Should()
            .BeEquivalentTo(
                expected,
                config =>
                    config
                        .Excluding(r => r.Result!.RecipientMonitorId));
    }

    [Test]
    public async Task
        InsertNewRecipientMonitor_Should_Return_RecipientMonitorCreatedDto_With_Linked_Monitor()
    {
        var linkedMonitor = new RecipientMonitorBuilder().Generate();
        Context.Add(linkedMonitor);
        await Context.SaveChangesAsync();

        var recipientMonitorPostDto = new RecipientMonitorPostDtoBuilder()
            .WithRecipientMonitorLinkedToMacAddress(linkedMonitor.MacAddress)
            .WithRecipientType(Faker.PickRandomWithout(linkedMonitor.RecipientType))
            .Generate();

        var subject =
            await _recipientMonitorServices
                .InsertNewRecipientMonitor(recipientMonitorPostDto);

        var expectedResult = new RecipientMonitorCreatedDto()
        {
            Name = recipientMonitorPostDto.Name,
            CreatedAt = Clock.Now.ToUniversalTime(),
            MaxHeight = recipientMonitorPostDto.MaxHeight,
            MinHeight = recipientMonitorPostDto.MinHeight,
            RecipientType = recipientMonitorPostDto.RecipientType,
            MacAddress = recipientMonitorPostDto.MacAddress,
            RecipientMonitorLinkedTo = new()
            {
                RecipientType = linkedMonitor.RecipientType,
                MacAddress = linkedMonitor.MacAddress,
                MaxHeight = linkedMonitor.MaxHeight,
                MinHeight = linkedMonitor.MinHeight,
                RecipientMonitorId = linkedMonitor.RecipientMonitorId,
                Name = linkedMonitor.Name,
            },
        };

        var expected = ServiceResult<RecipientMonitorCreatedDto>.Success(expectedResult);

        subject
            .Should()
            .BeEquivalentTo(
                expected,
                config =>
                    config
                        .Excluding(r => r.Result!.RecipientMonitorId));
    }

    [Test]
    public async Task GetRecipientMonitorsForMonitorsTable_Should_Return_List_Of_RecipientMonitorForMonitorsTableDto()
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

        var subject = await _recipientMonitorServices.GetRecipientMonitorsForMonitorsTable();

        var expected = Enumerable.Concat(monitors, linkedMonitors).Select(m => new RecipientMonitorForMonitorsTableDto
        {
            RecipientType = m.RecipientType,
            MacAddress = m.MacAddress,
            MaxHeight = m.MaxHeight,
            MinHeight = m.MinHeight,
            RecipientMonitorId = m.RecipientMonitorId,
            Name = m.Name,
            LinkedRecipientMonitorMacAddress = m.MonitorLinkedTo?.MacAddress,
            RecipientLevelPercentage = null,
            RecipientState = null,
        });

        subject.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task
        GetRecipientMonitorsForMonitorsTable_Should_Return_RecipientMonitorForMonitorsTableDto_With_RecipientSate()
    {
        var monitors = new RecipientMonitorBuilder().Generate(1);
        Context.AddRange(monitors);

        var log = new RecipientLogBuilder().WithRecipientMonitor(monitors[0]).Generate();
        Context.Add(log);

        await Context.SaveChangesAsync();

        var monitorsForTable = await _recipientMonitorServices.GetRecipientMonitorsForMonitorsTable();

        var subject = monitorsForTable.ToList()[0].RecipientState;

        subject.Should().Be(log.RecipientState);
    }

    [Test]
    public async Task
        GetRecipientMonitorsForMonitorsTable_Should_Return_RecipientMonitorForMonitorsTableDto_With_RecipientLevel()
    {
        var monitors = new RecipientMonitorBuilder().Generate(1);
        Context.AddRange(monitors);

        var log = new RecipientLogBuilder().WithRecipientMonitor(monitors[0]).Generate();
        Context.Add(log);

        await Context.SaveChangesAsync();

        var monitorsForTable = await _recipientMonitorServices.GetRecipientMonitorsForMonitorsTable();

        var subject = monitorsForTable.ToList()[0].RecipientLevelPercentage;

        subject.Should().Be(log.LevelPercentage);
    }

    [Test]
    public async Task UpdateRecipientMonitor_Should_Return_RecipientMonitorCreatedDto()
    {
        var monitorLinkedTo = new RecipientMonitorBuilder().Generate();
        var monitor = new RecipientMonitorBuilder().WithLinkedMonitor(monitorLinkedTo).Generate();
        Context.Add(monitor);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();

        var otherRecipientMonitor = new RecipientMonitorBuilder().Generate();
        Context.Add(otherRecipientMonitor);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();

        var monitorToUpdate = new RecipientMonitorPutDtoBuilder()
            .WithRecipientType(Faker.PickRandomWithout(otherRecipientMonitor.RecipientType))
            .WithRecipientMonitorId(monitor.RecipientMonitorId)
            .WithRecipientMonitorLinkedToMacAddress(otherRecipientMonitor.MacAddress).Generate();

        var subject = await _recipientMonitorServices.UpdateRecipientMonitor(monitorToUpdate);

        var expectedResult = new RecipientMonitorUpdatedDto
        {
            Name = monitorToUpdate.Name,
            MaxHeight = monitorToUpdate.MaxHeight,
            MinHeight = monitorToUpdate.MinHeight,
            RecipientType = monitorToUpdate.RecipientType,
            MacAddress = monitor.MacAddress,
            RecipientMonitorLinkedTo = new RecipientMonitorLinkedToUpdatedDto
            {
                RecipientMonitorId = otherRecipientMonitor.RecipientMonitorId,
                Name = otherRecipientMonitor.Name,
                MacAddress = otherRecipientMonitor.MacAddress,
                MaxHeight = otherRecipientMonitor.MaxHeight,
                MinHeight = otherRecipientMonitor.MinHeight,
                RecipientType = otherRecipientMonitor.RecipientType
            },
        };

        var expected = ServiceResult<RecipientMonitorUpdatedDto>.Success(expectedResult);

        subject
            .Should()
            .BeEquivalentTo(
                expected,
                config =>
                    config
                        .Excluding(r => r.Result!.RecipientMonitorId));
    }

    [Test]
    public async Task UpdateRecipientMonitor_Should_Return_Error_For_MinHeight_Bigger_Than_MaxHeight()
    {
        var monitor = new RecipientMonitorBuilder().Generate();
        Context.Add(monitor);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();

        var recipientMonitorPutDto = new RecipientMonitorPutDtoBuilder()
            .WithRecipientMonitorId(monitor.RecipientMonitorId).WithInvalidMaxAndMinHeight().Generate();

        var subject = await _recipientMonitorServices.UpdateRecipientMonitor(recipientMonitorPutDto);

        var expected = ServiceResult<RecipientMonitorUpdatedDto>.Error(
            "Altura máxima não pode ser menor que altura mínima");

        subject.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task
        UpdateRecipientMonitor_Should_Return_Error_For_PostDto_Has_Linked_Monitor_MacAddress_But_Linked_Monitor_Not_Found()
    {
        var monitor = new RecipientMonitorBuilder().Generate();
        Context.Add(monitor);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();

        var recipientMonitorPutDto = new RecipientMonitorPutDtoBuilder()
            .WithRecipientMonitorId(monitor.RecipientMonitorId)
            .WithRecipientMonitorLinkedToMacAddress(new MacAddress(Faker.Internet.Mac()))
            .Generate();

        var subject = await _recipientMonitorServices.UpdateRecipientMonitor(recipientMonitorPutDto);

        var expected = ServiceResult<RecipientMonitorUpdatedDto>.Error("Monitor Relacionado não encontrado");

        subject
            .Should()
            .BeEquivalentTo(
                expected);
    }

    [Test]
    public async Task
        UpdateRecipientMonitor_Should_Return_Error_For_Linked_Monitor_Has_Same_RecipientType_As_Monitor_Being_Inserted()
    {
        var linkedMonitor = new RecipientMonitorBuilder().Generate();
        Context.Add(linkedMonitor);
        await Context.SaveChangesAsync();

        var monitor = new RecipientMonitorBuilder()
            .WithRecipientType(Faker.PickRandomWithout(linkedMonitor.RecipientType)).Generate();
        Context.Add(monitor);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();

        var monitorToUpdate = new RecipientMonitorPutDtoBuilder()
            .WithRecipientMonitorLinkedToMacAddress(linkedMonitor.MacAddress)
            .WithRecipientMonitorId(monitor.RecipientMonitorId)
            .WithRecipientType(linkedMonitor.RecipientType)
            .Generate();

        var subject =
            await _recipientMonitorServices
                .UpdateRecipientMonitor(monitorToUpdate);

        var expected = ServiceResult<RecipientMonitorCreatedDto>.Error(
            "o Monitor cadastrado sendo cadastrado não pode pertencer ao mesmo tipo de recipient que o monitor conectado");

        subject
            .Should()
            .BeEquivalentTo(expected);
    }

    [Test]
    public async Task
        UpdateRecipientMonitor_Should_Return_Error_For_Trying_To_Link_With_A_Monitor_Already_Linked()
    {
        var otherMonitor = new RecipientMonitorBuilder().Generate();
        Context.Add(otherMonitor);
        await Context.SaveChangesAsync();

        var linkedMonitor = new RecipientMonitorBuilder().Generate();
        linkedMonitor.MonitorLinkedTo = otherMonitor;
        Context.Add(linkedMonitor);

        otherMonitor.MonitorLinkedTo = linkedMonitor;

        await Context.SaveChangesAsync();

        var monitorToBeUpdated = new RecipientMonitorBuilder().Generate();
        Context.Add(monitorToBeUpdated);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();

        var recipientMonitorPutDto = new RecipientMonitorPutDtoBuilder()
            .WithRecipientMonitorId(monitorToBeUpdated.RecipientMonitorId)
            .WithRecipientMonitorLinkedToMacAddress(linkedMonitor.MacAddress)
            .WithRecipientType(Faker.PickRandomWithout(linkedMonitor.RecipientType))
            .Generate();

        var subject =
            await _recipientMonitorServices
                .UpdateRecipientMonitor(recipientMonitorPutDto);

        var expected = ServiceResult<RecipientMonitorCreatedDto>.Error(
            $"O monitor a se conectar já está conectado com um outro. ({otherMonitor.Name} with Macaddress {otherMonitor.MacAddress})");

        subject
            .Should()
            .BeEquivalentTo(
                expected,
                config =>
                    config
                        .Excluding(r => r.Result!.RecipientMonitorId));
    }
}