using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Hippocampus.Api;

public static class MonitorLogRoutes
{
    public static void MapLogRoutes(this IEndpointRouteBuilder app)
    {
        var monitorLogGroup = app.MapGroup("MonitorLog");
        monitorLogGroup.MapGet("{monitorId}", GetMonitorLogs);
    }

    private static async Task<IResult> GetMonitorLogs(
        [FromServices] IRecipientMonitorServices recipientMonitorServices,
        WaterTankId monitorId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var result = await recipientMonitorServices.GetLogsForDateRange(monitorId, startDate, endDate);
        if (result.IsFailure) return Results.BadRequest(result.Message);
        return Results.Ok(result.Result);
    }
}