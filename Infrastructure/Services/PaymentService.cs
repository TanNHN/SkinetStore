using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entities.Product;

namespace Infrastructure.Services;

public class PaymentService(
    IConfiguration config,
    ICartService cartService,
    IUnitOfWork unitOfWork,
    ICouponService couponService) : IPaymentService
{
    public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
        ShoppingCart? cart = await cartService.GetCartAsync(cartId);
        if (cart == null) return null;

        await ValidateProductPrice(cart);
        decimal shippingPrice = await GetShippingPriceAsync(cart);
        decimal subTotal = cart.Items.Sum(item => item.Price * item.Quantity);
        if (cart.Coupon != null)
        {
            subTotal -= couponService.GetDiscountAmount(cart.Coupon, subTotal);
        }
        decimal total = subTotal + shippingPrice;

        await CreateOrUpdatePaymentIntent(cart, total);

        await cartService.SetCartAsync(cart);
        return cart;
    }

    public async Task<decimal> GetShippingPriceAsync(ShoppingCart cart)
    {

        decimal shippingPrice = 0;
        if (cart.DeliveryMethodId.HasValue)
        {
            DeliveryMethod? deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIDAsync((int)cart.DeliveryMethodId);
            if (deliveryMethod == null) throw new Exception($"Delivery method with id {cart.DeliveryMethodId} not found");
            shippingPrice = deliveryMethod.Price;
        }
        return shippingPrice;
    }

    public async Task ValidateProductPrice(ShoppingCart cart)
    {
        foreach (var item in cart.Items)
        {
            Product? product = await unitOfWork.Repository<Product>().GetByIDAsync(item.ProductId);
            if (product == null) throw new Exception($"Product with id {item.ProductId} not found");
            if (item.Price != product.Price)
            {
                item.Price = product.Price;
            }
        }
    }

    public async Task CreateOrUpdatePaymentIntent(ShoppingCart cart, decimal total)
    {
        PaymentIntentService paymentIntentService = new();
        PaymentIntent? intent = null;
        if (string.IsNullOrEmpty(cart.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                // if Currency = Dollar, Stripe take amount in cents
                Amount = (long)total,
                Currency = "vnd",
                PaymentMethodTypes = ["card"]
            };
            intent = await paymentIntentService.CreateAsync(options);
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)total,
            };
            intent = await paymentIntentService.UpdateAsync(cart.PaymentIntentId, options);
        }
        cart.PaymentIntentId = intent.Id;
        cart.ClientSecret = intent.ClientSecret;
        await cartService.SetCartAsync(cart);
    }
}
