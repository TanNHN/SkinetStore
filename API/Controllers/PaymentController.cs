using API.Extensions;
using API.SignalR;
using Core.Entities;
using Core.Entities.OrdersAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;

namespace API.Controllers;

public class PaymentController(IPaymentService paymentService, IUnitOfWork unitOfWork,
ILogger<PaymentController> logger, IConfiguration configuration,
IHubContext<NotificationHub> hubContext) : BaseAPIController
{
    private readonly string _whSecret = configuration["StripeSettings:whSecret"] ?? "";

    [Authorize]
    [HttpPost("{cartId}")]
    public async Task<ActionResult<ShoppingCart?>> CreateOrUpdatePaymentIntent(string cartId)
    {
        ShoppingCart? cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);
        if (cart == null) return BadRequest("Problem with your cart");
        return Ok(cart);
    }

    [HttpGet("delivery-methods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
    {
        return Ok(await unitOfWork.Repository<DeliveryMethod>().ListAllAsync());
    }

    [HttpPost("webhook")]
    public async Task<ActionResult> StripeWebHook()
    {
        string json = await new StreamReader(Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = ConstructStripeEvent(json);
            if (stripeEvent.Data.Object is not PaymentIntent intent)
            {
                return BadRequest("Invalid event data");
            }
            await HandlePaymentIntentSucceeded(intent);
            return Ok();
        }
        catch (StripeException ex)
        {
            logger.LogError(ex, "Stripe webhook error");
            return StatusCode(StatusCodes.Status500InternalServerError, "");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Webhook error occurred");
            return StatusCode(StatusCodes.Status500InternalServerError, "");
        }
    }

    private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
    {
        if (intent.Status == "succeeded")
        {
            OrderSpecification spec = new(intent.Id, true);
            var order = await unitOfWork.Repository<Core.Entities.OrdersAggregate.Order>().GetEntityWithSpec(spec) ?? throw new Exception("Order not found");
            if ((long)order.GetTotal() != intent.Amount)
            {
                order.Status = OrderStatus.PaymentMissmatch;
            }
            else
            {
                order.Status = OrderStatus.PaymentReceived;
            }

            await unitOfWork.Complete();

            string? connectionId = NotificationHub.GetConnectionIdByEmail(order.BuyerEmail);
            if(!string.IsNullOrEmpty(connectionId))
            {
                await hubContext.Clients.Client(connectionId).SendAsync("OrderCompleteNotification", order.ToDTO());
            }
        }
    }

    private Event ConstructStripeEvent(string json)
    {
        try
        {
            return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _whSecret, 300, false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to construct Stripe event");
            throw new Exception(ex.Message);
        }
    }
}
