namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface IColorService
{
    Task<IEnumerable<ColorDto>> GetColorsAsync();
    Task<ColorDto?> GetColorByIdAsync(Guid id);
    Task<ColorDto> CreateColorAsync(ColorDto colorDto);
    Task<ColorDto> UpdateColorAsync(Guid id, ColorDto colorDto);
    Task DeleteColorAsync(Guid id);
}
