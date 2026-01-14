using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class CartService(IConnectionMultiplexer redis) : ICartService
{
    private readonly IDatabase _redisDB = redis.GetDatabase();

    public async Task<bool> DeleteShoppingCartAsync(string key)
    {
        return await _redisDB.KeyDeleteAsync(key);
    }

    public async Task<ShoppingCart?> GetShoppingCartAsync(string key)
    {
        RedisValue data = await _redisDB.StringGetAsync(key);
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data.ToString());
    }

    public async Task<ShoppingCart?> SetShoppingCartAsync(ShoppingCart cart)
    {
        bool isCreated = await _redisDB.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
        if (isCreated)
        {
            return await GetShoppingCartAsync(cart.Id);
        }
        return null;
    }
}
