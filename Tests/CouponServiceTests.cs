namespace Tests;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services;
using CapEnjoyer.DAL;
using CapEnjoyer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class CouponServiceTests : IDisposable
{
    private readonly CapEnjoyerDbContext context;

    public CouponServiceTests()
    {
        var options = new DbContextOptionsBuilder<CapEnjoyerDbContext>()
            .UseInMemoryDatabase("TestCouponDatabase")
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
    public async Task GetCouponByIdAsyncExistingIdReturnsCoupon()
    {
        // Arrange
        var couponId = Guid.NewGuid();
        var buyerId = Guid.NewGuid();
        var coupon = new Coupon
        {
            Id = couponId, Code = "TestCode", BuyerId = buyerId, GeneratedAt = DateTime.Now.ToUniversalTime()
        };
        context.Coupons.Add(coupon);
        await context.SaveChangesAsync();

        // Act
        var couponService = new CouponService(context);
        var result = await couponService.GetCouponByIdAsync(couponId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(couponId, result.Id);
        Assert.Equal("TestCode", result.Code);
        Assert.Equal(buyerId, result.BuyerId);
    }

    [Fact]
    public async Task ActivateCouponAsyncExistingIdActivatesCoupon()
    {
        // Arrange
        var couponId = Guid.NewGuid();
        var buyerId = Guid.NewGuid();
        var activateeId = Guid.NewGuid();
        var coupon = new Coupon
        {
            Id = couponId, Code = "AB1337LOL", BuyerId = buyerId, GeneratedAt = DateTime.Now.ToUniversalTime()
        };
        context.Coupons.Add(coupon);
        await context.SaveChangesAsync();

        // Act
        var couponService = new CouponService(context);
        await couponService.ActivateCouponAsync(coupon.Code, activateeId);
        var result = await couponService.GetCouponByIdAsync(couponId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(coupon.Code, result.Code);
        Assert.Equal(couponId, result.Id);
        Assert.Equal(buyerId, result.BuyerId);
        Assert.Equal(activateeId, result.ActivateeId);
    }

    [Fact]
    public async Task CreateNewCouponAsyncAddsNewCoupon()
    {
        // Arrange
        var couponInsertDto = new CouponInsertDto { BuyerId = Guid.NewGuid() };
        var couponService = new CouponService(context);

        // Act
        var result = await couponService.CreateNewCouponAsync(couponInsertDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Code);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(couponInsertDto.BuyerId, result.BuyerId);
    }

    [Fact]
    public async Task UserIsProperlyLinkedAfterActivation()
    {
        var couponId = Guid.NewGuid();
        var buyerId = Guid.NewGuid();
        var activateeId = Guid.NewGuid();

        var couponService = new CouponService(context);
        var couponInsertDto = new CouponInsertDto { BuyerId = buyerId };
        var coupon = await couponService.CreateNewCouponAsync(couponInsertDto);
        var couponActivated = await couponService.ActivateCouponAsync(coupon.Code, activateeId);

        Assert.NotNull(couponActivated);
        Assert.Equal(activateeId, couponActivated.ActivateeId);
        Assert.Equal(buyerId, couponActivated.BuyerId);
        Assert.Equal(coupon.Code, couponActivated.Code);
    }
}
