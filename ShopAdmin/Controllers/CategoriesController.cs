using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopAdmin.Data;
using ShopAdmin.Models;

namespace ShopAdmin.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly IWebHostEnvironment environment;

        public CategoriesController(ProductDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            this.environment = environment;
        }


        //get all items
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            var category = await _context.Categories.Include(c => c.Products).ToListAsync();
            //var category = await _context.Categories.ToListAsync();
            return View(category);
        }
        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Categories"] = _context.Categories.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            var category = new Category()
            {
                Name = model.Name,

            };

            if (model.PictureFile != null && model.PictureFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.PictureFile.FileName)}";
                string filePath = Path.Combine(environment.WebRootPath, "images", "categories", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.PictureFile.CopyToAsync(stream);
                }

                category.PictureUrl = $"/images/categories/{fileName}";
            }
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category != null)
            {
                var viewProduct = new Category()
                {
                    Id = category.Id,
                    Name = category.Name,
                    PictureUrl = category.PictureUrl,
                };
                return await Task.Run(() => View("Details", viewProduct));
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            var category = await _context.Categories.FindAsync(model.Id);
            if (category != null)
            {
                category.Name = model.Name;

                if (model.PictureFile != null && model.PictureFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(category.PictureUrl))
                    {
                        string filePat = Path.Combine(environment.WebRootPath, category.PictureUrl.TrimStart('/'));
                        if (System.IO.File.Exists(filePat))
                        {
                            System.IO.File.Delete(filePat);
                        }
                    }

                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.PictureFile.FileName)}";
                    string filePath = Path.Combine(environment.WebRootPath, "images", "categories", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.PictureFile.CopyToAsync(stream);
                    }

                    category.PictureUrl = $"/images/categories/{fileName}";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ProductDbContext.Products' is null.");
            }

            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            if (category.Products.Count > 0)
            {
                ViewData["ErrorMessage"] = "Cannot delete category because it has associated products.";
                return RedirectToAction(nameof(Error));
            }

            try
            {
                if (!string.IsNullOrEmpty(category.PictureUrl))
                {
                    string filePat = Path.Combine(environment.WebRootPath, category.PictureUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePat))
                    {
                        System.IO.File.Delete(filePat);
                    }
                }
                _context.Categories.Remove(category);
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
