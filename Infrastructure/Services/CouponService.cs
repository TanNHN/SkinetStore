using Core.Entities;
using Core.Interfaces;
using Stripe;
using Microsoft.Extensions.Configuration;
using Coupon = Core.Entities.Coupon;
using StripeCoupon = Stripe.Coupon;

namespace Infrastructure.Services;

public class CouponService(IConfiguration config) : ICouponService
{
    public async Task<Coupon?> GetCouponFromPromoCode(string code)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
        PromotionCodeService promotionCodeService = new();
        PromotionCodeListOptions options = new() { Code = code, Limit = 1 };
        StripeList<PromotionCode> promotions = await promotionCodeService.ListAsync(options);

        if (promotions.Data.Count == 0 || !promotions.Data[0].Active) return null;
        Stripe.CouponService stripeCouponService = new Stripe.CouponService();
        
        return ToCouponObject(await stripeCouponService.GetAsync(promotions.Data[0].Promotion.CouponId));
    }
    private Coupon? ToCouponObject(StripeCoupon coupon)
    {
        if (coupon == null) return null;
        return new Coupon
        {
            Id = coupon.Id,
            AmountOff = coupon.AmountOff,
            PercentOff = coupon.PercentOff,
            Duration = coupon.Duration,
            DurationInMonths = coupon.DurationInMonths,
            MaxRedemptions = coupon.MaxRedemptions,
            Name = coupon.Name,
            Valid = coupon.Valid
        };
    }
    public decimal GetDiscountAmount(Coupon coupon, decimal subTotal)
    {
        if (coupon.PercentOff.HasValue)
        {
            return subTotal * (coupon.PercentOff.Value / 100);
        }
        else if (coupon.AmountOff.HasValue)
        {
            return coupon.AmountOff.Value;
        }
        return 0;
    }
}
