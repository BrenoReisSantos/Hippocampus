using Hippocampus.Models;
using Hippocampus.Models.Context;
using Hippocampus.Models.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Api;

public static class LogRoutes
{
    public static void MapLogRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGroup("log");
        app.MapGet("/{id}", GetLog);
        app.MapPost("/register", RegisterNewLog);
    }

    static async Task<IResult> RegisterNewLog(
        [FromServices] LogContext context,
        [FromBody] RecipientLogRegisterDto newRegister)
    {
        var newLog = newRegister.ToEntity();
        context.RecipientLogs.Add(newLog);
        await context.SaveChangesAsync();
        return Results.Created("", newRegister);
    }

    static async Task<IResult> GetLog(
        [FromServices] LogContext context,
        [FromRoute] RecipientLogId recipientId
    )
    {
        var recipient = await context.RecipientLogs.SingleOrDefaultAsync(rlog => rlog.Id == recipientId);
        return recipient is not null
            ? Results.Ok(recipient)
            : Results.NotFound();
    }
}
