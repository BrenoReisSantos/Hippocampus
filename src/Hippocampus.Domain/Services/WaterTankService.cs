using AutoMapper;
using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Dto;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Repository;
using Hippocampus.Domain.Services.ApplicationValues;

namespace Hippocampus.Domain.Services;

public interface IWaterTankService
{
    Task<ServiceResult<WaterTankCreatedDto>> Create(WaterTankCreateDto waterTank);
    Task<IEnumerable<WaterTankGetDto>> GetAll();
    Task<ServiceResult<WaterTankUpdatedDto>> Update(WaterTankUpdateDto waterTank);
    Task<ServiceResult<WaterTankDto>> Get(WaterTankId monitorId);

    Task<ServiceResult<IEnumerable<WaterTankLog>>> GetLogsForDateRange(
        WaterTankId monitorId,
        DateTime? startDate,
        DateTime? endDate
    );
}

public class WaterTankService(
    IWaterTankRepository waterTankRepository,
    IWaterTankLogRepository waterTankLogMonitor,
    IClock clock,
    IMapper mapper
) : IWaterTankService
{
    private readonly IClock _clock = clock;

    public async Task<ServiceResult<WaterTankCreatedDto>> Create(WaterTankCreateDto waterTank)
    {
        if (waterTank.LevelWhenFull < waterTank.LevelWhenEmpty)
            return ServiceResult<WaterTankCreatedDto>.Error(
                "Altura máxima não pode ser menor que altura mínima"
            );

        var waterTankLinkedTo = waterTank.PumpsToId is null
            ? null
            : await waterTankRepository.Get(waterTank.PumpsToId.Value);
        if (waterTankLinkedTo is not null)
        {
            if (waterTankLinkedTo.PumpsTo is not null)
                return ServiceResult<WaterTankCreatedDto>.Error(
                    $"O Reservatório a se conectar já está conectado com um outro reservatório. (Nome: {waterTankLinkedTo.PumpsTo.Name})"
                );
            // if (waterTank.PumpsToId is not null)
            //     return ServiceResult<WaterTankCreatedDto>.Error(
            //         "Reservatório relacionado não encontrado"
            //     );
        }

        var waterTankCreating = mapper.Map<WaterTank>(waterTank);
        if (waterTankLinkedTo is not null)
            waterTankCreating = waterTankCreating with { PumpsTo = waterTankLinkedTo };

        var newMonitor = await waterTankRepository.Insert(waterTankCreating);

        var recipientMonitorCreatedDto = mapper.Map<WaterTankCreatedDto>(newMonitor);

        return ServiceResult<WaterTankCreatedDto>.Success(recipientMonitorCreatedDto);
    }

    public async Task<IEnumerable<WaterTankGetDto>> GetAll()
    {
        var waterTank =  await waterTankRepository.GetAll();
        return mapper.Map<IEnumerable<WaterTankGetDto>>(waterTank);
    }

    public async Task<ServiceResult<WaterTankUpdatedDto>> Update(WaterTankUpdateDto waterTank)
    {
        var existsWaterTank = await waterTankRepository.Exists(waterTank.WaterTankId);

        if (!existsWaterTank)
            return ServiceResult<WaterTankUpdatedDto>.Error(
                $"Monitor de ID {waterTank.WaterTankId} não encontrado"
            );

        if (waterTank.LevelWhenFull < waterTank.LevelWhenEmpty)
            return ServiceResult<WaterTankUpdatedDto>.Error(
                "Altura máxima não pode ser menor que altura mínima"
            );

        var waterTankLinkedTo = waterTank.PumpsToId is null
            ? null
            : await waterTankRepository.Get(waterTank.PumpsToId.Value);
        if (waterTankLinkedTo is not null)
        {
            if (waterTankLinkedTo?.PumpsTo is not null)
                return ServiceResult<WaterTankUpdatedDto>.Error(
                    $"O Reservatório a se conectar já está conectado com um outro reservatório. (Nome: {waterTankLinkedTo.PumpsTo.Name})"
                );
            if (waterTank.PumpsToId is not null && waterTankLinkedTo is null)
                return ServiceResult<WaterTankUpdatedDto>.Error(
                    "Reservatório relacionado não encontrado"
                );
        }

        var waterTankUpdating = await waterTankRepository.Get(waterTank.WaterTankId);
        if (waterTankUpdating is null)
            return ServiceResult<WaterTankUpdatedDto>.Error(
                $"Monitor de ID {waterTank.WaterTankId} não encontrado"
            );

        if (waterTankLinkedTo is not null)
            waterTankUpdating = waterTankUpdating with { PumpsTo = waterTankLinkedTo };

        var updatedMonitor = await waterTankRepository.Update(waterTankUpdating);

        var recipientMonitorCreatedDto = mapper.Map<WaterTankUpdatedDto>(updatedMonitor);

        return ServiceResult<WaterTankUpdatedDto>.Success(recipientMonitorCreatedDto);
    }

    public async Task<ServiceResult<WaterTankDto>> Get(WaterTankId waterTankId)
    {
        var monitor = await waterTankRepository.Get(waterTankId);
        var mappedMonitor = mapper.Map<WaterTankDto>(monitor);
        return ServiceResult<WaterTankDto>.Success(mappedMonitor);
    }

    public async Task<ServiceResult<IEnumerable<WaterTankLog>>> GetLogsForDateRange(
        WaterTankId monitorId,
        DateTime? startDate,
        DateTime? endDate
    )
    {
        var startDateTreated = startDate ?? DateTime.UtcNow.AddDays(-30);
        var endDateTreated = endDate ?? DateTime.UtcNow;

        var monitorExists = await waterTankRepository.Exists(monitorId);

        if (!monitorExists)
            return ServiceResult<IEnumerable<WaterTankLog>>.Error("Monitor não existe");

        var logs = await waterTankLogMonitor.GetLogsForMonitorInAGivenDateRangeAsync(
            monitorId,
            startDateTreated,
            endDateTreated
        );
        return ServiceResult<IEnumerable<WaterTankLog>>.Success(logs);
    }
}
