using AutoMapper;
using Hippocampus.Domain.Diplomat.HttpIn;
using Hippocampus.Domain.Diplomat.HttpOut;
using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Domain.Configurations.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RecipientMonitor, RecipientMonitorCreatedDto>();
        CreateMap<RecipientMonitor, RecipientMonitorLinkedToCreatedDto>();
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