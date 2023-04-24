using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopAdmin.Data;
using ShopAdmin.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShopAdmin.Controllers
{
    [Authorize]
    public class BrandsController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly IWebHostEnvironment environment;

        public BrandsController(ProductDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            this.environment = environment;
        }

        // GET: Brands
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var brand = await _context.Brands.Include(c => c.Products).ToListAsync();
            //var brand = await _context.Brands.ToListAsync();
            return View(brand);
        }
        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Brands"] = _context.Brands.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Brand model)
        {
            var brand = new Brand()
            {
                Name = model.Name,

            };

            if (model.PictureFile != null && model.PictureFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.PictureFile.FileName)}";
                string filePath = Path.Combine(environment.WebRootPath, "images", "brands", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.PictureFile.CopyToAsync(stream);
                }
                brand.PictureUrl = $"/images/brands/{fileName}";
            }
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Brand model)
        {
            var brand = await _context.Brands.FindAsync(model.Id);
            if (brand != null)
            {
                brand.Name = model.Name;

                if (model.PictureFile != null && model.PictureFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(brand.PictureUrl))
                    {
                        string filePat = Path.Combine(environment.WebRootPath, brand.PictureUrl.TrimStart('/'));
                        if (System.IO.File.Exists(filePat))
                        {
                            System.IO.File.Delete(filePat);
                        }
                    }

                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.PictureFile.FileName)}";
                    string filePath = Path.Combine(environment.WebRootPath, "images", "brands", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.PictureFile.CopyToAsync(stream);
                    }

                    brand.PictureUrl = $"/images/brands/{fileName}";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Brands == null)
            {
                return Problem("Entity set 'ProductDbContext.Products' is null.");
            }

            var brand = await _context.Brands
                .Include(c => c.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (brand == null)
            {
                return NotFound();
            }

            if (brand.Products.Count > 0)
            {
                ViewData["ErrorMessage"] = "Cannot delete Brand because it has associated products.";
                return RedirectToAction(nameof(Error));
            }

            try
            {
                if (!string.IsNullOrEmpty(brand.PictureUrl))
                {
                    string filePat = Path.Combine(environment.WebRootPath, brand.PictureUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePat))
                    {
                        System.IO.File.Delete(filePat);
                    }
                }
                _context.Brands.Remove(brand);
            }
            catch (Exception ex)
            {
                // Handle the exception.
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
