namespace Cap.Enjoyer.WebMVC.Mappers;

using System.Reflection;
using Cap.Enjoyer.WebMVC.Models;
using CapEnjoyer.BL.DTOs;
using Mapster;

public static class MapsterExtensions
{

    public static void ConfigureEditRequestMapping(this TypeAdapterConfig config)
    {
        config.NewConfig<EditRequestsInfoDto<CapWithDetailsDto>, EditRequestCapViewModel>()
            .Map(dest => dest.RequestCount, src => src.EditRequestCount)
            .Map(dest => dest.EditRequestCap, src => src.FirstEditRequest)
            .Map(dest => dest.OriginalCap, src => src.CurrentEntity);

        config.NewConfig<EditRequestsInfoDto<BottleWithDetailsDto>, EditRequestBottleViewModel>()
            .Map(dest => dest.RequestCount, src => src.EditRequestCount)
            .Map(dest => dest.EditRequestBottle, src => src.FirstEditRequest)
            .Map(dest => dest.OriginalBottle, src => src.CurrentEntity);

        config.NewConfig<EditRequestsInfoDto<ProducerWithDetailsDto>, EditRequestProducerViewModel>()
            .Map(dest => dest.RequestCount, src => src.EditRequestCount)
            .Map(dest => dest.EditRequestProducer, src => src.FirstEditRequest)
            .Map(dest => dest.OriginalProducer, src => src.CurrentEntity);

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}
