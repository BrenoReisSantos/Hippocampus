using Hippocampus.Models.Context;
using Hippocampus.Models.Dto;
using Hippocampus.Models.Entities;
using Hippocampus.Models.Values;
using Hippocampus.Services.ApplicationValues;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Api;

public static class LogRoutes
{
    public static void MapLogRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGroup("log");
        // app.MapGet("/{recipientId}", GetLog);
        // app.MapPost("/register", RegisterNewLog);
    }
}