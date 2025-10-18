using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await repo.GetProductsAsync(brand, type ,sort));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        Product? product = await repo.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.AddProduct(product);
        if (await repo.SaveChangeAsync())
        {
            return CreatedAtAction("GetProduct", new {id = product.Id }, product);
        }
        return BadRequest();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (!IsProductExist(id) && product.Id != id)
            return BadRequest();
        repo.UpdateProduct(product);
        if (await repo.SaveChangeAsync())
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        Product? product = await repo.GetProductByIdAsync(id);
        if (product == null)
            return BadRequest();
        repo.DeleteProduct(product);
        if (await repo.SaveChangeAsync())
        {
            return NoContent();
        }
        return BadRequest();
    }
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands(int id)
    {
        return Ok(await repo.GetBrandAsync()); 
    }
    [HttpGet("types")]
     public async Task<ActionResult<IReadOnlyList<string>>> GetTypes(int id)
    {
        return Ok(await repo.GetTypeAsync()); 
    }

    private bool IsProductExist(int id)
    {
        return repo.ProductExists(id);
    }
}
