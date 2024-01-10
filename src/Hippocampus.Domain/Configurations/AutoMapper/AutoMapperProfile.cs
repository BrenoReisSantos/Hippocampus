using AutoMapper;
using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Domain.Configurations.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RecipientMonitor, RecipientMonitorCreatedDto>()
            .ForMember(dst => dst.RecipientMonitorLinkedTo,
                map =>
                    map.MapFrom(src => src.MonitorLinkedTo != null
                        ? new RecipientMonitorLinkedToCreatedDto()
                        {
                            RecipientType = src.MonitorLinkedTo.RecipientType,
                            MacAddress = src.MonitorLinkedTo.MacAddress,
                            MaxHeight = src.MonitorLinkedTo.MaxHeight,
                            MinHeight = src.MonitorLinkedTo.MinHeight,
                            RecipientMonitorId = src.MonitorLinkedTo.RecipientMonitorId,
                            Name = src.MonitorLinkedTo.Name
                        }
                        : null));
        CreateMap<RecipientMonitor, RecipientMonitorUpdatedDto>()
            .ForMember(dst => dst.RecipientMonitorLinkedTo,
                map =>
                    map.MapFrom(src => src.MonitorLinkedTo != null
                        ? new RecipientMonitorLinkedToUpdatedDto()
                        {
                            RecipientType = src.MonitorLinkedTo.RecipientType,
                            MacAddress = src.MonitorLinkedTo.MacAddress,
                            MaxHeight = src.MonitorLinkedTo.MaxHeight,
                            MinHeight = src.MonitorLinkedTo.MinHeight,
                            RecipientMonitorId = src.MonitorLinkedTo.RecipientMonitorId,
                            Name = src.MonitorLinkedTo.Name
                        }
                        : null));
        CreateMap<RecipientMonitorPostDto, RecipientMonitor>()
            .ForMember(dst => dst.IsActive,
                config =>
                    config.MapFrom(src => true))
            .ForMember(dst => dst.RecipientMonitorId,
                config =>
                    config.MapFrom(src => RecipientMonitorId.New()));
        CreateMap<RecipientMonitorPutDto, RecipientMonitor>()
            .ForMember(dst => dst.IsActive,
                config =>
                    config.MapFrom(src => true))
            .ForMember(dst => dst.RecipientMonitorId,
                config =>
                    config.MapFrom(src => RecipientMonitorId.New()))
            .ForMember(dst => dst.RecipientMonitorId,
                config => config.MapFrom(
                    src => src.RecipientMonitorId));

        CreateMap<RecipientMonitor, RecipientMonitorForMonitorsTableDto>()
            .ForMember(
                dst => dst.LinkedRecipientMonitorMacAddress,
                map =>
                    map.MapFrom(src => src.MonitorLinkedTo != null
                        ? src.MonitorLinkedTo.MacAddress
                        : null))
            .ForMember(
                dst => dst.RecipientLevelPercentage,
                map => map.MapFrom(
                    src => src.RecipientLogs.Any()
                        ? src.RecipientLogs[0].LevelPercentage.Value
                        : (int?)null))
            .ForMember(
                dst => dst.RecipientState,
                map => map.MapFrom(
                    src =>
                        src.RecipientLogs.Any()
                            ? src.RecipientLogs[0].RecipientState
                            : (RecipientState?)null));

        CreateMap<RecipientMonitor, RecipientMonitorDto>()
            .ForMember(
                dst => dst.MonitorLinkedToMacAddress,
                map =>
                    map.MapFrom(src => src.MonitorLinkedTo != null
                        ? src.MonitorLinkedTo.MacAddress
                        : null));
    }
}