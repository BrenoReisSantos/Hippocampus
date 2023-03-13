using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Services;
using Hippocampus.Domain.Services.ApplicationValues;
using Hippocampus.Tests.Common.Builders;
using Hippocampus.Tests.Integration.TestUtils.Fixtures;

namespace Hippocampus.Tests.Integration.Specs.Services;

public class RecipientMonitorServices : DatabaseFixture
{
    private IRecipientMonitorServices _recipientMonitorServices = null!;

    [SetUp]
    public void RecipientMonitorServicesTests()
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
            MacAddress = recipientMonitorPostDto.MacAddress
        };

        var expected = ServiceResult<RecipientMonitorCreatedDto>.Success(expectedResult);

        subject.Should().BeEquivalentTo(expected, config => config.Excluding(r => r.Result!.RecipientMonitorId));
    }
}