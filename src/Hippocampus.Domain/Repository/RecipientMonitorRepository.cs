using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Hippocampus.Domain.Repository.Context;
using Hippocampus.Domain.Services.ApplicationValues;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Domain.Repository;

public interface IRecipientMonitorRepository
{
    Task<RecipientMonitor> InsertRecipientMonitor(RecipientMonitor recipientMonitor);
    Task<RecipientMonitor?> GetRecipientMonitorWithMonitorLinkedToByMacAddress(MacAddress macAddress);
    Task<RecipientMonitor?> Get(RecipientMonitorId recipientMonitorId);
    Task<IEnumerable<RecipientMonitor>> GetAllLinkedMonitor();
    Task<RecipientMonitor?> Update(RecipientMonitor changedRecipientMonitor);
    Task<bool> ExistsMonitor(RecipientMonitorId recipientMonitorId);
    Task DeleteRecipientMonitor(RecipientMonitorId recipientMonitorId);
}

public class RecipientMonitorRepository : IRecipientMonitorRepository
{
    private readonly HippocampusContext _context;
    private readonly IClock _clock;

    public RecipientMonitorRepository(HippocampusContext context, IClock clock)
    {
        _context = context;
        _clock = clock;
    }

    public async Task<RecipientMonitor> InsertRecipientMonitor(RecipientMonitor recipientMonitor)
    {
        RecipientMonitor? linkedRecipientMonitor = null;

        if (recipientMonitor.MonitorLinkedTo is not null)
            linkedRecipientMonitor =
                await _context.RecipientMonitors.FindAsync(recipientMonitor.MonitorLinkedTo?.RecipientMonitorId);

        var newRecipient = new RecipientMonitor()
        {
            Name = recipientMonitor.Name.Trim(),
            CreatedAt = _clock.Now.ToUniversalTime(),
            IsActive = true,
            MacAddress = recipientMonitor.MacAddress,
            MaxHeight = recipientMonitor.MaxHeight,
            MinHeight = recipientMonitor.MinHeight,
            RecipientType = recipientMonitor.RecipientType,
            RecipientMonitorId = RecipientMonitorId.New()
        };
        newRecipient.MonitorLinkedTo = linkedRecipientMonitor;

        _context.Add(newRecipient);

        if (linkedRecipientMonitor is not null)
            linkedRecipientMonitor.MonitorLinkedTo = newRecipient;

        await _context.SaveChangesAsync();

        return newRecipient;
    }

    public Task<RecipientMonitor?> GetRecipientMonitorWithMonitorLinkedToByMacAddress(MacAddress macAddress)
    {
        return _context.RecipientMonitors
            .AsSplitQuery()
            .Include(r => r.MonitorLinkedTo)
            .SingleOrDefaultAsync(r => r.MacAddress.Equals(macAddress));
    }

    public async Task<RecipientMonitor?> Get(
        RecipientMonitorId recipientMonitorId)
    {
        return await _context.RecipientMonitors.Include(recipientMonitor => recipientMonitor.MonitorLinkedTo)
            .SingleOrDefaultAsync(recipientMonitor => recipientMonitor.RecipientMonitorId == recipientMonitorId);
    }

    public async Task<IEnumerable<RecipientMonitor>> GetAllLinkedMonitor()
    {
        return await _context.RecipientMonitors
            .Include(r => r.MonitorLinkedTo)
            .Include(recipient =>
                recipient.RecipientLogs.OrderByDescending(
                        recipientLog => recipientLog.RegisterDate)
                    .Take(1))
            .ToListAsync();
    }

    public async Task<RecipientMonitor?> Update(RecipientMonitor changedRecipientMonitor)
    {
        var recipientToChange = await _context.RecipientMonitors.Include(r => r.MonitorLinkedTo).FirstOrDefaultAsync(
            r =>
                r.RecipientMonitorId == changedRecipientMonitor.RecipientMonitorId);

        if (recipientToChange is null) return null;

        recipientToChange.Name = changedRecipientMonitor.Name;
        recipientToChange.MaxHeight = changedRecipientMonitor.MaxHeight;
        recipientToChange.MinHeight = changedRecipientMonitor.MinHeight;
        recipientToChange.RecipientType = changedRecipientMonitor.RecipientType;
        recipientToChange.UpdatedAt = _clock.Now.ToUniversalTime();

        if (changedRecipientMonitor.MonitorLinkedTo is not null)
        {
            var linkedMonitor =
                await _context.RecipientMonitors.FindAsync(changedRecipientMonitor.MonitorLinkedTo.RecipientMonitorId);
            if (linkedMonitor is not null) recipientToChange.MonitorLinkedTo = linkedMonitor;
        }
        else
        {
            recipientToChange.MonitorLinkedTo = null;
        }

        await _context.SaveChangesAsync();

        return recipientToChange;
    }

    public async Task<bool> ExistsMonitor(RecipientMonitorId recipientMonitorId)
    {
        return await _context.RecipientMonitors.AnyAsync(r => r.RecipientMonitorId == recipientMonitorId);
    }

    public async Task DeleteRecipientMonitor(RecipientMonitorId recipientMonitorId)
    {
        var monitor = await _context.RecipientMonitors.FindAsync(recipientMonitorId);
        if (monitor is null) return;
        monitor.IsActive = false;
        await _context.SaveChangesAsync();
    }
}