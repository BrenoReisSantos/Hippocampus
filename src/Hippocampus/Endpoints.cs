using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using RecipientMonitorPostDto = Hippocampus.Domain.Diplomat.HttpIn.RecipientMonitorPostDto;

namespace Hippocampus.Api;

public static class LogRoutes
{
    public static void MapLogRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGroup("monitor");
        app.MapPost("", CreateNewRecipientMonitor).WithSummary("Created a new Recipient Monitor")
            .Produces<RecipientMonitorCreatedDto>();
    }

    private static async Task<IResult> CreateNewRecipientMonitor(
        [FromServices] IRecipientMonitorServices recipientMonitorServices,
        RecipientMonitorPostDto monitor)
    {
        var serviceResult = await recipientMonitorServices.InsertNewRecipientMonitor(monitor);
        if (serviceResult.IsFailure) return Results.BadRequest();
        return Results.Ok(serviceResult.Result);
    }
}