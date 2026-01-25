using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entities.Product;

namespace Infrastructure.Services;

public class PaymentService(IConfiguration config, ICartService cartService, IUnitOfWork unitOfWork) : IPaymentService
{
    public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];

        ShoppingCart? cart = await cartService.GetCartAsync(cartId);
        if (cart == null) return null;
        decimal shippingPrice = 0;

        if (cart.DeliveryMethodId.HasValue)
        {
            DeliveryMethod? deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIDAsync((int)cart.DeliveryMethodId);
            if (deliveryMethod == null) return null;
            shippingPrice = deliveryMethod.Price;
        }

        foreach (var item in cart.Items)
        {
            Product? product = await unitOfWork.Repository<Product>().GetByIDAsync(item.ProductId);
            if (product == null) return null;
            if (item.Price != product.Price)
            {
                item.Price = product.Price;
            }
        }

        PaymentIntentService paymentIntentService = new();
        PaymentIntent? intent = null;
        if(string.IsNullOrEmpty(cart.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                // Stripe take amount in cents
                Amount = (long)(cart.Items.Sum(i => i.Price * i.Quantity) + shippingPrice),
                Currency = "vnd",
                PaymentMethodTypes = ["card"]
            };
            intent = await paymentIntentService.CreateAsync(options);
            cart.PaymentIntentId = intent.Id;
            cart.ClientSecret = intent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)(cart.Items.Sum(i => i.Price * i.Quantity) + shippingPrice),
            };
            intent = await paymentIntentService.UpdateAsync(cart.PaymentIntentId, options);
        }

        await cartService.SetCartAsync(cart);
        return cart;
    }
}
