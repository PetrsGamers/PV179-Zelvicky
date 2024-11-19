namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface ICountryService
{
    Task<IEnumerable<CountryDto>> GetCountriesAsync();
    Task<CountryDto?> GetCountryByIdAsync(Guid id);
}
