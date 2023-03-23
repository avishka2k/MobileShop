using Microsoft.EntityFrameworkCore;
using MobileShop.Models;

namespace MobileShop.Data
{
    public class ProductDbContext: DbContext
    {
        public ProductDbContext()
        {

        }
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<TagsModel> Tags { get; set; }
    }
}
