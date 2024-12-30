namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

public interface IBottleService
{
    public Task<BottleDto> GetBottleByIdAsync(Guid id);
    public Task<BottleWithDetailsDto?> FindBottleWithDetailsByIdAsync(Guid id);
    public Task<IEnumerable<BottleDto>> GetAllBottlesAsync();
    public Task<BottleDto> CreateBottleAsync(BottleInsertDto bottle);
    public Task<BottleDto> UpdateBottleAsync(Guid id, BottleInsertDto bottle);
    public Task DeleteBottleAsync(Guid id);
    List<SelectListItem> GetDrinkTypeOptions();
    public Task<List<SelectListItem>> GetBottleOptionsAsync();

    public Task<IEnumerable<BottleDto>> GetBottlesbySearchFieldAsync(string searchField);
}
