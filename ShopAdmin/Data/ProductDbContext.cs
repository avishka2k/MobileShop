using Azure;
using Microsoft.EntityFrameworkCore;
using ShopAdmin.Models;

using System.Collections.Generic;


namespace ShopAdmin.Data
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

}
