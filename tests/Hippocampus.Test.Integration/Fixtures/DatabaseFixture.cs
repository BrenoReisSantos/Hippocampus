using Hippocampus.Domain.Repository.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;

namespace Hippocampus.Test.Integration.Fixtures;

public class DatabaseFixture : IDisposable
{
    private static readonly string connectionString =
        "Server=localhost;Port=5432;Database=HippocampusTest;User Id=postgres;Password=postgres;";

    public HippocampusContext Context { get; set; }

    public DatabaseFixture()
    {
        Context = new HippocampusContext(
            new DbContextOptionsBuilder().UseNpgsql(connectionString).Options
        );
        MigrateDatabase();
    }

    private void MigrateDatabase()
    {
        if (Context.Database.GetPendingMigrations().Any())
            Context.Database.Migrate();
    }

    public async Task ResetDatabase()
    {
        var options = new RespawnerOptions
        {
            TablesToIgnore = ["__EFMigrationsHistory"],
            SchemasToInclude = ["public"],
            DbAdapter = DbAdapter.Postgres,
        };
        var connection = new NpgsqlConnection(connectionString);

        await connection.OpenAsync();

        var respawner = await Respawner.CreateAsync(connection, options);
        await respawner.ResetAsync(connection);
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
    }
}

[CollectionDefinition("database")]
public class DatabaseCollectionFixture : ICollectionFixture<DatabaseFixture> { }
