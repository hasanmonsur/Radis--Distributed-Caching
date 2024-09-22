using Microsoft.AspNetCore.Mvc;
using RedisDapperDemo.Contacts;
using RedisDapperDemo.Models;
using RedisDapperDemo.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisDapperDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IConnectionMultiplexer _redis;

        public ProductsController(IProductRepository repository, IConnectionMultiplexer redis)
        {
            _repository = repository;
            _redis = redis;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var cacheKey = $"product:{id}";
            var redisDb = _redis.GetDatabase();

            // Check the cache first
            var cachedProduct = await redisDb.StringGetAsync(cacheKey);
            if (cachedProduct.HasValue)
            {
                var product = JsonSerializer.Deserialize<Product>(cachedProduct);
                return Ok(product); // Return from cache
            }

            // If not in cache, fetch from database
            var productFromDb = await _repository.GetProductAsync(id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            // Store the result in cache
            await redisDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(productFromDb), TimeSpan.FromMinutes(30));

            return Ok(productFromDb); // Return fresh data
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _repository.GetAllProductsAsync();
            return Ok(products);
        }
    }
}
