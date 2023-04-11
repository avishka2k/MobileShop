using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopAdmin.Data;
using ShopAdmin.Models;

namespace ShopAdmin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ProductDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            this.environment = environment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var product = await _context.Products.Include(c => c.Category).Include(c => c.Brand).ToListAsync();
            //var spec = await _context.Products.Include(c => c.Specifications).ToListAsync();

            //var category = await _context.Categories.ToListAsync();
            return View(product);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(m => m.Id == id);
            var spec = await _context.Products.Include(p => p.Specifications).FirstOrDefaultAsync(m => m.Id == id);
            var color = await _context.Products.Include(p => p.Colors).FirstOrDefaultAsync(m => m.Id == id);
            if (product == null || spec == null || color == null)
            {
                return NotFound();
            }

            ViewBag.Images = product.Images.ToList();
            ViewBag.Specifications = spec.Specifications.ToList();
            ViewBag.Colors = spec.Colors.ToList();

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        public async Task<IActionResult> Create(Product product, List<IFormFile> Images)
        {
            var guid = $"{Guid.NewGuid()}";

            product.FirstImageUrl = $"/images/products/{guid}_{Images[0].FileName}";

            await _context.Products.AddAsync(product);
            _context.SaveChanges();

            foreach (var spec in product.Specifications)
            {
                spec.ProductId = product.Id;
                _context.Specifications.Attach(spec);
            }
            foreach (var color in product.Colors)
            {
                color.ProductId = product.Id;
                _context.Colors.Attach(color);
            }

            foreach (var image in Images)
            {
                if (image.Length > 0)
                {
                    var fileName = $"{guid}_{Path.GetFileName(image.FileName)}";
                    var filePath = Path.Combine(environment.WebRootPath, "images", "products", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    var newImage = new Image { Name = fileName, Url = "/images/products/" + fileName, ProductId = product.Id };
                   
                    _context.Images.Add(newImage);                    
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int id)
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            var product = _context.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == id);
            if ( _context.Products == null || product == null)
            {
                return NotFound();
            }
            ViewBag.Images = product.Images.ToList();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product model, List<IFormFile> Images)
        {
            var product = _context.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == model.Id);

            if (product == null)
            {
                return NotFound();
            }
            product.Title = model.Title;
            product.Description = model.Description;
            product.Colors = model.Colors;
            product.Reviews = model.Reviews;
            product.ReviewScore = model.ReviewScore;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.Delivery = model.Delivery;
            product.SKU = model.SKU;
            product.CategoryId = model.CategoryId;
            product.BrandId = model.BrandId;

            var guid = $"{Guid.NewGuid()}";

            product.FirstImageUrl = $"/images/products/{guid}_{Images[0].FileName}";

            foreach (var image in Images)
            {
                if (image.Length > 0)
                {
                    var fileName = $"{guid}_{Path.GetFileName(image.FileName)}";
                    var filePath = Path.Combine(environment.WebRootPath, "images", "products", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    if (product.Images.Count > 0)
                    {
                        var oldImage = product.Images.First();
                        System.IO.File.Delete(environment.WebRootPath + oldImage.Url);
                        _context.Images.Remove(oldImage);
                    }

                    var newImage = new Image { Name = fileName, Url = "/images/products/" + fileName, ProductId = product.Id };
                    _context.Images.Add(newImage);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ProductDbContext.Products' is null.");
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Delete associated images
            var images = await _context.Images.Where(i => i.ProductId == product.Id).ToListAsync();
            foreach (var image in images)
            {
                
                string filePath = Path.Combine(environment.WebRootPath, image.Url.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                _context.Images.Remove(image);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
