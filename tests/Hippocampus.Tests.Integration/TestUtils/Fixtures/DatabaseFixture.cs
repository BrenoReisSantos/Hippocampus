using Hippocampus.Domain.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hippocampus.Tests.Integration.TestUtils.Fixtures;

public class DatabaseFixture : ServiceFixture
{
    protected static string TestDatabaseConnectionString =>
        $"Server=localhost;Port=5432;Database=Hippocampus.Domain{Guid.NewGuid():N};User Id=postgres;Password=postgres;";

    AsyncServiceScope _dbScope;
    protected HippocampusContext Context { get; private set; }

    protected override void ConfigureAppConfiguration(IConfigurationBuilder configuration)
    {
        configuration.AddInMemoryCollection(
            new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] =
                    TestDatabaseConnectionString
            }
        );
        base.ConfigureAppConfiguration(configuration);
    }

    [SetUp]
    public async Task DatabaseFixtureSetup()
    {
        _dbScope = CreateScope();
        Context = _dbScope.ServiceProvider.GetRequiredService<HippocampusContext>();
        await Context.Database.MigrateAsync();
    }

    [TearDown]
    public async Task DatabaseFixtureTearDown()
    {
        await Context.Database.EnsureDeletedAsync();
        await _dbScope.DisposeAsync();
    }
}