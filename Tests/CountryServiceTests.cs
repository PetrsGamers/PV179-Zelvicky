namespace Tests;

using CapEnjoyer.BL.Services;
using CapEnjoyer.DAL;
using CapEnjoyer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class CountryServiceTests : IDisposable
{
    private readonly CapEnjoyerDbContext context;

    public CountryServiceTests()
    {
        var options = new DbContextOptionsBuilder<CapEnjoyerDbContext>()
            .UseInMemoryDatabase("TestCountryDatabase")
            .Options;

        context = new CapEnjoyerDbContext(options);
    }

    public void Dispose()
    {
        context.Database.EnsureDeleted();
        context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task GetCountryByIdReturnsCorrectCountry()
    {
        var countryService = new CountryService(context);

        var countryId = Guid.NewGuid();
        var country = new Country { Id = countryId, Name = "Czech Republic" };

        context.Countries.Add(country);
        await context.SaveChangesAsync();

        var result = await countryService.GetCountryByIdAsync(countryId);

        Assert.NotNull(result);
        Assert.Equal(countryId, result?.Id);
        Assert.Equal("Czech Republic", result?.Name);
    }

    [Fact]
    public async Task GetCountriesReturnsAllCountries()
    {
        var countryService = new CountryService(context);

        context.Countries.AddRange(
            new Country { Id = Guid.NewGuid(), Name = "Czech Republic" },
            new Country { Id = Guid.NewGuid(), Name = "Slovakia" }
        );
        await context.SaveChangesAsync();

        var result = await countryService.GetCountriesAsync();

        Assert.Equal(2, result.Count);
    }
}
