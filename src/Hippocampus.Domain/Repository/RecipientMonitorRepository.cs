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
    Task<IEnumerable<RecipientMonitor>> GetAllRecipientMonitorsWithLinkedMonitor();
    Task<RecipientMonitor?> UpdateRecipientMonitor(RecipientMonitor changedRecipientMonitor);
}

public class RecipientMonitorMonitorRepository : IRecipientMonitorRepository
{
    private readonly HippocampusContext _context;
    private readonly IClock _clock;

    public RecipientMonitorMonitorRepository(HippocampusContext context, IClock clock)
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
            RecipientMonitorId = RecipientMonitorId.New(),
        };
        newRecipient.MonitorLinkedTo = linkedRecipientMonitor;

        _context.Add(newRecipient);

        if (linkedRecipientMonitor is not null)
            linkedRecipientMonitor.MonitorLinkedTo = newRecipient;

        await _context.SaveChangesAsync();

        return newRecipient;
    }

    public Task<RecipientMonitor?> GetRecipientMonitorWithMonitorLinkedToByMacAddress(MacAddress macAddress) =>
        _context.RecipientMonitors
            .AsSplitQuery()
            .Include(r => r.MonitorLinkedTo)
            .SingleOrDefaultAsync(r => r.MacAddress.Equals(macAddress));

    public async Task<IEnumerable<RecipientMonitor>> GetAllRecipientMonitorsWithLinkedMonitor() =>
        await _context.RecipientMonitors
            .Include(r => r.MonitorLinkedTo)
            .ToListAsync();

    public async Task<RecipientMonitor?> UpdateRecipientMonitor(RecipientMonitor changedRecipientMonitor)
    {
        var recipientToChange = await _context.RecipientMonitors.Include(r => r.MonitorLinkedTo).SingleOrDefaultAsync(
            r =>
                r.RecipientMonitorId == changedRecipientMonitor.RecipientMonitorId);

        if (recipientToChange is null) return null;

        recipientToChange.Name = changedRecipientMonitor.Name;
        recipientToChange.MaxHeight = changedRecipientMonitor.MaxHeight;
        recipientToChange.MinHeight = changedRecipientMonitor.MinHeight;
        recipientToChange.UpdatedAt = _clock.Now.ToUniversalTime();

        if (changedRecipientMonitor.MonitorLinkedTo is not null)
        {
            var linkedMonitor =
                await _context.RecipientMonitors.FindAsync(changedRecipientMonitor.MonitorLinkedTo.RecipientMonitorId);
            if (linkedMonitor is not null) recipientToChange.MonitorLinkedTo = linkedMonitor;
        }
        else recipientToChange.MonitorLinkedTo = null;

        await _context.SaveChangesAsync();

        return recipientToChange;
    }
}