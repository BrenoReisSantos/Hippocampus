using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Hippocampus.Domain.Repository.Context;
using Hippocampus.Domain.Services.ApplicationValues;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Domain.Repository;

public interface IWaterTankRepository
{
    Task<WaterTank> Insert(WaterTank waterTank);
    Task<WaterTank?> Get(WaterTankId waterTankId);
    Task<IEnumerable<WaterTank>> GetAll();
    Task<WaterTank?> Update(WaterTank waterTank);
    Task<bool> Exists(WaterTankId waterTankId);
    Task Delete(WaterTankId waterTankId);
}

public class WaterTankRepository : IWaterTankRepository
{
    private readonly HippocampusContext _context;
    private readonly IClock _clock;

    public WaterTankRepository(HippocampusContext context, IClock clock)
    {
        _context = context;
        _clock = clock;
    }

    public async Task<WaterTank> Insert(WaterTank waterTank)
    {
        var newRecipient = waterTank with
        {
            Name = waterTank.Name.Trim(),
            CreatedAt = _clock.Now.ToUniversalTime(),
            WaterTankId = WaterTankId.New()
        };

        _context.Add(newRecipient);

        await _context.SaveChangesAsync();

        return newRecipient;
    }

    public async Task<WaterTank?> Get(
        WaterTankId waterTankId)
    {
        return await _context.WaterTank.Include(recipientMonitor => recipientMonitor.PumpsTo)
            .SingleOrDefaultAsync(recipientMonitor => recipientMonitor.WaterTankId == waterTankId);
    }

    public async Task<IEnumerable<WaterTank>> GetAll()
    {
        return await _context.WaterTank
            .Include(r => r.PumpsTo)
            .Include(recipient =>
                recipient.WaterTankLogs.OrderByDescending(
                        recipientLog => recipientLog.LogDate)
                    .Take(1))
            .ToListAsync();
    }

    public async Task<WaterTank?> Update(WaterTank waterTank)
    {
        var waterTankUpdating = await _context.WaterTank.AsNoTracking().Include(r => r.PumpsTo).FirstOrDefaultAsync(
            r =>
                r.WaterTankId == waterTank.WaterTankId);

        if (waterTankUpdating is null) return null;

        waterTankUpdating = waterTankUpdating with
        {
            Name = waterTank.Name.Trim(),
            LevelWhenFull = waterTank.LevelWhenFull,
            LevelWhenEmpty = waterTank.LevelWhenEmpty,
            CreatedAt = waterTankUpdating.CreatedAt,
            CurrentLevel = waterTank.CurrentLevel,
            IsActive = waterTank.IsActive,
            State = waterTank.State,
            Type = waterTank.Type,
            PumpingWater = waterTank.PumpingWater,
            UpdatedAt = _clock.Now.ToUniversalTime(),
            PumpsTo = waterTank.PumpsTo,
        };

        _context.Update(waterTankUpdating);
        await _context.SaveChangesAsync();
        return waterTankUpdating;
    }

    public async Task<bool> Exists(WaterTankId waterTankId)
    {
        return await _context.WaterTank.AnyAsync(r => r.WaterTankId == waterTankId);
    }

    public async Task Delete(WaterTankId waterTankId)
    {
        var monitor = await _context.WaterTank.FindAsync(waterTankId);
        if (monitor is null) return;
        monitor.IsActive = false;
        await _context.SaveChangesAsync();
    }
}