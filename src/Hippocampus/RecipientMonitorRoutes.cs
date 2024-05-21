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
            .Produces<WaterTankCreatedDto>()
            .Produces<MessageResponse>(400);
        recipientMonitorsGroup.MapGet("list", GetListOfRecipientMonitors).WithSummary("Get the list of all Monitors")
            .Produces<IEnumerable<WaterTankForTableDto>>();
        recipientMonitorsGroup.MapPut("", PutRecipientMonitor);
        recipientMonitorsGroup.MapGet("{monitorId}", GetMonitor);
        recipientMonitorsGroup.MapGet("{monitorId}/{level}", UpdateLevel);
    }

    private static async Task<IResult> CreateNewRecipientMonitor(
        [FromServices] IRecipientMonitorServices recipientMonitorServices,
        WaterTankCreateDto monitor)
    {
        var serviceResult = await recipientMonitorServices.Create(monitor);
        if (serviceResult.IsFailure) return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.Ok(serviceResult.Result);
    }

    // TODO: Gerar os testes automatizados para checar o RecipientState e o RecipientLevelPercentage do DTO
    private static async Task<IResult> GetListOfRecipientMonitors(
        [FromServices] IRecipientMonitorServices recipientMonitorServices)
    {
        return Results.Ok(await recipientMonitorServices.GetForTable());
    }

    private static async Task<IResult> PutRecipientMonitor(
        [FromServices] IRecipientMonitorServices recipientMonitorServices,
        [FromBody] WaterTankUpdateDto updateMonitor)
    {
        var serviceResult = await recipientMonitorServices.Update(updateMonitor);
        if (serviceResult.IsFailure) return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.Ok(serviceResult.Result);
    }

    private static async Task<IResult> GetMonitor(
        [FromServices] IRecipientMonitorServices recipientMonitorServices,
        WaterTankId monitorId)
    {
        var serviceResult = await recipientMonitorServices.Get(monitorId);
        if (serviceResult.IsFailure) return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.Ok(serviceResult.Result);
    }

    private static async Task<IResult> UpdateLevel(
        [FromServices] IWaterTankLevelUpdateProcessor waterTankLevelUpdateProcessor,
        [FromQuery] WaterTankId waterTankId,
        [FromQuery] int newLevel)
    {
        var serviceResult = await waterTankLevelUpdateProcessor.Update(waterTankId, newLevel);
        if (serviceResult.IsFailure) return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.NoContent();
    }
}