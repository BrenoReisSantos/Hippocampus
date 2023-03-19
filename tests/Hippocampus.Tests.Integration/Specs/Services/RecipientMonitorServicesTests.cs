using Hippocampus.Domain.Diplomat.HttpOut;
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
            WifiSsid = recipientMonitorPostDto.WifiSsid,
            WifiPassword = recipientMonitorPostDto.WifiPassword,
            MacAddress = recipientMonitorPostDto.MacAddress
        };

        var expected = ServiceResult<RecipientMonitorCreatedDto>.Success(expectedResult);

        subject.Should().BeEquivalentTo(expected, config => config.Excluding(r => r.Result!.RecipientMonitorId));
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
}