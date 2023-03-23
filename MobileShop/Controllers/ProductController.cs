using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileShop.Data;
using MobileShop.Models;
using System.IO;

namespace MobileShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDbContext dbContext;
        private readonly IWebHostEnvironment environment;
        public ProductController(ProductDbContext dbContext, IWebHostEnvironment environment)
        {
            this.dbContext = dbContext;
            this.environment = environment;
        }

        public static int RandomSKU()
        {
            Random random = new Random();
            return random.Next(1,100000);
        }
        //get all items
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var product = await dbContext.Products.ToListAsync();
            return View(product);
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductModel model)
        {
            int random= RandomSKU();
            var product = new ProductModel()
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                BrandName = model.BrandName,
                Colors = model.Colors,
                Reviews = model.Reviews,
                ReviewScore = model.ReviewScore,
                Price = model.Price,
                Stock = model.Stock,
                Delivery = model.Delivery,
                SKU = random.ToString(),
            };

            if (model.PictureFile != null && model.PictureFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.PictureFile.FileName)}";
                string filePath = Path.Combine(environment.WebRootPath, "images", "products", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.PictureFile.CopyToAsync(stream);
                }

                product.PictureUrl = $"/images/products/{fileName}";
            }
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ViewProduct(Guid id)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if(product != null)
            {
                var viewProduct = new ProductModel()
                {
                    Id = product.Id,
                    Title = product.Title,
                    Description = product.Description,
                    PictureUrl = product.PictureUrl,
                    BrandName = product.BrandName,
                    SKU = product.SKU,
                    Colors = product.Colors,
                    Reviews = product.Reviews,
                    ReviewScore = product.ReviewScore,
                    Price = product.Price,
                    Stock = product.Stock,
                    Delivery = product.Delivery,
                };
                return await Task.Run(() => View("ViewProduct", viewProduct));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductModel model)
        {
            var product = await dbContext.Products.FindAsync(model.Id);
            if(product != null)
            {
                product.Title = model.Title;
                product.Description = model.Description;
                product.BrandName = model.BrandName;
                product.Colors = model.Colors;
                product.Reviews = model.Reviews;
                product.ReviewScore = model.ReviewScore;
                product.Price = model.Price;
                product.Stock = model.Stock;
                product.Delivery = model.Delivery;
                product.SKU = model.SKU;


                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductModel model)
        {
            var product = await dbContext.Products.FindAsync(model.Id);
            if(product != null )
            {
                dbContext.Products.Remove(product);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}
