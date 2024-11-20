namespace Tests;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services;
using CapEnjoyer.DAL;
using CapEnjoyer.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

public class CapServiceTests : IDisposable
{
    private readonly CapEnjoyerDbContext context;

    public CapServiceTests()
    {
        var options = new DbContextOptionsBuilder<CapEnjoyerDbContext>()
            .UseInMemoryDatabase("TestCapDatabase")
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
    public async Task GetCapByIdReturnsCorrectCap()
    {
        var capService = new CapService(this.context);

        var capId = Guid.NewGuid();
        var cap = new Cap { Id = capId, TextOnCap = "Cool Cap", Description = "A really cool cap", CapPicture = "" };

        this.context.Caps.Add(cap);
        await this.context.SaveChangesAsync();

        var result = await capService.GetCapByIdAsync(capId);

        Assert.NotNull(result);
        Assert.Equal(capId, result?.Id);
        Assert.Equal("Cool Cap", result?.TextOnCap);
        Assert.Equal("A really cool cap", result?.Description);
    }

    [Fact]
    public async Task CreateCapAddsNewCap()
    {
        var capService = new CapService(this.context);

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

        var createdCap = this.context.Caps.FirstOrDefault(c => c.TextOnCap == "Awesome Cap");
        Assert.NotNull(createdCap);
        Assert.Equal("An awesome cap with amazing design", createdCap?.Description);
    }

    [Fact]
    public async Task DeleteCapRemovesCap()
    {
        var capService = new CapService(this.context);

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

        this.context.Caps.Add(cap);
        await this.context.SaveChangesAsync();

        await capService.DeleteCapAsync(capId);

        var deletedCap = await this.context.Caps.FindAsync(capId);
        Assert.Null(deletedCap);
    }

    [Fact]
    public async Task UpdateCapModifiesCapDetails()
    {
        var capService = new CapService(this.context);

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

        this.context.Caps.Add(originalCap);
        await this.context.SaveChangesAsync();

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

        var updatedCap = await this.context.Caps.FindAsync(capId);
        Assert.NotNull(updatedCap);
        Assert.Equal("Updated Cap", updatedCap?.TextOnCap);
        Assert.Equal("This cap has been updated", updatedCap?.Description);
    }

    [Fact]
    public async Task UploadImageForCapStoresImage()
    {
        var capService = new CapService(this.context);

        var capId = new Guid("3DB206E7-AAF1-4250-9B0B-B7E3E9629037");
        var cap = new Cap
        {
            Id = capId,
            TextOnCap = "Image cap",
            Description = "asdasd",
            CapPicture = "",
            TextColorLinks = [],
            BackgroundColorLinks = [],
            BottleLinks = [],
            AlbumLinks = [],
            Edits = []
        };

        this.context.Caps.Add(cap);
        await this.context.SaveChangesAsync();

        var mockFile = new Mock<IFormFile>();
        var content = "image content";
        var fileName = "image.png";
        // add image type as image/png


        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        mockFile.Setup(_ => _.ContentType).Returns("image/png");
        mockFile.Setup(_ => _.OpenReadStream()).Returns(stream);
        mockFile.Setup(_ => _.FileName).Returns(fileName);
        mockFile.Setup(_ => _.Length).Returns(stream.Length);

        await capService.UploadImageForCapAsync(capId, mockFile.Object);

        // Assume the image is stored in the cap's `CapPicture` or similar property
        var updatedCap = await this.context.Caps.FindAsync(capId);
        Assert.NotNull(updatedCap);
        Assert.NotEqual("", cap.CapPicture); // Replace with your actual property name
    }
}
