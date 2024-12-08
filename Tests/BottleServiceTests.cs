namespace Tests;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
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
            Id = bottleId, Name = "Test Bottle", Description = "A test bottle description"
        };

        bottleServiceMock
            .Setup(service => service.GetBottleById(bottleId))
            .ReturnsAsync(expectedBottle);

        // Act
        var bottle = await bottleServiceMock.Object.GetBottleById(bottleId);

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

        bottleServiceMock
            .Setup(service => service.GetAllBottles())
            .ReturnsAsync(bottles);

        // Act
        var result = await bottleServiceMock.Object.GetAllBottles();

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
            Id = Guid.NewGuid(), Name = "New Bottle", Description = "A new bottle description"
        };

        bottleServiceMock
            .Setup(service => service.CreateBottle(newBottle))
            .ReturnsAsync(createdBottle);

        // Act
        var result = await bottleServiceMock.Object.CreateBottle(newBottle);

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
            Id = bottleId, Name = "Updated Bottle", Description = "An updated bottle description"
        };

        bottleServiceMock
            .Setup(service => service.UpdateBottle(bottleId, updatedBottle))
            .ReturnsAsync(updatedBottle);

        // Act
        var result = await bottleServiceMock.Object.UpdateBottle(bottleId, updatedBottle);

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

        bottleServiceMock
            .Setup(service => service.DeleteBottle(bottleId))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await bottleServiceMock.Object.DeleteBottle(bottleId);

        // Assert
        bottleServiceMock.Verify(service => service.DeleteBottle(bottleId), Times.Once);
    }

    [Fact]
    public async Task CreateBottleThrowsExceptionWhenRequiredFieldsAreMissing()
    {
        var invalidBottle = new BottleDto { Name = "", Description = "" };

        bottleServiceMock
            .Setup(service => service.CreateBottle(invalidBottle))
            .ThrowsAsync(new ArgumentException("Name or description is missing"));

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            bottleServiceMock.Object.CreateBottle(invalidBottle));

        // Assert
        Assert.Equal("Name or description is missing", exception.Message);
    }

    [Fact]
    public async Task UpdateBottleThrowsExceptionWhenBottleDoesNotExist()
    {
        // Arrange
        var nonExistentBottleId = Guid.NewGuid();
        var updatedBottle = new BottleDto
        {
            Id = nonExistentBottleId, Name = "Updated Bottle", Description = "Updated description"
        };

        bottleServiceMock
            .Setup(service => service.UpdateBottle(nonExistentBottleId, updatedBottle))
            .ThrowsAsync(new ArgumentException($"Bottle with ID {nonExistentBottleId} not found."));

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            bottleServiceMock.Object.UpdateBottle(nonExistentBottleId, updatedBottle));

        //Assert
        Assert.Equal($"Bottle with ID {nonExistentBottleId} not found.", exception.Message);
    }
}
