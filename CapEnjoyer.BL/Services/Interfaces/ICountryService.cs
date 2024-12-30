namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

public interface ICountryService
{
    public Task<List<CountryDto>> GetCountriesAsync();
    public Task<List<SelectListItem>> GetCountryOptionsAsync();
    public Task<CountryDto?> GetCountryByIdAsync(Guid id);
}
