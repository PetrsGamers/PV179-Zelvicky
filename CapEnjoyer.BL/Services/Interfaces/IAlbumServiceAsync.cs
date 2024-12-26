namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface IAlbumServiceAsync
{
    public Task<IEnumerable<AlbumDto>> GetAllAlbums();
    public Task<AlbumDto> GetAlbumById(Guid id);
    public Task<AlbumDto> CreateAlbum(AlbumInsertDto album);
    public Task<AlbumDto> UpdateAlbum(Guid id, AlbumInsertDto album);
    public Task DeleteAlbum(Guid id);

    public Task AddCapToAlbum(Guid albumId, Guid capId);
    public Task RemoveCapFromAlbum(Guid albumId, Guid capId);
}
