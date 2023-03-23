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
            RecipientBoundary = new()
            {
                MaxHeight = recipientToCreate.MaxHeight,
                MinHeight = recipientToCreate.MinHeight
            },
            RecipientType = recipientToCreate.RecipientType,
        };

        subject.Should().Be200Ok().And.BeAs(expected, config => config.Excluding(r => r.RecipientMonitorId));
    }

    [Test]
    public async Task CreateNewRecipientMonitor_Should_Return_400BadRequest_For_MinHeight_Bigger_Than_MaxHeight()
    {
        var recipientToCreate = new RecipientMonitorPostDtoBuilder().WithInvalidMaxAndMinHeight().Generate();

        var subject = await Api.PostAsync(RouteUrl, JsonContent.Create(recipientToCreate));

        subject.Should().Be400BadRequest().And.HaveErrorMessage("Altura máxima não pode ser menor que altura mínima");
    }
}