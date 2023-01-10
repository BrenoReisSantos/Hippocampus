using Hippocampus.Models;
using Hippocampus.Models.Context;
using Hippocampus.Models.Values;
using Hippocampus.Services.ApplicationValues;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Hippocampus.Tests.Integration.Specs.Routes;

public class RegisterLogRoutesTests
{
    WebApplicationFactory<Program> _webApplicationFactory = null!;
    HttpClient _webApp = null!;
    AsyncServiceScope _scope;
    LogContext _context = null!;
    IClock _clock;

    [OneTimeSetUp]
    public void RegisterLogRoutesTestsOneTimeSetUp()
    {
        _webApplicationFactory = new();
        _webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            builder.ConfigureAppConfiguration(configurationBuilder =>
                configurationBuilder.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DefaultConnection"] =
                            $"Server=localhost;Port=5432;Database=Hippocampus-{Guid.NewGuid().ToString("N")};User Id=postgres;Password=postgres;"
                    }
                ));
        });
        _webApp = _webApplicationFactory.CreateClient();
    }

    [SetUp]
    public async Task RegisterLogRoutesTestsSetUp()
    {
        _scope = _webApplicationFactory.Services.CreateAsyncScope();
        _context = _scope.ServiceProvider.GetRequiredService<LogContext>();
        await _context.Database.EnsureCreatedAsync();
        _clock = _scope.ServiceProvider.GetRequiredService<IClock>();
    }

    [TearDown]
    public async Task RegisterLogRoutesTearDown()
    {
        await _context.Database.EnsureDeletedAsync();
        await _scope.DisposeAsync();
    }

    [OneTimeTearDown]
    public async Task RegisterLogRoutesOneTimeTearDown()
    {
        await _context.DisposeAsync();
    }

    [Test]
    public async Task Test()
    {
        RecipientLogId id = RecipientLogId.New();
        var registerDate = _clock.Now;
        _context.RecipientLogs.Add(new RecipientLog()
        {
            Id = id,
            Level = 50,
            State = State.Average,
            MacAddress = new MacAddress("11:11:11:11:11:11"),
            RegisterDate = registerDate
        });
        await _context.SaveChangesAsync();
        var test = await _context.RecipientLogs.SingleOrDefaultAsync(log => log.Id == id);
        test.Should().BeEquivalentTo(new RecipientLog()
        {
            Id = id,
            Level = 50,
            State = State.Average,
            MacAddress = new MacAddress("11:11:11:11:11:11"),
            RegisterDate = registerDate
        });
    }
}