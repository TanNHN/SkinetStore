using System;
using Core.Entities.OrdersAggregate;

namespace API.DTOs;

public class OrderDTO
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public required string BuyerEmail { get; set; }
    public required ShippingAddress ShippingAddress { get; set; }
    public required string DeliveryMethod { get; set; }
    public required PaymentSummary PaymentSummary { get; set; }
    public required List<OrderItemDTO> OrderItems { get; set; }
    public decimal SubTotal { get; set; }
    public decimal ShippingPrice { get; set; }
    public decimal Total { get; set; }
    public required string Status { get; set; }
    public required string PaymentIntentId { get; set; }
}

public class OrderItemDTO
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string PictureUrl { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}