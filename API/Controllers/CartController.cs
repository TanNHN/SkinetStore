using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CartController(ICartService cartService): BaseAPIController
{
    [HttpGet]
    public async Task<ActionResult<ShoppingCart>> GetShoppingCart(string id)
    {
         ShoppingCart? cart = await cartService.GetCartAsync(id);
         return Ok(cart ?? new ShoppingCart{Id = id});
    }

    [HttpPost]
    public async Task<ActionResult<ShoppingCart>> SetShoppingCart([FromBody] ShoppingCart cart)
    {
         ShoppingCart? insertedCart = await cartService.SetCartAsync(cart);
         if(insertedCart == null) return BadRequest("Problem with cart");
         return cart;
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteShoppingCart(string id)
    {   
        bool result = await cartService.DeleteCartAsync(id);
        return result ? Ok() : BadRequest("Problem deleting cart");
    }
}
