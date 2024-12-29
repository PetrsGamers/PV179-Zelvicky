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
            .Select(c => c.Adapt<CouponDto>())
            .ToListAsync();


        var activateeIds = coupons
            .Where(c => c.ActivateeId.HasValue)
            .Select(c => c.ActivateeId.Value)
            .Distinct()
            .ToList();

        var buyerIds = coupons
            .Select(c => c.BuyerId)
            .Distinct()
            .ToList();

        var activateeNames = await context.Users
            .Where(u => activateeIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.Username);

        var buyerNames = await context.Users
            .Where(u => buyerIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.Username);

        var couponDtos = coupons.Select(c => new CouponDto
        {
            Id = c.Id,
            Code = c.Code,
            IsUsed = c.IsUsed,
            ValidFrom = c.ValidFrom,
            ValidUntil = c.ValidUntil,
            BuyerUsername = buyerNames.GetValueOrDefault(c.BuyerId),
            ActivateeUsername = c.ActivateeId.HasValue &&
                                activateeNames.TryGetValue(c.ActivateeId.Value,
                                    out var name)
                ? name
                : null,
            BuyerId = c.BuyerId,
            GeneratedAt = c.GeneratedAt
        }).ToList();

        return couponDtos;
    }


    public async Task<List<CouponDto>> GetCouponsByBuyerIdAsync(Guid userId)
    {
        var coupons = await context.Coupons
            .Where(c => c.BuyerId == userId)
            .ToListAsync();

        var activateeIds = coupons
            .Where(c => c.ActivateeId.HasValue)
            .Select(c => c.ActivateeId.Value)
            .Distinct()
            .ToList();

        var activateeNames = await context.Users
            .Where(u => activateeIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.Username);

        var couponDtos = coupons.Select(c => new CouponDto
        {
            Id = c.Id,
            Code = c.Code,
            IsUsed = c.IsUsed,
            ValidFrom = c.ValidFrom,
            ValidUntil = c.ValidUntil,
            ActivateeUsername = c.ActivateeId.HasValue &&
                                activateeNames.TryGetValue(c.ActivateeId.Value,
                                    out var name)
                ? name
                : null,
            BuyerId = c.BuyerId
        }).ToList();

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

    private string GenerateCouponCode()
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
