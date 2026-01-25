using System;

namespace Core.Entities.OrdersAggregate;

public class ProductItemOrdered
{
    public required string Name { get; set; }
    public required string PictureUrl { get; set; }
    public required int ProductId { get; set; }
}
