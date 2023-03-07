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