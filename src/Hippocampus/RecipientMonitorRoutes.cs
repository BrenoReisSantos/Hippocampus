using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Hippocampus.Api;

public static class RecipientMonitorRoutes
{
    public static void MapMonitorRoutes(this IEndpointRouteBuilder app)
    {
        var recipientMonitorsGroup = app.MapGroup("WaterTank");
        recipientMonitorsGroup
            .MapPost("", PostWaterTank)
            .WithSummary("Created a new Water Tank")
            .Produces<WaterTankCreatedDto>()
            .Produces<MessageResponse>(400);
        recipientMonitorsGroup
            .MapGet("list", GetListOfRecipientMonitors)
            .WithSummary("Get the list of all WaterTanks")
            .Produces<IEnumerable<WaterTankForTableDto>>();
        recipientMonitorsGroup.MapPut("", PutRecipientMonitor);
        recipientMonitorsGroup.MapGet("{monitorId}", GetMonitor);
        recipientMonitorsGroup.MapPatch("updateLevel/{waterTankId}", ExecuteWaterTankStateUpdate);
        recipientMonitorsGroup.MapPut("turnOnPump/{waterTankId}", TurnOnPump);
        recipientMonitorsGroup.MapPut("turnOffPump/{waterTankId}", TurnOffPump);
    }

    private static async Task<IResult> PostWaterTank(
        [FromServices] IWaterTankService waterTankService,
        WaterTankCreateDto waterTank
    )
    {
        var serviceResult = await waterTankService.Create(waterTank);
        if (serviceResult.IsFailure)
            return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.Ok(serviceResult.Result);
    }

    private static async Task<IResult> GetListOfRecipientMonitors(
        [FromServices] IWaterTankService waterTankService
    )
    {
        return Results.Ok(await waterTankService.GetForTable());
    }

    private static async Task<IResult> PutRecipientMonitor(
        [FromServices] IWaterTankService waterTankService,
        [FromBody] WaterTankUpdateDto updateMonitor
    )
    {
        var serviceResult = await waterTankService.Update(updateMonitor);
        if (serviceResult.IsFailure)
            return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.Ok(serviceResult.Result);
    }

    private static async Task<IResult> GetMonitor(
        [FromServices] IWaterTankService waterTankService,
        WaterTankId monitorId
    )
    {
        var serviceResult = await waterTankService.Get(monitorId);
        if (serviceResult.IsFailure)
            return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.Ok(serviceResult.Result);
    }

    private static async Task<IResult> ExecuteWaterTankStateUpdate(
        [FromServices] IWaterTankStateUpdateProcessor waterTankStateUpdateProcessor,
        [FromRoute] Guid waterTankId,
        [FromBody] WaterTankLevelUpdateDto waterTankLevelUpdateDto
    )
    {
        WaterTankId.TryParse(waterTankId.ToString(), out var convertedWaterTankId);
        var serviceResult =
            await waterTankStateUpdateProcessor.UpdateWaterTankStateForWaterLevel(convertedWaterTankId,
                waterTankLevelUpdateDto);
        if (serviceResult.IsFailure)
            return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.NoContent();
    }

    private static async Task<IResult> TurnOnPump(
        [FromServices] IPumpControlService pumpControlService,
        [FromQuery] WaterTankId waterTankId
    )
    {
        var serviceResult = await pumpControlService.ControlPump(waterTankId, PumpPower.On);
        if (serviceResult.IsFailure)
            return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.NoContent();
    }

    private static async Task<IResult> TurnOffPump(
        [FromServices] IPumpControlService pumpControlService,
        [FromQuery] WaterTankId waterTankId
    )
    {
        var serviceResult = await pumpControlService.ControlPump(waterTankId, PumpPower.Off);
        if (serviceResult.IsFailure)
            return Results.BadRequest(new MessageResponse(serviceResult.Message));
        return Results.NoContent();
    }
}
