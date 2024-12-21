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
            .Map(dest => dest.User, src => src.UserId)
            .Map(dest => dest.Caps, src => src.CapLinks.Select(link => link.CapId).ToList());

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

    }
}
