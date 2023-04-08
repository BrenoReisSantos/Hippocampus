using AutoMapper;
using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
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
    private readonly IMapper _mapper;

    public RecipientMonitorServices(IRecipientMonitorRepository monitorRepository, IClock clock, IMapper mapper)
    {
        _monitorRepository = monitorRepository;
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
}