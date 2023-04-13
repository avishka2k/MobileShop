using Microsoft.EntityFrameworkCore;
using ShopClient.Models;

namespace ShopClient.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<Color> Colors { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Image>()
               .HasOne(i => i.Product)
               .WithMany(p => p.Images)
               .HasForeignKey(i => i.ProductId);
            modelBuilder.Entity<Product>()
               .HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Product>()
               .HasOne(p => p.Brand)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.BrandId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Specification>()
               .HasOne(t => t.Product)
               .WithMany(p => p.Specifications)
               .HasForeignKey(t => t.ProductId);
            modelBuilder.Entity<Color>()
               .HasOne(t => t.Product)
               .WithMany(p => p.Colors)
               .HasForeignKey(t => t.ProductId);
        }
    }

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
