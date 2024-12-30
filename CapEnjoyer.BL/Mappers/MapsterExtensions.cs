namespace CapEnjoyer.BL.Mappers;

using System.Reflection;
using DAL.Entities;
using DTOs;
using Mapster;

public static class MapsterExtensions
{
    public static void ConfigureAlbumMapping(this TypeAdapterConfig config)
    {
        config.NewConfig<Album, AlbumDto>()
            .Map(dest => dest.Caps, src => src.CapLinks.Select(cl => cl.CapId).ToList());

        config.NewConfig<Bottle, BottleDto>()
            .Map(dest => dest.CapIds, src => src.CapLinks.Select(cl => cl.CapId).ToList());

        config.NewConfig<Cap, CapDto>()
            .Map(dest => dest.TextColorIds, src => src.TextColorLinks.Select(tcl => tcl.TextColorId).ToList())
            .Map(dest => dest.BgColorIds, src => src.BackgroundColorLinks.Select(bcl => bcl.BackgroundColorId).ToList())
            .Map(dest => dest.BottleIds, src => src.BottleLinks.Select(bl => bl.BottleId).ToList());

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }

    public static void ConfigureProducerMapping(this TypeAdapterConfig config)
    {
        config.NewConfig<Producer, ProducerWithDetailsDto>()
            .Map(dest => dest.Country, src => src.Country.Name);

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
    public static void ConfigureBottleMapping(this TypeAdapterConfig config)
    {
        config.NewConfig<Bottle, BottleDto>()
            .Map(dest => dest.DrinkType, src => src.DrinkType.ToString())
            .Map(dest => dest.CapIds, src => src.CapLinks.Select(cl => cl.CapId).ToList());

        config.NewConfig<Bottle, BottleWithDetailsDto>()
            .Map(dest => dest.DrinkType, src => src.DrinkType.ToString())
            .Map(dest => dest.Producer, src => src.Adapt<ProducerDto>())
            .Map(dest => dest.CapDetails, src => src.CapLinks.Select(cl => cl.Cap.Adapt<CapDto>()).ToList());

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
    public static void ConfigureCapMapping(this TypeAdapterConfig config)
    {

        config.NewConfig<Cap, CapDto>().Map(dest => dest.TextColorIds,
                src => src.TextColorLinks.Select(tcl => tcl.TextColorId).ToList())
            .Map(dest => dest.BgColorIds,
                src => src.BackgroundColorLinks.Select(bcl => bcl.BackgroundColorId).ToList())
            .Map(dest => dest.BottleIds,
                src => src.BottleLinks.Select(bl => bl.BottleId).ToList());

        config.NewConfig<Cap, CapWithDetailsDto>()
            .Map(dest => dest.Bottles, src => src.BottleLinks.Select(bl => bl.Bottle.Adapt<BottleDto>()).ToList())
            .Map(dest => dest.TextColors, src => src.TextColorLinks.Select(tcl => tcl.TextColor.Adapt<ColorDto>()).ToList())
            .Map(dest => dest.BgColors, src => src.BackgroundColorLinks.Select(bcl => bcl.BackgroundColor.Adapt<ColorDto>()).ToList());



        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}
