using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProductsController(IGenericRepository<Product> repo) : BaseAPIController
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
    {
        ISpecification<Product> spec = new ProductSpecification(specParams);
        // How does it know type T is Product
        // Base
        return await CreatePageResult(repo, spec, specParams.PageIndex, specParams.PageSize);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        Product? product = await repo.GetByIDAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);
        if (await repo.SaveAllAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }
        return BadRequest();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (!IsProductExist(id) && product.Id != id)
            return BadRequest();
        repo.Update(product);
        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        Product? product = await repo.GetByIDAsync(id);
        if (product == null)
            return BadRequest();
        repo.Remove(product);
        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest();
    }
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands(int id)
    {
        BrandListSpecification spec = new();
        return Ok(await repo.ListAsync(spec));
    }
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes(int id)
    {
        TypeListSpecification spec = new();
        return Ok(await repo.ListAsync(spec));
    }

    private bool IsProductExist(int id)
    {
        return repo.Exists(id);
    }
}
