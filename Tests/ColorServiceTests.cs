namespace Tests;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services;
using CapEnjoyer.DAL;
using CapEnjoyer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class ColorServiceTests : IDisposable
{
    private readonly CapEnjoyerDbContext context;

    public ColorServiceTests()
    {
        var options = new DbContextOptionsBuilder<CapEnjoyerDbContext>()
            .UseInMemoryDatabase("TestColorDatabase")
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
    public async Task GetColorByIdReturnsCorrectColor()
    {
        var colorService = new ColorService(this.context);

        var colorId = Guid.NewGuid();
        var color = new Color { Id = colorId, Name = "Blue", HexCode = "#0000FF" };

        this.context.Colors.Add(color);
        await this.context.SaveChangesAsync();

        var result = await colorService.GetColorByIdAsync(colorId);

        Assert.NotNull(result);
        Assert.Equal(colorId, result?.Id);
        Assert.Equal("Blue", result?.Name);
        Assert.Equal("#0000FF", result?.HexCode);
    }

    [Fact]
    public async Task GetColorsReturnsAllColors()
    {
        var colorService = new ColorService(this.context);

        this.context.Colors.AddRange(
            new Color { Id = Guid.NewGuid(), Name = "Blue", HexCode = "#0000FF" },
            new Color { Id = Guid.NewGuid(), Name = "Red", HexCode = "#FF0000" }
        );
        await this.context.SaveChangesAsync();

        var result = await colorService.GetColorsAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateColorAddsNewColor()
    {
        var colorService = new ColorService(this.context);

        var newColor = new ColorDto { Name = "Green", HexCode = "#00FF00" };

        var result = await colorService.CreateColorAsync(newColor);

        Assert.NotNull(result);
        Assert.Equal("Green", result.Name);
        Assert.Equal("#00FF00", result.HexCode);

        var createdColor = this.context.Colors.FirstOrDefault(c => c.Name == "Green");
        Assert.NotNull(createdColor);
        Assert.Equal("#00FF00", createdColor?.HexCode);
    }

    [Fact]
    public async Task DeleteColorRemovesColor()
    {
        var colorService = new ColorService(this.context);

        var colorId = Guid.NewGuid();
        var color = new Color { Id = colorId, Name = "Yellow", HexCode = "#FFFF00" };

        this.context.Colors.Add(color);
        await this.context.SaveChangesAsync();

        await colorService.DeleteColorAsync(colorId);

        var deletedColor = await this.context.Colors.FindAsync(colorId);
        Assert.Null(deletedColor);
    }

    [Fact]
    public async Task UpdateColorModifiesColorDetails()
    {
        var colorService = new ColorService(this.context);

        var colorId = Guid.NewGuid();
        var originalColor = new Color { Id = colorId, Name = "Orange", HexCode = "#FFA500" };

        this.context.Colors.Add(originalColor);
        await this.context.SaveChangesAsync();

        var updatedColorDto = new ColorDto { Name = "Dark Orange", HexCode = "#FF8C00" };

        var result = await colorService.UpdateColorAsync(colorId, updatedColorDto);

        Assert.NotNull(result);
        Assert.Equal("Dark Orange", result.Name);
        Assert.Equal("#FF8C00", result.HexCode);

        var updatedColor = await this.context.Colors.FindAsync(colorId);
        Assert.NotNull(updatedColor);
        Assert.Equal("Dark Orange", updatedColor?.Name);
        Assert.Equal("#FF8C00", updatedColor?.HexCode);
    }
}
