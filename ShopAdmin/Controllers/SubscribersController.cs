using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAdmin.Data;
using ShopAdmin.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using Microsoft.Extensions.Hosting;

namespace ShopAdmin.Controllers
{
	public class SubscribersController : Controller
	{
		private readonly ProductDbContext _context;
		public SubscribersController(ProductDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var sub = _context.Subscribers.OrderByDescending(s => s.DateTime).ToList();
			return View(sub);
		}

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subscribers == null)
            {
                return NotFound();
            }

            var subs = await _context.Subscribers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subs == null)
            {
                return NotFound();
            }
            return View(subs);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subscribers == null)
            {
                return Problem("Entity set is null.");
            }

            var sub = await _context.Subscribers.FindAsync(id);
            if (sub == null)
            {
                return NotFound();
            }

            _context.Subscribers.Remove(sub);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
