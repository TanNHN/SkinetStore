using Core.Entities.OrdersAggregate;

namespace Core.Specification;

public class OrderSpecification : BaseSpecification<Order>
{
    public OrderSpecification(string email) : base(o => o.BuyerEmail == email)
    {
        AddInclude(o => o.DeliveryMethod);
        AddInclude(o => o.OrderItems);
        AddOrderByDesc(o => o.OrderDate);
    }

    public OrderSpecification(string email, int id) : base
    (
        o => o.BuyerEmail == email && o.Id == id
    )
    {
        AddInclude("OrderItems");
        AddInclude("DeliveryMethod");

    }
}
