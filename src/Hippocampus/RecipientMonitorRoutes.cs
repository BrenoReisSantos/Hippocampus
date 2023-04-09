using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Services;
using Hippocampus.Domain.Services.ApplicationValues;
using Microsoft.AspNetCore.Mvc;

namespace Hippocampus.Api;

public static class RecipientMonitorRoutes
{
    public static void MapLogRoutes(this IEndpointRouteBuilder app)
    {
        var recipientMonitorsGroup = app.MapGroup("RecipientMonitors");
        recipientMonitorsGroup.MapPost("", CreateNewRecipientMonitor).WithSummary("Created a new Recipient Monitor")
            .Produces<RecipientMonitorCreatedDto>()
            .Produces<MessageResponse>(400);
        recipientMonitorsGroup.MapGet("list", GetListOfRecipientMonitors).WithSummary("Get the list of all Monitors")
            .Produces<IEnumerable<RecipientMonitorForMonitorsTableDto>>();
    }

    private static async Task<IResult> CreateNewRecipientMonitor(
        [FromServices] IRecipientMonitorServices recipientMonitorServices,
        RecipientMonitorPostDto monitor)
    {
        var serviceResult = await recipientMonitorServices.InsertNewRecipientMonitor(monitor);
        if (serviceResult.IsFailure) return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.Ok(serviceResult.Result);
    }

    private static async Task<IResult> GetListOfRecipientMonitors(
        [FromServices] IRecipientMonitorServices recipientMonitorServices) =>
        Results.Ok(await recipientMonitorServices.GetRecipientMonitorsForMonitorsTable());
}