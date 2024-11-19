namespace CapEnjoyer.BL.Interfaces;

using DTOs;

public interface IAlbumService
{
    Task<IEnumerable<AlbumDto>> GetAllAlbums();
    Task<AlbumDto> GetAlbumById(Guid id);
    Task<AlbumDto> CreateAlbum(AlbumInsertDto album);
    Task<AlbumDto> UpdateAlbum(Guid id, AlbumInsertDto album);
    Task DeleteAlbum(Guid id);

    Task AddCapToAlbum(Guid albumId, Guid capId);
    Task RemoveCapFromAlbum(Guid albumId, Guid capId);
}
