namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface IColorService
{
    public Task<IEnumerable<ColorDto>> GetColorsAsync();
    public Task<ColorDto?> GetColorByIdAsync(Guid id);
    public Task<ColorDto> CreateColorAsync(ColorDto colorDto);
    public Task<ColorDto> UpdateColorAsync(Guid id, ColorDto colorDto);
    public Task DeleteColorAsync(Guid id);
}
