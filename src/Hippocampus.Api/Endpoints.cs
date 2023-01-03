using Hippocampus.Models.Context;
using Hippocampus.Models.Dto;
using Hippocampus.Models.Values;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Hippocampus.Api;

public static class LogRoutes
{
    public static void MapLogRoutes(this IEndpointRouteBuilder app)
    {
        app.MapPost("/registerLog", RegisterNewLog);
    }

    static Results<Ok, NotFound<MacAddress>> RegisterNewLog(
        [FromServices] LogContext context,
        RecipientLogRegisterDto newRegister)
    {
        context.RecipientLogs.Add(newRegister.ToEntity());
        return Ok($"Log for {newRegister.MacAddress}");
    }
}