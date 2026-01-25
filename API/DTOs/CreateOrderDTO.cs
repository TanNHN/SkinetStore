using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.OrdersAggregate;

namespace API.DTOs;

public class CreateOrderDTO
{
    [Required]
    public string CartId { get; set; } = string.Empty;
    [Required]
    public int DeliveryMethodId { get; set; }
    [Required]
    public ShippingAddress ShippingAddress { get; set; } = null!;
    [Required]
    public PaymentSummary PaymentSummary { get; set; } = null!;
}
