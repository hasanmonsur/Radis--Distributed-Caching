using Dapper;
using RedisDapperDemo.Contacts;
using RedisDapperDemo.Data;
using RedisDapperDemo.Models;
using System.Data;

namespace RedisDapperDemo.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _dapperContext;

        public ProductRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
            ;
        }

        public async Task<Product> GetProductAsync(int id)
        {
            using (var db = _dapperContext.CreateDbConnection())
            {
                var sql = "SELECT * FROM Products WHERE Id = @Id";
                return await db.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
            }

        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using (var db = _dapperContext.CreateDbConnection())
            {
                var sql = "SELECT * FROM Products";
                return await db.QueryAsync<Product>(sql);
            }
        }
    }
}
