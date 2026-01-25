using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrdersAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrdersController(ICartService cartService, IUnitOfWork unitOfWork) : BaseAPIController
{
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(CreateOrderDTO orderDTO)
    {
        ShoppingCart? cart = await cartService.GetCartAsync(orderDTO.CartId);
        if(cart == null) return BadRequest("Cart not found");
        if(string.IsNullOrEmpty(cart.PaymentIntentId)) return BadRequest("Payment intent not found");
        List<OrderItem> orderItems = new();
        foreach(CartItem item in cart.Items)
        {
            Product? product = await unitOfWork.Repository<Product>().GetByIDAsync(item.ProductId);
            if(product == null) return BadRequest($"Can't get Product {item.ProductId} info in cart");
            ProductItemOrdered itemOrdered = new()
            {
                Name = product.Name,
                PictureUrl = product.PictureUrl,
                ProductId = product.Id
            };
            OrderItem orderItem = new()
            {
              ItemOrdered = itemOrdered,
              Price = product.Price,
              Quantity = item.Quantity  
            };
            orderItems.Add(orderItem);
        }

        DeliveryMethod? deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIDAsync(cart.DeliveryMethodId ?? -1);
        if(deliveryMethod == null) return BadRequest("Delivery method not found");
        
        Order order = new()
        {
            OrderItems = orderItems,
            DeliveryMethod = deliveryMethod,
            ShippingAddress = orderDTO.ShippingAddress,
            SubTotal = orderItems.Sum(i => i.Price * i.Quantity),
            PaymentSummary = orderDTO.PaymentSummary,
            PaymentIntentId = cart.PaymentIntentId,
            BuyerEmail = User.GetEmail()
        };
        unitOfWork.Repository<Order>().Add(order);
        if (await unitOfWork.Complete())
        {
            return order;
        }
        return Problem("Problem creating order");
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDTO>>> GetOrdersByUserId()
    {
        OrderSpecification spec = new(User.GetEmail());
        IReadOnlyList<Order> orders = await unitOfWork.Repository<Order>().ListAsync(spec);
        IReadOnlyList<OrderDTO> orderToReturn = orders.Select(o => o.ToDTO()).ToList();
        //C# doesn't support implicit cast operators on interfaces. So we has to wrap Ok() by oursef
        return Ok(orderToReturn);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
    {
        OrderSpecification spec = new(User.GetEmail(), id);
        Order? order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec); 
        if(order == null) return NotFound();
    
        return order.ToDTO();
    }
}
