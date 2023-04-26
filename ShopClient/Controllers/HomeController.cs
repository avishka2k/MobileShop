using Microsoft.AspNetCore.Mvc;
using ShopClient.Data;
using ShopClient.Helpers;
using ShopClient.Models;
using System.Diagnostics;

namespace ShopClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductDbContext _context;
        public HomeController(ILogger<HomeController> logger, ProductDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            string adminWebUrl = Environment.GetEnvironmentVariable("ASPNETCORE_WEB_URL");
            ViewBag.AdminWebUrl = adminWebUrl;
            var category = _context.Categories.Take(8).OrderByDescending(p => p.Products.Count()).ToList();
            var product = _context.Products.Take(4).OrderByDescending(c => c.Id).ToList();
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.ProductForFooter = product;
            return View(category);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}