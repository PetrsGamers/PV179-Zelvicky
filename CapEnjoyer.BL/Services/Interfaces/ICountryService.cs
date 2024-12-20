namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface ICountryService
{
    public Task<List<CountryDto>> GetCountriesAsync();
    public Task<CountryDto?> GetCountryByIdAsync(Guid id);
}
