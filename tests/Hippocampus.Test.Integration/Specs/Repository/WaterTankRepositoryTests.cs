using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Repository;
using Hippocampus.Domain.Repository.Context;
using Hippocampus.Domain.Services.ApplicationValues;
using Hippocampus.Test.Integration.Fixtures;
using Hippocampus.Tests.Common;
using Hippocampus.Tests.Common.Mocks;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Test.Integration.Specs.Repository;

[Collection("database")]
public class WaterTankRepositoryTests : BaseTest, IDisposable
{
    private readonly HippocampusContext _context;
    private readonly DatabaseFixture _databaseFixture;
    private readonly WaterTankRepository _sut;
    private readonly IClock _clock;

    public WaterTankRepositoryTests(DatabaseFixture databaseFixture)
    {
        _context = databaseFixture.Context;
        _databaseFixture = databaseFixture;
        _clock = ClockMocker.Mock();
        _sut = new WaterTankRepository(_context, _clock);
    }

    public void Dispose()
    {
        _databaseFixture.ResetDatabase().Wait();
    }

    [Fact]
    public async Task Insert_WaterTank_Returns()
    {
        var waterTank = new WaterTankBuilder().Generate();

        var actual = await _sut.Insert(waterTank);

        var expected = waterTank with
        {
            CreatedAt = _clock.Now,
        };

        actual.Should().BeEquivalentTo(expected, options => options.Excluding(w => w.WaterTankId));
    }

    [Fact]
    public async Task Insert_WaterTank_CreateNewWaterTankInDatabase()
    {
        var waterTank = new WaterTankBuilder().Generate();

        var returned = await _sut.Insert(waterTank);

        var expected = waterTank with
        {
            CreatedAt = _clock.Now,
        };

        var actual = await _context.WaterTank.AsNoTracking()
            .FirstOrDefaultAsync(w => w.WaterTankId == returned.WaterTankId);

        actual.Should().BeEquivalentTo(expected, options => options.Excluding(w => w.WaterTankId));
    }

    [Fact]
    public async Task Update_WaterTank_ReturnsUpdatedWaterTank()
    {
        var waterTankInDatabase = new WaterTankBuilder().Generate();

        _context.WaterTank.Add(waterTankInDatabase);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var waterTankGenerated = new WaterTankBuilder().WithId(waterTankInDatabase.WaterTankId).Generate();

        var actual = await _sut.Update(waterTankGenerated);

        var expected = waterTankGenerated with
        {
            UpdatedAt = _clock.Now,
            CreatedAt = waterTankInDatabase.CreatedAt,
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Update_WaterTank_UpdatedWaterTankInTheDatabaseWithTheSameId()
    {
        var waterTankInDatabase = new WaterTankBuilder().Generate();

        _context.WaterTank.Add(waterTankInDatabase);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var waterTankGenerated = new WaterTankBuilder().WithId(waterTankInDatabase.WaterTankId).Generate();

        await _sut.Update(waterTankGenerated);

        var actual =
            await _context.WaterTank.FirstOrDefaultAsync(w => w.WaterTankId == waterTankInDatabase.WaterTankId);

        var expected = waterTankGenerated with
        {
            UpdatedAt = _clock.Now,
            CreatedAt = waterTankInDatabase.CreatedAt,
        };

        actual.Should().BeEquivalentTo(expected);
    }
}