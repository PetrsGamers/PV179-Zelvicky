namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface ICapService
{
    public Task UploadImageForCapAsync(Guid capId, IFormFile image);
    public Task<CapDto> GetCapByIdAsync(Guid id);
    public Task<CapDto> CreateCapAsync(CapInsertDto capInsertDto);
    public Task<IEnumerable<CapDto>> GetAllCapsByAlbumIdAsync(Guid albumId);

    public Task<IEnumerable<CapDto>> GetAllCapsFilteredAsync(
        string? textSubstring = null,
        List<Guid>? textColorIds = null,
        List<Guid>? bgColorIds = null,
        List<Guid>? producerIds = null,
        List<Guid>? countryIds = null);

    public Task<CapDto> UpdateCapAsync(Guid id, CapInsertDto capInsertDto);
    public Task DeleteCapAsync(Guid id);
}
