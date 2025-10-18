using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using Core.Entities;

namespace Infrastructure.Data.SeedData;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext storeContext)
    {
        if (!storeContext.Products.Any())
        {
            var products = JsonSerializer.Deserialize<List<Product>>(await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json"));
            if (products == null) return;
            storeContext.AddRange(products);
            await storeContext.SaveChangesAsync();
        }
    }
}
