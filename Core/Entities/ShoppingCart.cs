using System;

namespace Core.Entities;

public class ShoppingCart
{
    public required string ID { get; set; }
    public List<CartItem> Items { get; set; } = [];
}
