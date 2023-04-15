using Microsoft.EntityFrameworkCore;
using ShopClient.Models;

namespace ShopClient.Data
{
    public class ProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository()
        {
            var builder = WebApplication.CreateBuilder();

            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("ProductDbContext"));

            _context = new ProductDbContext(optionsBuilder.Options);
        }

        public Product Find(int id)
        {
            return _context.Products.Find(id);
        }
    }
}
