using Core.Entities;

namespace Core.Interfaces;

public interface ICouponService
{
    Task<Coupon?> GetCouponFromPromoCode(string code);
    decimal GetDiscountAmount(Coupon coupon, decimal subTotal);
}
