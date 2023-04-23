using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAdmin.Data;
using ShopAdmin.Models;

namespace ShopAdmin.Controllers
{
    
    public class NotificationsController : Controller
    {
        private readonly ProductDbContext _dbContext;
        public NotificationsController(ProductDbContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            var homeView = new Notifications
            {
                Orders = _dbContext.Orders.ToList(),
            };
            return View(homeView);
        }
    }
}
