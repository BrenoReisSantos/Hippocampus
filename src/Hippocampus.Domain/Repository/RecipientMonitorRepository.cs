using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Hippocampus.Domain.Repository.Context;
using Hippocampus.Domain.Services.ApplicationValues;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Domain.Repository;

public interface IWaterTankRepository
{
    Task<WaterTank> InsertWaterTank(WaterTank waterTank);
    Task<WaterTank?> Get(WaterTankId waterTankId);
    Task<IEnumerable<WaterTank>> GetAllLinkedMonitor();
    Task<WaterTank?> Update(WaterTank changedWaterTank);
    Task<bool> ExistsMonitor(WaterTankId waterTankId);
    Task DeleteRecipientMonitor(WaterTankId waterTankId);
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

    public async Task<WaterTank> InsertWaterTank(WaterTank waterTank)
    {
        WaterTank? linkedRecipientMonitor = null;

        if (waterTank.PumpsTo is not null)
            linkedRecipientMonitor =
                await _context.RecipientMonitors.FindAsync(waterTank.PumpsTo?.WaterTankId);

        var newRecipient = new WaterTank()
        {
            Name = waterTank.Name.Trim(),
            CreatedAt = _clock.Now.ToUniversalTime(),
            IsActive = true,
            LevelWhenFull = waterTank.LevelWhenFull,
            LevelWhenEmpty = waterTank.LevelWhenEmpty,
            WaterTankType = waterTank.WaterTankType,
            WaterTankId = WaterTankId.New()
        };
        newRecipient.PumpsTo = linkedRecipientMonitor;

        _context.Add(newRecipient);

        if (linkedRecipientMonitor is not null)
            linkedRecipientMonitor.PumpsTo = newRecipient;

        await _context.SaveChangesAsync();

        return newRecipient;
    }

    public async Task<WaterTank?> Get(
        WaterTankId waterTankId)
    {
        return await _context.RecipientMonitors.Include(recipientMonitor => recipientMonitor.PumpsTo)
            .SingleOrDefaultAsync(recipientMonitor => recipientMonitor.WaterTankId == waterTankId);
    }

    public async Task<IEnumerable<WaterTank>> GetAllLinkedMonitor()
    {
        return await _context.RecipientMonitors
            .Include(r => r.PumpsTo)
            .Include(recipient =>
                recipient.WaterTankLogs.OrderByDescending(
                        recipientLog => recipientLog.LogDate)
                    .Take(1))
            .ToListAsync();
    }

    public async Task<WaterTank?> Update(WaterTank changedWaterTank)
    {
        var recipientToChange = await _context.RecipientMonitors.Include(r => r.PumpsTo).FirstOrDefaultAsync(
            r =>
                r.WaterTankId == changedWaterTank.WaterTankId);

        if (recipientToChange is null) return null;

        recipientToChange.Name = changedWaterTank.Name;
        recipientToChange.LevelWhenFull = changedWaterTank.LevelWhenFull;
        recipientToChange.LevelWhenEmpty = changedWaterTank.LevelWhenEmpty;
        recipientToChange.WaterTankType = changedWaterTank.WaterTankType;
        recipientToChange.UpdatedAt = _clock.Now.ToUniversalTime();

        if (changedWaterTank.PumpsTo is not null)
        {
            var linkedMonitor =
                await _context.RecipientMonitors.FindAsync(changedWaterTank.PumpsTo.WaterTankId);
            if (linkedMonitor is not null) recipientToChange.PumpsTo = linkedMonitor;
        }
        else
        {
            recipientToChange.PumpsTo = null;
        }

        await _context.SaveChangesAsync();

        return recipientToChange;
    }

    public async Task<bool> ExistsMonitor(WaterTankId waterTankId)
    {
        return await _context.RecipientMonitors.AnyAsync(r => r.WaterTankId == waterTankId);
    }

    public async Task DeleteRecipientMonitor(WaterTankId waterTankId)
    {
        var monitor = await _context.RecipientMonitors.FindAsync(waterTankId);
        if (monitor is null) return;
        monitor.IsActive = false;
        await _context.SaveChangesAsync();
    }
}