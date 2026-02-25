using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CouponsController(ICouponService couponService) : BaseAPIController
{
    [HttpGet("{code}")]
    public async Task<ActionResult<Coupon>> GetCoupon(string code)
    {
        Coupon? coupon = await couponService.GetCouponFromPromoCode(code);
        if (coupon == null) return BadRequest("Invalid promo code");
        return Ok(coupon);
    }
}
