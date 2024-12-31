namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Entities;
using DTOs;
using Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

public class CouponService(CapEnjoyerDbContext context) : ICouponService
{
    public async Task<List<CouponDto>> GetCouponsAsync(bool onlyActive = false)
    {
        var coupons = await context.Coupons
            .Where(c => !onlyActive || (c.ValidFrom <= DateTime.Now && c.ValidUntil >= DateTime.Now))
            .Include(c => c.Activatee)
            .Include(c => c.Buyer)
            .ToListAsync();

        var couponDtos = coupons.Select(c => c.Adapt<CouponDto>()).ToList();
        return couponDtos;
    }


    public async Task<List<CouponDto>> GetCouponsByBuyerIdAsync(Guid userId)
    {
        var couponsWithActivateesAndBuyers = await context.Coupons
            .Where(c => c.BuyerId == userId)
            .Include(c => c.Activatee)
            .Include(c => c.Buyer)
            .ToListAsync();

        var couponDtos = couponsWithActivateesAndBuyers.Select(c => c.Adapt<CouponDto>()).ToList();
        return couponDtos;
    }


    public async Task<CouponDto> CreateNewCouponAsync(CouponInsertDto couponInsertDto)
    {
        var coupon = new Coupon
        {
            Id = new Guid(),
            Code = GenerateCouponCode(),
            BuyerId = couponInsertDto.BuyerId,
            GeneratedAt = DateTime.Now.ToUniversalTime()
        };
        await context.Coupons.AddAsync(coupon);
        await context.SaveChangesAsync();
        return coupon.Adapt<CouponDto>();
    }

    public async Task<CouponDto> GetCouponByIdAsync(Guid id)
    {
        var coupon = await context.Coupons
            .Where(c => c.Id == id).FirstOrDefaultAsync();
        return coupon.Adapt<CouponDto>();
    }

    public async Task<CouponDto?> ActivateCouponAsync(string code, Guid activateeId)
    {
        var coupon = await context.Coupons
            .Where(c => c.Code == code && c.IsUsed == false).FirstOrDefaultAsync();
        if (coupon == null)
        {
            return null;
        }

        // check if the user already has an active coupon
        var activeCoupon = await context.Coupons
            .Where(c => c.ActivateeId == activateeId && c.ValidFrom < DateTime.Now.ToUniversalTime() &&
                        c.ValidUntil > DateTime.Now.ToUniversalTime()).FirstOrDefaultAsync();
        if (activeCoupon != null)
        {
            return null;
        }

        coupon.IsUsed = true;
        coupon.ActivateeId = activateeId;
        coupon.ValidFrom = DateTime.Now.ToUniversalTime();
        coupon.ValidUntil = DateTime.Now.AddMonths(1).ToUniversalTime();
        context.Coupons.Update(coupon);
        await context.SaveChangesAsync();
        return coupon.Adapt<CouponDto>();
    }

    private static string GenerateCouponCode()
    {
        var code = "";
        var random = new Random();
        for (var i = 0; i < 8; i++)
        {
            var type = random.Next(0, 2);
            if (type == 0)
            {
                code += (char)random.Next(65, 91);
            }
            else
            {
                code += random.Next(0, 10);
            }
        }

        return code;
    }
}
