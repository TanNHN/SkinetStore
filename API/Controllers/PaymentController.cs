using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PaymentController(IPaymentService paymentService, IUnitOfWork unitOfWork) : BaseAPIController
{
    [Authorize]
    [HttpPost("{cartId}")]
    public async Task<ActionResult<ShoppingCart?>> CreateOrUpdatePaymentIntent(string cartId)
    {
        ShoppingCart? cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);
        if(cart == null) return BadRequest("Problem with your cart");
        return Ok(cart);
    }

    [HttpGet("delivery-methods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
    {
        return Ok(await unitOfWork.Repository<DeliveryMethod>().ListAllAsync());
    }
}
