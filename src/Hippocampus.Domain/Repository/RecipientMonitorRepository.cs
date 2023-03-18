using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Repository.Context;
using Hippocampus.Domain.Services.ApplicationValues;

namespace Hippocampus.Domain.Repository;

public interface IRecipientMonitorRepository
{
    Task<RecipientMonitor> InsertRecipientMonitor(RecipientMonitor recipientMonitor);
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
        var newRecipient = new RecipientMonitor()
        {
            Name = recipientMonitor.Name,
            CreatedAt = _clock.Now.ToUniversalTime(),
            IsActive = true,
            MacAddress = recipientMonitor.MacAddress,
            RecipientBoundary = recipientMonitor.RecipientBoundary,
            WifiSsid = recipientMonitor.WifiSsid,
            WifiPassword = recipientMonitor.WifiPassword,
            RecipientType = recipientMonitor.RecipientType,
            RecipientMonitorId = RecipientMonitorId.New(),
        };

        _context.RecipientMonitors.Add(newRecipient);
        await _context.SaveChangesAsync();

        return newRecipient;
    }
}