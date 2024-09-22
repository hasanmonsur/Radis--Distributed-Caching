using RedisDapperDemo.Models;

namespace RedisDapperDemo.Contacts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync(int id);
    }
}