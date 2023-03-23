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
            .Produces<RecipientMonitorCreatedDto>();
    }

    private static async Task<IResult> CreateNewRecipientMonitor(
        [FromServices] IRecipientMonitorServices recipientMonitorServices,
        RecipientMonitorPostDto monitor)
    {
        var serviceResult = await recipientMonitorServices.InsertNewRecipientMonitor(monitor);
        if (serviceResult.IsFailure) return Results.BadRequest(serviceResult);
        return Results.Ok(serviceResult.Result);
    }
}