namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CouponController(ICouponService couponService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCoupons()
    {
        var coupons = await couponService.GetCouponsAsync();
        return Ok(coupons);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCouponById(Guid id)
    {
        var coupon = await couponService.GetCouponByIdAsync(id);
        return Ok(coupon);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCoupon(CouponInsertDto coupon)
    {
        var createdCoupon = await couponService.CreateNewCouponAsync(coupon);
        return Ok(createdCoupon);
    }

    [HttpPost("/activate")]
    public async Task<IActionResult> ActivateCoupon(string code, Guid activateeId)
    {
        var activatedCoupon = await couponService.ActivateCouponAsync(code, activateeId);
        if (activatedCoupon == null)
        {
            return NotFound();
        }

        return Ok(activatedCoupon);
    }
}
