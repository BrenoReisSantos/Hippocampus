using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Hippocampus.Domain.Repository.Context;
using Hippocampus.Domain.Services.ApplicationValues;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Domain.Repository;

public interface IWaterTankLogRepository
{
    Task<WaterTankLog?> GetMostRecentRecipientLogAsync(WaterTankId waterTankId);
    Task<WaterTankLog> InsertRecipientLog(WaterTankLog waterTankLog);

    Task<IEnumerable<WaterTankLog>> GetLogsForMonitorInAGivenDateRangeAsync(WaterTankId monitorId,
        DateTime startDate, DateTime endDate);
}

public class WaterTankLogRepository : IWaterTankLogRepository
{
    private readonly HippocampusContext _context;
    private readonly IClock _clock;

    public WaterTankLogRepository(HippocampusContext context, IClock clock)
    {
        _context = context;
        _clock = clock;
    }

    public async Task<WaterTankLog?> GetMostRecentRecipientLogAsync(WaterTankId waterTankId)
    {
        return await _context.RecipientLogs.OrderByDescending(r => r.LogDate)
            .FirstOrDefaultAsync(r => r.WaterTankId == waterTankId);
    }

    public async Task<WaterTankLog> InsertRecipientLog(WaterTankLog waterTankLog)
    {
        var recipientToLogFor = await
            _context.RecipientMonitors.SingleOrDefaultAsync(
                r => r.WaterTankId == waterTankLog.WaterTank.WaterTankId);
        var recipientLogToInsert = new WaterTankLog
        {
            WaterTank = recipientToLogFor,
            LogDate = _clock.Now.ToUniversalTime(),
            WaterTankState = waterTankLog.WaterTankState,
            Level = waterTankLog.Level
        };

        _context.RecipientLogs.Add(recipientLogToInsert);

        await _context.SaveChangesAsync();

        return recipientLogToInsert;
    }

    public async Task<IEnumerable<WaterTankLog>> GetLogsForMonitorInAGivenDateRangeAsync(WaterTankId monitorId,
        DateTime startDate, DateTime endDate)
    {
        var startDateUtc = startDate.ToUniversalTime();
        var endDateUtc = endDate.ToUniversalTime();
        return await _context.RecipientLogs.Where(log =>
                log.WaterTankId == monitorId && log.LogDate >= startDateUtc &&
                log.LogDate <= endDateUtc)
            .ToListAsync();
    }
}