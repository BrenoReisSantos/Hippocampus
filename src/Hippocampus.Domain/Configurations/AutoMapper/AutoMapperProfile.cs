using AutoMapper;
using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

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
                            RecipientBoundary = src.MonitorLinkedTo.RecipientBoundary,
                            RecipientMonitorId = src.MonitorLinkedTo.RecipientMonitorId,
                            Name = src.MonitorLinkedTo.Name,
                        }
                        : null));
        CreateMap<RecipientMonitorPostDto, RecipientMonitor>()
            .ForMember(
                dst => dst.RecipientBoundary,
                map =>
                    map.MapFrom(
                        src => new RecipientBoundary
                        {
                            MaxHeight = src.MaxHeight, MinHeight = src.MinHeight
                        }))
            .ForMember(dst => dst.IsActive,
                config =>
                    config.MapFrom(src => true))
            .ForMember(dst => dst.RecipientMonitorId,
                config =>
                    config.MapFrom(src => RecipientMonitorId.New()));
    }
}