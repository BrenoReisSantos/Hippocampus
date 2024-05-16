using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Hippocampus.Domain.Repository.Context;
using Hippocampus.Domain.Services.ApplicationValues;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Domain.Repository;

public interface IRecipientLogRepository
{
    Task<RecipientLog?> GetMostRecentRecipientLogAsync(RecipientMonitorId recipientMonitorId);
    Task<RecipientLog> InsertRecipientLog(RecipientLog recipientLog);

    Task<IEnumerable<RecipientLog>> GetLogsForMonitorInAGivenDateRangeAsync(RecipientMonitorId monitorId,
        DateTime startDate, DateTime endDate);
}

public class RecipientLogRepository : IRecipientLogRepository
{
    private readonly HippocampusContext _context;
    private readonly IClock _clock;

    public RecipientLogRepository(HippocampusContext context, IClock clock)
    {
        _context = context;
        _clock = clock;
    }

    public async Task<RecipientLog?> GetMostRecentRecipientLogAsync(RecipientMonitorId recipientMonitorId)
    {
        return await _context.RecipientLogs.OrderByDescending(r => r.RegisterDate)
            .FirstOrDefaultAsync(r => r.RecipientMonitorId == recipientMonitorId);
    }

    public async Task<RecipientLog> InsertRecipientLog(RecipientLog recipientLog)
    {
        var recipientToLogFor = await
            _context.RecipientMonitors.SingleOrDefaultAsync(
                r => r.RecipientMonitorId == recipientLog.RecipientMonitor.RecipientMonitorId);
        var recipientLogToInsert = new RecipientLog
        {
            RecipientMonitor = recipientToLogFor,
            RegisterDate = _clock.Now.ToUniversalTime(),
            RecipientState = recipientLog.RecipientState,
            LevelHeight = recipientLog.LevelHeight
        };

        _context.RecipientLogs.Add(recipientLogToInsert);

        await _context.SaveChangesAsync();

        return recipientLogToInsert;
    }

    public async Task<IEnumerable<RecipientLog>> GetLogsForMonitorInAGivenDateRangeAsync(RecipientMonitorId monitorId,
        DateTime startDate, DateTime endDate)
    {
        var startDateUtc = startDate.ToUniversalTime();
        var endDateUtc = endDate.ToUniversalTime();
        return await _context.RecipientLogs.Where(log =>
                log.RecipientMonitorId == monitorId && log.RegisterDate >= startDateUtc &&
                log.RegisterDate <= endDateUtc)
            .ToListAsync();
    }
}