using System;
using API.DTOs;
using Core.Entities.OrdersAggregate;

namespace API.Extensions;

public static class OrderMappingExtension
{
    public static OrderDTO ToDTO(this Order order)
    {
        List<OrderItemDTO> orderItemDTOs = order.OrderItems.Select(x => x.ToDTO()).ToList();
        return new OrderDTO
        {
            Id = order.Id,
            BuyerEmail = order.BuyerEmail,
            DeliveryMethod = order.DeliveryMethod.ShortName,
            OrderDate = order.OrderDate,
            ShippingPrice = order.DeliveryMethod.Price,
            OrderItems = orderItemDTOs,
            SubTotal = order.SubTotal,
            Total = order.GetTotal(),
            Status = order.Status.ToString(),
            PaymentIntentId = order.PaymentIntentId,
            PaymentSummary = order.PaymentSummary,
            ShippingAddress = order.ShippingAddress,
            TotalDiscount = order.TotalDiscount,
            CouponId = order.CouponId
        };
    }

    public static OrderItemDTO ToDTO(this OrderItem orderItem)
    {
        return new OrderItemDTO
        {
            ProductId = orderItem.ItemOrdered.ProductId,
            PictureUrl = orderItem.ItemOrdered.PictureUrl,
            ProductName = orderItem.ItemOrdered.Name,
            Price = orderItem.Price,
            Quantity = orderItem.Quantity,
        };
    }
}
