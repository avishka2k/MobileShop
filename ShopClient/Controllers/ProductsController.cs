using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShopClient.Data;
using ShopClient.Models;

namespace ShopClient.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductDbContext _context;
        
        public ProductsController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string[] brands, string[] categories)
        {
            string adminWebUrl = Environment.GetEnvironmentVariable("ASPNETCORE_WEB_URL");
            ViewBag.AdminWebUrl = adminWebUrl;

            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Specifications)
                .Include(p => p.Colors)
                .AsQueryable(); // start with all products

            if (brands != null && brands.Length > 0)
            {
                query = query.Where(p => brands.Contains(p.Brand.Name)); // filter by selected brands
            }

            if (categories != null && categories.Length > 0)
            {
                query = query.Where(p => categories.Contains(p.Category.Name)); // filter by selected categories
            }

            var uniqueColorCodes = _context.Colors
                                .Select(c => c.ColorCode)
                                .Distinct()
                                .ToList();

            ViewBag.UniqueColorCodes = uniqueColorCodes;

            var products = await query.ToListAsync(); // execute the query

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string adminWebUrl = Environment.GetEnvironmentVariable("ASPNETCORE_WEB_URL");
            ViewBag.AdminWebUrl = adminWebUrl;

            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Specifications)
                .Include(p => p.Colors)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Images = product.Images.ToList();
            ViewBag.Specifications = product.Specifications.ToList();
            ViewBag.Colors = product.Colors.ToList();

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Reviews,ReviewScore,Price,Stock,Delivery,SKU,CategoryId,BrandId,FirstImageUrl")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Reviews,ReviewScore,Price,Stock,Delivery,SKU,CategoryId,BrandId,FirstImageUrl")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
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
                return Problem("Entity set 'ProductDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Search(string searchTerm, string[] brands, string[] categories)
        {
            string adminWebUrl = Environment.GetEnvironmentVariable("ASPNETCORE_WEB_URL");
            ViewBag.AdminWebUrl = adminWebUrl;

            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Specifications)
                .Include(p => p.Colors)
                .AsQueryable(); // start with all products

            if (brands != null && brands.Length > 0)
            {
                query = query.Where(p => brands.Contains(p.Brand.Name)); // filter by selected brands
            }

            if (categories != null && categories.Length > 0)
            {
                query = query.Where(p => categories.Contains(p.Category.Name)); // filter by selected categories
            }

            var uniqueColorCodes = _context.Colors
                                .Select(c => c.ColorCode)
                                .Distinct()
                                .ToList();

            ViewBag.UniqueColorCodes = uniqueColorCodes;

           
            var products = await query.Where(p => p.Title.Contains(searchTerm)).ToListAsync();
            return View(products);
        }

    }
}
