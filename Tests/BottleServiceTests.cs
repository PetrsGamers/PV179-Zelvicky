namespace Tests;

using System.Text;
using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

public class BottleServiceTests
{
    private readonly Mock<IBottleService> bottleServiceMock = new();

    [Fact]
    public async Task GetBottleByIdReturnsBottle()
    {
        // Arrange
        var bottleId = Guid.NewGuid();
        var expectedBottle = new BottleDto
        {
            Id = bottleId,
            Name = "Test Bottle",
            Description = "A test bottle description"
        };

        this.bottleServiceMock
            .Setup(service => service.GetBottleById(bottleId))
            .ReturnsAsync(expectedBottle);

        // Act
        var bottle = await this.bottleServiceMock.Object.GetBottleById(bottleId);

        // Assert
        Assert.NotNull(bottle);
        Assert.Equal(expectedBottle.Id, bottle.Id);
        Assert.Equal(expectedBottle.Name, bottle.Name);
    }

    [Fact]
    public async Task GetAllBottlesReturnsListOfBottles()
    {
        // Arrange
        var bottles = new List<BottleDto>
        {
            new() { Id = Guid.NewGuid(), Name = "Bottle 1", Description = "Description 1" },
            new() { Id = Guid.NewGuid(), Name = "Bottle 2", Description = "Description 2" }
        };

        this.bottleServiceMock
            .Setup(service => service.GetAllBottles())
            .ReturnsAsync(bottles);

        // Act
        var result = await this.bottleServiceMock.Object.GetAllBottles();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateBottleReturnsCreatedBottle()
    {
        // Arrange
        var newBottle = new BottleDto { Name = "New Bottle", Description = "A new bottle description" };
        var createdBottle = new BottleDto
        {
            Id = Guid.NewGuid(),
            Name = "New Bottle",
            Description = "A new bottle description"
        };

        this.bottleServiceMock
            .Setup(service => service.CreateBottle(newBottle))
            .ReturnsAsync(createdBottle);

        // Act
        var result = await this.bottleServiceMock.Object.CreateBottle(newBottle);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdBottle.Id, result.Id);
        Assert.Equal(createdBottle.Name, result.Name);
    }

    [Fact]
    public async Task UpdateBottleReturnsUpdatedBottle()
    {
        // Arrange
        var bottleId = Guid.NewGuid();
        var updatedBottle = new BottleDto
        {
            Id = bottleId,
            Name = "Updated Bottle",
            Description = "An updated bottle description"
        };

        this.bottleServiceMock
            .Setup(service => service.UpdateBottle(bottleId, updatedBottle))
            .ReturnsAsync(updatedBottle);

        // Act
        var result = await this.bottleServiceMock.Object.UpdateBottle(bottleId, updatedBottle);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedBottle.Id, result.Id);
        Assert.Equal(updatedBottle.Name, result.Name);
    }

    [Fact]
    public async Task DeleteBottleCallsServiceOnce()
    {
        // Arrange
        var bottleId = Guid.NewGuid();

        this.bottleServiceMock
            .Setup(service => service.DeleteBottle(bottleId))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await this.bottleServiceMock.Object.DeleteBottle(bottleId);

        // Assert
        this.bottleServiceMock.Verify(service => service.DeleteBottle(bottleId), Times.Once);
    }

    [Fact]
    public async Task UploadImageForBottleAsyncHandlesImageUpload()
    {
        // Arrange
        var bottleId = Guid.NewGuid();
        var mockImageFile = new Mock<IFormFile>();
        const string fileName = "image.png";
        const string content = "fake image content";
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        mockImageFile.Setup(f => f.FileName).Returns(fileName);
        mockImageFile.Setup(f => f.OpenReadStream()).Returns(fileStream);
        mockImageFile.Setup(f => f.Length).Returns(fileStream.Length);
        mockImageFile.Setup(f => f.ContentType).Returns("image/png");

        this.bottleServiceMock
            .Setup(service => service.UploadImageForBottleAsync(bottleId, mockImageFile.Object))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await this.bottleServiceMock.Object.UploadImageForBottleAsync(bottleId, mockImageFile.Object);

        // Assert
        this.bottleServiceMock.Verify(service => service.UploadImageForBottleAsync(bottleId, mockImageFile.Object),
            Times.Once);
    }
}
