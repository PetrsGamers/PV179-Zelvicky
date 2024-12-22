namespace CapEnjoyer.BL.Mappers;

using System.Reflection;
using DAL.Entities;
using DTOs;
using Mapster;

public static class MapsterExtensions
{
    public static void ConfigureAlbumMapping(this TypeAdapterConfig config)
    {
        // Mapping from Album to AlbumDto
        config.NewConfig<Album, AlbumDto>()
        .Map(dest => dest.Caps, src => src.CapLinks.Select(cl => cl.CapId).ToList());

        // Mapping from Bottle to BottleDto
        config.NewConfig<Bottle, BottleDto>().Map(dest => dest.Caps, src => src.CapLinks.Select(cl => cl.CapId).ToList());

        // Mapping from Cap to CapDto
        config.NewConfig<Cap, CapDto>()
            .Map(dest => dest.TextColors, src => src.TextColorLinks.Select(tcl => tcl.TextColorId).ToList())
            .Map(dest => dest.BgColors, src => src.BackgroundColorLinks.Select(bcl => bcl.BackgroundColorId).ToList())
            .Map(dest => dest.Bottles, src => src.BottleLinks.Select(bl => bl.BottleId).ToList());

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

    }
}
