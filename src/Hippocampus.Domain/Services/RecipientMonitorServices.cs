using AutoMapper;
using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Repository;
using Hippocampus.Domain.Services.ApplicationValues;

namespace Hippocampus.Domain.Services;

public interface IRecipientMonitorServices
{
    Task<ServiceResult<RecipientMonitorCreatedDto>> InsertNewRecipientMonitor(RecipientMonitorPostDto monitor);
    Task<IEnumerable<RecipientMonitorForMonitorsTableDto>> GetRecipientMonitorsForMonitorsTable();
    Task<ServiceResult<RecipientMonitorUpdatedDto>> UpdateRecipientMonitor(RecipientMonitorPutDto monitor);
    Task<ServiceResult<RecipientMonitorDto>> GetRecipientMonitorById(RecipientMonitorId monitorId);
}

public class RecipientMonitorServices : IRecipientMonitorServices
{
    private readonly IRecipientMonitorRepository _monitorRepository;
    private readonly IRecipientLogRepository _recipientLogMonitor;
    private readonly IClock _clock;
    private readonly IMapper _mapper;

    public RecipientMonitorServices(IRecipientMonitorRepository monitorRepository,
        IRecipientLogRepository recipientLogMonitor, IClock clock, IMapper mapper)
    {
        _monitorRepository = monitorRepository;
        _recipientLogMonitor = recipientLogMonitor;
        _clock = clock;
        _mapper = mapper;
    }

    public async Task<ServiceResult<RecipientMonitorCreatedDto>> InsertNewRecipientMonitor(
        RecipientMonitorPostDto monitor)
    {
        if (monitor.MaxHeight < monitor.MinHeight)
            return ServiceResult<RecipientMonitorCreatedDto>.Error(
                "Altura máxima não pode ser menor que altura mínima");

        RecipientMonitor? monitorLinkedTo =
            await _monitorRepository.GetRecipientMonitorWithMonitorLinkedToByMacAddress(monitor
                .RecipientMonitorLinkedToMacAddress);

        if (monitorLinkedTo?.MonitorLinkedTo is not null)
            return ServiceResult<RecipientMonitorCreatedDto>.Error(
                $"O monitor a se conectar já está conectado com um outro. ({monitorLinkedTo.MonitorLinkedTo.Name} with Macaddress {monitorLinkedTo.MonitorLinkedTo.MacAddress})");

        if (monitor.RecipientMonitorLinkedToMacAddress is not null && monitorLinkedTo is null)
            return ServiceResult<RecipientMonitorCreatedDto>.Error("Monitor Relacionado não encontrado");

        if (monitor.RecipientType == monitorLinkedTo?.RecipientType)
            return ServiceResult<RecipientMonitorCreatedDto>.Error(
                "o Monitor cadastrado sendo cadastrado não pode pertencer ao mesmo tipo de recipient que o monitor conectado");

        var monitorToInsert = _mapper.Map<RecipientMonitor>(monitor);
        if (monitorLinkedTo is not null) monitorToInsert.MonitorLinkedTo = monitorLinkedTo;

        var newMonitor = await _monitorRepository.InsertRecipientMonitor(monitorToInsert);

        var recipientMonitorCreatedDto = _mapper.Map<RecipientMonitorCreatedDto>(newMonitor);

        return ServiceResult<RecipientMonitorCreatedDto>.Success(recipientMonitorCreatedDto);
    }

    public async Task<IEnumerable<RecipientMonitorForMonitorsTableDto>> GetRecipientMonitorsForMonitorsTable()
    {
        var monitors = await _monitorRepository.GetAllRecipientMonitorsWithLinkedMonitor();
        return _mapper.Map<IEnumerable<RecipientMonitorForMonitorsTableDto>>(monitors);
    }

    public async Task<ServiceResult<RecipientMonitorUpdatedDto>> UpdateRecipientMonitor(RecipientMonitorPutDto monitor)
    {
        var existsMonitor = await _monitorRepository.ExistsMonitor(monitor.RecipientMonitorId);

        if (!existsMonitor)
            return ServiceResult<RecipientMonitorUpdatedDto>.Error(
                $"Monitor de ID {monitor.RecipientMonitorId} não encontrado");

        if (monitor.MaxHeight < monitor.MinHeight)
            return ServiceResult<RecipientMonitorUpdatedDto>.Error(
                "Altura máxima não pode ser menor que altura mínima");

        RecipientMonitor? monitorLinkedTo =
            await _monitorRepository.GetRecipientMonitorWithMonitorLinkedToByMacAddress(monitor
                .RecipientMonitorLinkedToMacAddress);

        if (monitorLinkedTo?.MonitorLinkedTo is not null)
            return ServiceResult<RecipientMonitorUpdatedDto>.Error(
                $"O monitor a se conectar já está conectado com um outro. ({monitorLinkedTo.MonitorLinkedTo.Name} with Macaddress {monitorLinkedTo.MonitorLinkedTo.MacAddress})");

        if (monitor.RecipientMonitorLinkedToMacAddress is not null && monitorLinkedTo is null)
            return ServiceResult<RecipientMonitorUpdatedDto>.Error("Monitor Relacionado não encontrado");

        if (monitor.RecipientType == monitorLinkedTo?.RecipientType)
            return ServiceResult<RecipientMonitorUpdatedDto>.Error(
                "o Monitor cadastrado sendo cadastrado não pode pertencer ao mesmo tipo de recipient que o monitor conectado");

        var monitorToUpdate = _mapper.Map<RecipientMonitor>(monitor);
        if (monitorLinkedTo is not null) monitorToUpdate.MonitorLinkedTo = monitorLinkedTo;

        var updatedMonitor = await _monitorRepository.UpdateRecipientMonitor(monitorToUpdate);

        var recipientMonitorCreatedDto = _mapper.Map<RecipientMonitorUpdatedDto>(updatedMonitor);

        return ServiceResult<RecipientMonitorUpdatedDto>.Success(recipientMonitorCreatedDto);
    }

    public async Task<ServiceResult<RecipientMonitorDto>> GetRecipientMonitorById(RecipientMonitorId recipientMonitorId)
    {
        var monitor = await _monitorRepository.GetRecipientMonitorWithMonitorLinkedToById(recipientMonitorId);
        var mappedMonitor = _mapper.Map<RecipientMonitorDto>(monitor);
        return ServiceResult<RecipientMonitorDto>.Success(mappedMonitor);
    }
}