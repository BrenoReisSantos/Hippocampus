using Hippocampus.Domain.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Api;

public static class Extensions
{
    public static async Task MigrateDatabaseAsync(this WebApplication webApp)
    {
        using (var asyncScope = webApp.Services.CreateAsyncScope())
        {
            using (var context = asyncScope.ServiceProvider.GetRequiredService<HippocampusContext>())
            {
                try
                {
                    if (context.Database.GetMigrations().Any())
                        await context.Database.MigrateAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}