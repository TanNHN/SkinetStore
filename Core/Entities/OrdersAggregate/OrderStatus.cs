namespace Core.Entities.OrdersAggregate;

public enum OrderStatus
{
    Pending,
    PaymentReceived,
    PaymentFailed,
    PaymentMissmatch,
}
