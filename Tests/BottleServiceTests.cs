namespace Tests;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services;
using CapEnjoyer.DAL;
using CapEnjoyer.DAL.Constants;
using CapEnjoyer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class BottleServiceTests : IDisposable
{
    private readonly CapEnjoyerDbContext context;

    public BottleServiceTests()
    {
        var options = new DbContextOptionsBuilder<CapEnjoyerDbContext>()
            .UseInMemoryDatabase("TestBottleDatabase")
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
    public async Task GetBottleByIdAsyncExistingIdReturnsBottle()
    {
        // Arrange
        var bottleId = Guid.NewGuid();
        var producerId = Guid.NewGuid();
        var producer = new Producer
        {
            Id = producerId,
            Name = "Producer1",
            Description = "Description1",
            City = "Pelhrimov",
            CountryId = default
        };

        var bottle = new Bottle
        {
            Id = bottleId,
            Name = "Test Bottle",
            Description = "A really cool bottle",
            BottlePicture = "",
            CapLinks = [],
            Voltage = 4.2,
            DrinkType = DrinkType.BeerLager,
            ProducerId = producerId,
            Producer = producer
        };
        context.Bottles.Add(bottle);
        await context.SaveChangesAsync();

        // Act
        var result = await new BottleService(context).GetBottleById(bottleId);

        // Assert
        Assert.Equal(bottleId, result.Id);
        Assert.Equal("Test Bottle", result.Name);
    }

    [Fact]
    public async Task CreateBottleAsyncAddsNewBottle()
    {
        // Arrange
        var bottleService = new BottleService(context);
        var producerId = Guid.NewGuid();
        var producer = new Producer
        {
            Id = producerId,
            Name = "Producer1",
            Description = "Description1",
            City = "Pelhrimov",
            CountryId = default
        };
        context.Producers.Add(producer);
        await context.SaveChangesAsync();
        var newBottle = new BottleDto
        {
            Name = "Awesome Bottle",
            Description = "An awesome bottle with amazing design",
            BottlePicture = "",
            Voltage = 4.2,
            DrinkType = "BeerLager",
            Caps = [],
            ProducerId = producerId
        };

        // Act
        await bottleService.CreateBottle(newBottle);

        // Assert
        Assert.Equal(1, context.Bottles.Count());
        Assert.Equal("Awesome Bottle", context.Bottles.First().Name);
        Assert.Equal(newBottle.DrinkType, context.Bottles.First().DrinkType.ToString());
    }

    [Fact]
    public async Task DeleteBottleAsyncRemovesBottle()
    {
        // Arrange
        var bottleService = new BottleService(context);
        var bottleId = Guid.NewGuid();
        var producerId = Guid.NewGuid();
        var producer = new Producer
        {
            Id = producerId,
            Name = "Producer1",
            Description = "Description1",
            City = "Pelhrimov",
            CountryId = default
        };
        var bottle = new Bottle
        {
            Id = bottleId,
            Name = "Test Bottle",
            Description = "A really cool bottle",
            BottlePicture = "",
            CapLinks = [],
            Voltage = 4.2,
            DrinkType = DrinkType.BeerLager,
            ProducerId = producerId,
            Producer = producer
        };
        context.Bottles.Add(bottle);
        await context.SaveChangesAsync();

        // Act
        await bottleService.DeleteBottle(bottleId);

        // Assert
        Assert.Equal(0, context.Bottles.Count());
    }

    [Fact]
    public async Task UpdateBottleAsyncUpdatesBottle()
    {
        // Arrange
        var bottleService = new BottleService(context);
        var bottleId = Guid.NewGuid();
        var bottle = new Bottle
        {
            Id = bottleId,
            Name = "Test Bottle",
            Description = "A really cool bottle",
            BottlePicture = "",
            CapLinks = [],
            Voltage = 4.2,
            DrinkType = DrinkType.BeerLager,
            ProducerId = new Guid(),
            Producer = null
        };
        context.Bottles.Add(bottle);
        await context.SaveChangesAsync();

        var updatedBottle = new BottleDto
        {
            Id = bottleId,
            Name = "Updated Bottle",
            Description = "An updated bottle",
            BottlePicture = "",
            Voltage = 4.2,
            DrinkType = "BeerLager",
            Caps = []
        };

        // Act
        await bottleService.UpdateBottle(bottleId, updatedBottle);

        // Assert
        Assert.Equal(1, context.Bottles.Count());
        Assert.Equal("Updated Bottle", context.Bottles.First().Name);
        Assert.Equal("An updated bottle", context.Bottles.First().Description);
    }

    [Fact]
    public async Task GetBottlesAsyncReturnsAllBottles()
    {
        // Arrange
        var bottleService = new BottleService(context);
        var producerId = Guid.NewGuid();
        var producer = new Producer
        {
            Id = producerId,
            Name = "Producer1",
            Description = "Description1",
            City = "Pelhrimov",
            CountryId = default
        };
        context.Bottles.AddRange(
            new Bottle
            {
                Id = Guid.NewGuid(),
                Name = "Bottle1",
                Description = "Description1",
                Voltage = 4.2,
                DrinkType = DrinkType.BeerLager,
                BottlePicture = "",
                CapLinks = [],
                ProducerId = producerId,
                Producer = producer
            },
            new Bottle
            {
                Id = Guid.NewGuid(),
                Name = "Bottle2",
                Description = "Description2",
                Voltage = 4.2,
                DrinkType = DrinkType.BeerLager,
                BottlePicture = "",
                CapLinks = [],
                ProducerId = producerId,
                Producer = producer
            }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await bottleService.GetAllBottles();

        // Assert
        var bottleDtos = result.ToList();
        Assert.Equal(2, bottleDtos.Count);
        Assert.Equal("Bottle1", bottleDtos.First().Name);
        Assert.Equal("Bottle2", bottleDtos.Last().Name);
    }
}
