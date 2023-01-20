using System.Net.Http.Json;
using Hippocampus.Models;
using Hippocampus.Models.Context;
using Hippocampus.Models.Dto;
using Hippocampus.Models.Values;
using Hippocampus.Services.ApplicationValues;
using Hippocampus.Tests.Common.Mocks;
using Hippocampus.Tests.Common.TestUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hippocampus.Tests.Integration.Specs.Routes;

public class RegisterLogRoutesTests
{
    private Faker _faker = new("pt_BR");
    WebApplicationFactory<Program> _webApplicationFactory = null!;
    HttpClient _webApp = null!;
    AsyncServiceScope _scope;
    LogContext _context = null!;
    IClock _clock = null!;

    [OneTimeSetUp]
    public void RegisterLogRoutesTestsOneTimeSetUp()
    {
        AssertionConfiguration.ConfigureOptions();
        _webApplicationFactory = new();
        _webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IClock>();
                services.AddSingleton(ClockMocker.MockClock());
            });
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
    public async Task RegisterPath_Should_Save_New_RecipientLog_In_The_Database()
    {
        var mac = _faker.Internet.Mac();
        var macAddress = new MacAddress(mac);
        var state = _faker.PickRandom<State>();
        var levelValue = _faker.Random.Byte(0, 100);
        var level = new RecipientLevel(levelValue);

        var newLog = new RecipientLogRegisterDto()
        {
            MacAddress = macAddress,
            State = state,
            Level = level
        };

        var response = await _webApp.PostAsync("api/register", JsonContent.Create(newLog));

        response.Should().Be201Created().And.BeAs(
            new RecipientLog()
            {
                MacAddress = macAddress,
                Level = level,
                State = state,
                RegisterDate = _clock.Now
            }, options => options.Excluding(log => log.RecipientLogId));
    }
}