using Microsoft.AspNetCore.Mvc;
using ShopClient.Data;
using ShopClient.Models;

namespace ShopClient.Controllers
{
	public class SubscribersController : Controller
	{
		private readonly ProductDbContext _dbContext;
		public SubscribersController(ProductDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IActionResult Thankyou()
		{
			return View();
		}
        [HttpPost]
		public async Task<IActionResult> Subscribe(string email)
		{
			var sub = new Subscribers()
			{
				Email = email,
				DateTime = DateTime.Now,
			};

			await _dbContext.Subscribers.AddAsync(sub);
			await _dbContext.SaveChangesAsync();
			return RedirectToAction(nameof(Thankyou));
		}
	}
}
