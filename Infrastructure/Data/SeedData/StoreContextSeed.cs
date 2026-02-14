using System.Reflection;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data.SeedData;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext storeContext)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!storeContext.Products.Any())
        {
            var products = JsonSerializer.Deserialize<List<Product>>(
                await File.ReadAllTextAsync(path + "/Data/SeedData/products.json"));
            if (products == null) return;
            storeContext.AddRange(products);
            await storeContext.SaveChangesAsync();
        }
        if (!storeContext.DeliveryMethods.Any())
        {
            var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(
                await File.ReadAllTextAsync(path + "/Data/SeedData/delivery.json"));
            if (deliveryMethods == null) return;
            storeContext.DeliveryMethods.AddRange(deliveryMethods);
            await storeContext.SaveChangesAsync();
        }
    }
}
