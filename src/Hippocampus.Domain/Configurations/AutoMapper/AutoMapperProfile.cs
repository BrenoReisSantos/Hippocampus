using AutoMapper;
using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Domain.Configurations.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<WaterTank, WaterTankCreatedDto>()
            .ForMember(dst => dst.RecipientMonitorLinkedTo,
                map =>
                    map.MapFrom(src => src.PumpsTo != null
                        ? new RecipientMonitorLinkedToCreatedDto()
                        {
                            WaterTankType = src.PumpsTo.WaterTankType,
                            MaxHeight = src.PumpsTo.LevelWhenFull,
                            MinHeight = src.PumpsTo.LevelWhenEmpty,
                            WaterTankId = src.PumpsTo.WaterTankId,
                            Name = src.PumpsTo.Name
                        }
                        : null));
        CreateMap<WaterTank, WaterTankUpdatedDto>()
            .ForMember(dst => dst.WaterTankLinkedTo,
                map =>
                    map.MapFrom(src => src.PumpsTo != null
                        ? new RecipientMonitorLinkedToUpdatedDto()
                        {
                            WaterTankType = src.PumpsTo.WaterTankType,
                            MaxHeight = src.PumpsTo.LevelWhenFull,
                            MinHeight = src.PumpsTo.LevelWhenEmpty,
                            WaterTankId = src.PumpsTo.WaterTankId,
                            Name = src.PumpsTo.Name
                        }
                        : null));
        CreateMap<WaterTankCreateDto, WaterTank>()
            .ForMember(dst => dst.IsActive,
                config =>
                    config.MapFrom(src => true))
            .ForMember(dst => dst.WaterTankId,
                config =>
                    config.MapFrom(src => WaterTankId.New()));
        CreateMap<WaterTankUpdateDto, WaterTank>()
            .ForMember(dst => dst.IsActive,
                config =>
                    config.MapFrom(src => true))
            .ForMember(dst => dst.WaterTankId,
                config =>
                    config.MapFrom(src => WaterTankId.New()))
            .ForMember(dst => dst.WaterTankId,
                config => config.MapFrom(
                    src => src.WaterTankId));

        CreateMap<WaterTank, WaterTankForTableDto>()
            .ForMember(
                dst => dst.RecipientLevelPercentage,
                map => map.MapFrom(
                    src => src.WaterTankLogs.Any()
                        ? src.WaterTankLogs[0].Level
                        : (int?)null))
            .ForMember(
                dst => dst.RecipientState,
                map => map.MapFrom(
                    src =>
                        src.WaterTankLogs.Any()
                            ? src.WaterTankLogs[0].WaterTankState
                            : (WaterTankState?)null));

        CreateMap<WaterTank, WaterTankDto>();
    }
}