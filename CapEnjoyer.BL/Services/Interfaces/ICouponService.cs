namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface ICouponService
{
    public Task<List<CouponDto>> GetCouponsAsync(bool onlyActive = false);
    public Task<List<CouponDto>> GetCouponsByBuyerIdAsync(Guid userId);
    public Task<CouponDto> CreateNewCouponAsync(CouponInsertDto couponInsertDto);
    public Task<CouponDto> GetCouponByIdAsync(Guid id);
    public Task<CouponDto?> ActivateCouponAsync(string code, Guid activateeId);
}
