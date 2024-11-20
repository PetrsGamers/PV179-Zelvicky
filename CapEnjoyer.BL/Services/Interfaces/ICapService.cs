namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface ICapService
{
    Task UploadImageForCapAsync(Guid capId, IFormFile image);
    Task<CapDto> GetCapByIdAsync(Guid id);
    Task<CapDto> CreateCapAsync(CapInsertDto capInsertDto);
    Task<IEnumerable<CapDto>> GetAllCapsByAlbumIdAsync(Guid albumId);

    Task<IEnumerable<CapDto>> GetAllCapsFilteredAsync(
        string? textSubstring = null,
        List<Guid>? textColorIds = null,
        List<Guid>? bgColorIds = null,
        List<Guid>? producerIds = null,
        List<Guid>? countryIds = null);

    Task<CapDto> UpdateCapAsync(Guid id, CapInsertDto capInsertDto);
    Task DeleteCapAsync(Guid id);
}
