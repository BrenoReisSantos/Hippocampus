using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hippocampus.Api;

public static class RecipientMonitorRoutes
{
    public static void MapMonitorRoutes(this IEndpointRouteBuilder app)
    {
        var recipientMonitorsGroup = app.MapGroup("RecipientMonitors");
        recipientMonitorsGroup.MapPost("", CreateNewRecipientMonitor).WithSummary("Created a new Recipient Monitor")
            .Produces<RecipientMonitorCreatedDto>()
            .Produces<MessageResponse>(400);
        recipientMonitorsGroup.MapGet("list", GetListOfRecipientMonitors).WithSummary("Get the list of all Monitors")
            .Produces<IEnumerable<RecipientMonitorForMonitorsTableDto>>();
        recipientMonitorsGroup.MapPut("", PutRecipientMonitor);
        recipientMonitorsGroup.MapGet("{monitorId}", GetMonitor);
    }

    private static async Task<IResult> CreateNewRecipientMonitor(
        [FromServices] IRecipientMonitorServices recipientMonitorServices,
        RecipientMonitorPostDto monitor)
    {
        var serviceResult = await recipientMonitorServices.InsertNewRecipientMonitor(monitor);
        if (serviceResult.IsFailure) return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.Ok(serviceResult.Result);
    }

    // TODO: Gerar os testes automatizados para checar o RecipientState e o RecipientLevelPercentage do DTO
    private static async Task<IResult> GetListOfRecipientMonitors(
        [FromServices] IRecipientMonitorServices recipientMonitorServices)
    {
        return Results.Ok(await recipientMonitorServices.GetRecipientMonitorsForMonitorsTable());
    }

    private static async Task<IResult> PutRecipientMonitor(
        [FromServices] IRecipientMonitorServices recipientMonitorServices,
        [FromBody] RecipientMonitorPutDto putMonitor)
    {
        var serviceResult = await recipientMonitorServices.UpdateRecipientMonitor(putMonitor);
        if (serviceResult.IsFailure) return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.Ok(serviceResult.Result);
    }

    private static async Task<IResult> GetMonitor(
        [FromServices] IRecipientMonitorServices recipientMonitorServices,
        RecipientMonitorId monitorId)
    {
        var serviceResult = await recipientMonitorServices.GetRecipientMonitorById(monitorId);
        if (serviceResult.IsFailure) return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.Ok(serviceResult.Result);
    }
}