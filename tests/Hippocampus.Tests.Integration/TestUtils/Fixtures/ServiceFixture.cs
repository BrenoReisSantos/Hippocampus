using Hippocampus.Domain.Services.ApplicationValues;
using Hippocampus.Tests.Common.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hippocampus.Tests.Integration.TestUtils.Fixtures;

class ProgramFactory : WebApplicationFactory<Program>
{
    readonly Action<IWebHostBuilder> _configure;

    public ProgramFactory(Action<IWebHostBuilder> configure) =>
        this._configure = configure;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var dir = Directory.GetCurrentDirectory();
        builder.UseContentRoot(dir);
        _configure(builder);
    }
}

public class ServiceFixture
{
    protected readonly Faker faker = new("pt_BR");
    protected AsyncServiceScope TestScope { get; private set; }
    private protected ProgramFactory ApplicationFactory { get; set; } = null!;
    protected IClock Clock { get; private set; }

    [SetUp]
    public void ServiceFixtureSetup()
    {
        ApplicationFactory = InitializeProgramFactory();
        TestScope = CreateScope();
        Clock = GetService<IClock>();
    }

    [TearDown]
    public async Task ServiceFixtureTearDown()
    {
        await TestScope.DisposeAsync();
    }

    [OneTimeTearDown]
    public void ServiceFixtureOneTimeTearDown()
    {
        ApplicationFactory.Dispose();
    }

    ProgramFactory InitializeProgramFactory() => new(builder => builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    ["TokenSettings:Key"] = Guid.NewGuid().ToString("N"),
                }
            );
            ConfigureAppConfiguration(config);
        })
        .ConfigureTestServices(services =>
        {
            services.Replace(ServiceDescriptor.Singleton(ClockMocker.Mock()));
            ConfigureTestServices(services);
        }));

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
    }

    protected virtual void ConfigureAppConfiguration(IConfigurationBuilder configuration)
    {
    }

    protected T GetService<T>() where T : notnull => TestScope.ServiceProvider.GetRequiredService<T>();
    protected AsyncServiceScope CreateScope() => ApplicationFactory.Services.CreateAsyncScope();
}