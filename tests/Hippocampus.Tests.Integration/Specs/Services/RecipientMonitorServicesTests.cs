using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Values;
using Hippocampus.Domain.Services;
using Hippocampus.Domain.Services.ApplicationValues;
using Hippocampus.Tests.Common.Builders;
using Hippocampus.Tests.Integration.TestUtils.Fixtures;

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
            RecipientBoundary = new()
            {
                MaxHeight = recipientMonitorPostDto.MaxHeight,
                MinHeight = recipientMonitorPostDto.MinHeight
            },
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
            .WithRecipientMonitorLinkedToMacAddress(new MacAddress())
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
        InsertNewRecipientMonitor_Should_Return_RecipientMonitorCreatedDto_With_Linked_Monitor()
    {
        var linkedMonitor = new RecipientMonitorBuilder().Generate();
        Context.Add(linkedMonitor);
        await Context.SaveChangesAsync();

        var recipientMonitorPostDto = new RecipientMonitorPostDtoBuilder()
            .WithRecipientMonitorLinkedToMacAddress(linkedMonitor.MacAddress)
            .WithRecipientType(faker.PickRandomWithout(linkedMonitor.RecipientType))
            .Generate();

        var subject =
            await _recipientMonitorServices
                .InsertNewRecipientMonitor(recipientMonitorPostDto);

        var expectedResult = new RecipientMonitorCreatedDto()
        {
            Name = recipientMonitorPostDto.Name,
            CreatedAt = Clock.Now.ToUniversalTime(),
            RecipientBoundary = new()
            {
                MaxHeight = recipientMonitorPostDto.MaxHeight,
                MinHeight = recipientMonitorPostDto.MinHeight
            },
            RecipientType = recipientMonitorPostDto.RecipientType,
            MacAddress = recipientMonitorPostDto.MacAddress,
            RecipientMonitorLinkedTo = new()
            {
                RecipientType = linkedMonitor.RecipientType,
                MacAddress = linkedMonitor.MacAddress,
                RecipientBoundary = new()
                {
                    MaxHeight = linkedMonitor.RecipientBoundary.MaxHeight,
                    MinHeight = linkedMonitor.RecipientBoundary.MinHeight,
                },
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
}