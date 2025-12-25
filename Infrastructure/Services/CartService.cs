using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.Json;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class CartService(IConnectionMultiplexer redis) : ICartService
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<bool> DeleteShoppingCartAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public async Task<ShoppingCart?> GetShoppingCartAsync(string key)
    {
        RedisValue data = await _database.StringGetAsync(key);
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data.ToString());
    }

    public async Task<ShoppingCart?> SetShoppingCartAsync(ShoppingCart cart)
    {
        bool isCreated = await _database.StringSetAsync(cart.ID, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
        if (isCreated)
        {
            return await GetShoppingCartAsync(cart.ID);
        }
        return null;
    }
}
