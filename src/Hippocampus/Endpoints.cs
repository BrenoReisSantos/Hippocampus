using Hippocampus.Domain.Models.Context;
using Hippocampus.Domain.Models.Dto;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Hippocampus.Domain.Services.ApplicationValues;
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