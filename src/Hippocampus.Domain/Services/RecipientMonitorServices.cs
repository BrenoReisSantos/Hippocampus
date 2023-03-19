using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Repository;
using Hippocampus.Domain.Services.ApplicationValues;

namespace Hippocampus.Domain.Services;

public interface IRecipientMonitorServices
{
    Task<ServiceResult<RecipientMonitorCreatedDto>> InsertNewRecipientMonitor(RecipientMonitorPostDto monitor);
}

public class RecipientMonitorServices : IRecipientMonitorServices
{
    private readonly IRecipientMonitorRepository _monitorRepository;
    private readonly IClock _clock;

    public RecipientMonitorServices(IRecipientMonitorRepository monitorRepository, IClock clock)
    {
        _monitorRepository = monitorRepository;
        _clock = clock;
    }

    public async Task<ServiceResult<RecipientMonitorCreatedDto>> InsertNewRecipientMonitor(
        RecipientMonitorPostDto monitor)
    {
        if (monitor.MaxHeight < monitor.MinHeight)
            return ServiceResult<RecipientMonitorCreatedDto>.Error(
                "Altura máxima não pode ser menor que altura mínima");

        var monitorToInsert = new RecipientMonitor()
        {
            RecipientMonitorId = RecipientMonitorId.New(),
            Name = monitor.Name,
            CreatedAt = _clock.Now,
            RecipientType = monitor.RecipientType,
            WifiSsid = monitor.WifiSsid,
            WifiPassword = monitor.WifiPassword,
            MacAddress = monitor.MacAddress,
            RecipientBoundary = new RecipientBoundary()
            {
                MaxHeight = monitor.MaxHeight,
                MinHeight = monitor.MinHeight,
            },
            IsActive = true,
        };
        var newMonitor = await _monitorRepository.InsertRecipientMonitor(monitorToInsert);

        var recipientMonitorCreatedDto = new RecipientMonitorCreatedDto()
        {
            RecipientMonitorId = newMonitor.RecipientMonitorId,
            MacAddress = newMonitor.MacAddress,
            CreatedAt = newMonitor.CreatedAt,
            Name = newMonitor.Name,
            WifiSsid = newMonitor.WifiSsid,
            WifiPassword = newMonitor.WifiPassword,
            RecipientType = newMonitor.RecipientType,
            RecipientBoundary = new()
            {
                MaxHeight = newMonitor.RecipientBoundary.MaxHeight,
                MinHeight = newMonitor.RecipientBoundary.MinHeight,
            }
        };

        return ServiceResult<RecipientMonitorCreatedDto>.Success(recipientMonitorCreatedDto);
    }
}