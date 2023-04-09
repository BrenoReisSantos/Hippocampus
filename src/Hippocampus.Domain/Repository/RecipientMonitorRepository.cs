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
            Name = recipientMonitor.Name,
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
}