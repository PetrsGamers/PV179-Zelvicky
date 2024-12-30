namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

public interface IColorService
{
    public Task<IEnumerable<ColorDto>> GetColorsAsync();
    public Task<ColorDto?> GetColorByIdAsync(Guid id);
    public Task<ColorDto> CreateColorAsync(ColorInsertDto colorDto);
    public Task<ColorDto> UpdateColorAsync(Guid id, ColorInsertDto colorDto);
    public Task DeleteColorAsync(Guid id);
    public Task<List<SelectListItem>> GetColorOptionsAsync();
}
