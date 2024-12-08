namespace Tests;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services;
using CapEnjoyer.DAL;
using CapEnjoyer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class CapServiceTests : IDisposable
{
    private readonly CapEnjoyerDbContext context;

    public CapServiceTests()
    {
        var options = new DbContextOptionsBuilder<CapEnjoyerDbContext>()
            .UseInMemoryDatabase("TestCapDatabase")
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
    public async Task GetCapByIdReturnsCorrectCap()
    {
        var capService = new CapService(context);

        var capId = Guid.NewGuid();
        var cap = new Cap { Id = capId, TextOnCap = "Cool Cap", Description = "A really cool cap", CapPicture = "" };

        context.Caps.Add(cap);
        await context.SaveChangesAsync();

        var result = await capService.GetCapByIdAsync(capId);

        Assert.NotNull(result);
        Assert.Equal(capId, result?.Id);
        Assert.Equal("Cool Cap", result?.TextOnCap);
        Assert.Equal("A really cool cap", result?.Description);
    }

    [Fact]
    public async Task CreateCapAddsNewCap()
    {
        var capService = new CapService(context);

        var newCap = new CapInsertDto
        {
            TextOnCap = "Awesome Cap",
            Description = "An awesome cap with amazing design",
            CapPicture = "",
            TextColors = [],
            BgColors = [],
            Bottles = []
        };

        var result = await capService.CreateCapAsync(newCap);

        Assert.NotNull(result);
        Assert.Equal("Awesome Cap", result.TextOnCap);
        Assert.Equal("An awesome cap with amazing design", result.Description);

        var createdCap = context.Caps.FirstOrDefault(c => c.TextOnCap == "Awesome Cap");
        Assert.NotNull(createdCap);
        Assert.Equal("An awesome cap with amazing design", createdCap?.Description);
    }

    [Fact]
    public async Task DeleteCapRemovesCap()
    {
        var capService = new CapService(context);

        var capId = Guid.NewGuid();

        var cap = new Cap
        {
            Id = capId,
            TextOnCap = "Delete Me",
            Description = "This cap is to be deleted",
            CapPicture = "",
            TextColorLinks = [],
            BackgroundColorLinks = [],
            BottleLinks = [],
            AlbumLinks = [],
            Edits = []
        };

        context.Caps.Add(cap);
        await context.SaveChangesAsync();

        await capService.DeleteCapAsync(capId);

        var deletedCap = await context.Caps.FindAsync(capId);
        Assert.Null(deletedCap);
    }

    [Fact]
    public async Task UpdateCapModifiesCapDetails()
    {
        var capService = new CapService(context);

        var capId = new Guid("8D47DAEC-E8AF-49B9-BBFE-18A438E8D705");
        var originalCap = new Cap
        {
            Id = capId,
            TextOnCap = "Old cap",
            Description = "This cap is an old cap",
            CapPicture = "",
            TextColorLinks = [],
            BackgroundColorLinks = [],
            BottleLinks = [],
            AlbumLinks = [],
            Edits = []
        };

        context.Caps.Add(originalCap);
        await context.SaveChangesAsync();

        var updatedCapDto = new CapInsertDto
        {
            TextOnCap = "Updated Cap",
            Description = "This cap has been updated",
            CapPicture = "",
            TextColors = [],
            BgColors = [],
            Bottles = []
        };

        var result = await capService.UpdateCapAsync(capId, updatedCapDto);

        Assert.NotNull(result);
        Assert.Equal("Updated Cap", result.TextOnCap);
        Assert.Equal("This cap has been updated", result.Description);

        var updatedCap = await context.Caps.FindAsync(capId);
        Assert.NotNull(updatedCap);
        Assert.Equal("Updated Cap", updatedCap?.TextOnCap);
        Assert.Equal("This cap has been updated", updatedCap?.Description);
    }
}
