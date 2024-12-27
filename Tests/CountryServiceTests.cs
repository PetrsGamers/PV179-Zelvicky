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

        this.context = new CapEnjoyerDbContext(options);
    }

    public void Dispose()
    {
        this.context.Database.EnsureDeleted();
        this.context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task GetCountryByIdReturnsCorrectCountry()
    {
        var countryService = new CountryService(this.context);

        var countryId = Guid.NewGuid();
        var country = new Country { Id = countryId, Name = "Czech Republic" };

        this.context.Countries.Add(country);
        await this.context.SaveChangesAsync();

        var result = await countryService.GetCountryByIdAsync(countryId);

        Assert.NotNull(result);
        Assert.Equal(countryId, result?.Id);
        Assert.Equal("Czech Republic", result?.Name);
    }

    [Fact]
    public async Task GetCountriesReturnsAllCountries()
    {
        var countryService = new CountryService(this.context);

        this.context.Countries.AddRange(
            new Country { Id = Guid.NewGuid(), Name = "Czech Republic" },
            new Country { Id = Guid.NewGuid(), Name = "Slovakia" }
        );
        await this.context.SaveChangesAsync();

        var result = await countryService.GetCountriesAsync();

        Assert.Equal(2, result.Count);
    }
}
