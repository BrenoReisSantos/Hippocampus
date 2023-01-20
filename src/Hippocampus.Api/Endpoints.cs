using Hippocampus.Models;
using Hippocampus.Models.Context;
using Hippocampus.Models.Dto;
using Hippocampus.Models.Entities;
using Hippocampus.Services.ApplicationValues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Api;

public static class LogRoutes
{
    public static void MapLogRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGroup("log");
        app.MapGet("/{recipientId}", GetLog);
        app.MapPost("/register", RegisterNewLog);
    }

    static async Task<IResult> RegisterNewLog(
        [FromServices] LogContext context,
        [FromServices] IClock clock,
        RecipientLogRegisterDto newRegister)
    {
        var newLog = newRegister.ToEntity(clock);
        context.RecipientLogs.Add(newLog);
        await context.SaveChangesAsync();
        return Results.Created($"api/{newLog.RecipientLogId}", newLog);
    }

    static async Task<IResult> GetLog(
        [FromServices] LogContext context,
        [FromRoute] RecipientLogId recipientId
    )
    {
        var recipient = await context.RecipientLogs.SingleOrDefaultAsync(rlog => rlog.RecipientLogId == recipientId);
        return recipient is not null
            ? Results.Ok(recipient)
            : Results.NotFound();
    }
}