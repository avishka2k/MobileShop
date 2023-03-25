using Microsoft.EntityFrameworkCore;
using ShopAdmin.Models;
using System.Collections.Generic;

namespace ShopAdmin.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext()
        {

        }
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
    }
}
