using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAdmin.Data;
using ShopAdmin.Models;
using System.Diagnostics;
using ShopAdmin.Helpers;

namespace ShopAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductDbContext _dbcontext;
        private readonly VisitCountService _visitCountService;

        public HomeController(ILogger<HomeController> logger, ProductDbContext context, VisitCountService visitCountService)
        {
            _logger = logger;
            _dbcontext = context;
            _visitCountService = visitCountService;
        }

        public IActionResult Index()
        {
            var topSellingProducts = _dbcontext.Orders
                .SelectMany(o => o.Carts)
                .GroupBy(c => c.ProductId)
                .Select(g => new { ProductId = g.Key, Quantity = g.Sum(c => c.Quantity) })
                .OrderByDescending(p => p.Quantity)
                .Take(6)
                .Join(_dbcontext.Products, p => p.ProductId, prod => prod.Id, (p, prod) => new { Product = prod, Quantity = p.Quantity });
            ViewBag.TopSellingProducts = topSellingProducts;
            var subCount = _dbcontext.Subscribers.Count();
            ViewBag.SubCount = subCount;

            var homeView = new HomeViewModel
            {
                Orders = _dbcontext.Orders.ToList(),
                Products = _dbcontext.Products.ToList(),
            };

            ViewBag.RevenueIncrease = RevenueIncrease();
            ViewBag.OrderIncrease = OrderIncrease();
            ViewBag.SubIncrease = SubIncrease();

            int visitCount = _dbcontext.VisitCounts.First().VisitCount;
            ViewBag.VisitCount = visitCount;

            return View(homeView);
        }

        private double RevenueIncrease()
        {
            var todayOrders = _dbcontext.Orders.Where(o => o.DateAndTime.Date == DateTime.Today).ToList();
            double? todayRevenue = todayOrders.Sum(o => o.TotalPrice);

            var yesterdayOrders = _dbcontext.Orders.Where(o => o.DateAndTime.Date == DateTime.Today.AddDays(-1)).ToList();
            double? yesterdayRevenue = yesterdayOrders.Sum(o => o.TotalPrice);

            if (yesterdayRevenue.HasValue && todayRevenue.HasValue)
            {
                double revenueIncrease = ((todayRevenue.Value - yesterdayRevenue.Value) / yesterdayRevenue.Value) * 100;
                return revenueIncrease;
            }
            else
            {
                return 0;
            }
        }
        private double OrderIncrease()
        {
            double todayOrders = _dbcontext.Orders.Where(o => o.DateAndTime.Date == DateTime.Today).ToList().Count;

            double yesterdayOrders = _dbcontext.Orders.Where(o => o.DateAndTime.Date == DateTime.Today.AddDays(-1)).ToList().Count;

            if (todayOrders > 0 && yesterdayOrders > 0)
            {
                double revenueIncrease = ((todayOrders - yesterdayOrders) / yesterdayOrders) * 100;
                return revenueIncrease;
            }
            else
            {
                return 0;
            }
        }

        private double SubIncrease()
        {
            double todaySub = _dbcontext.Subscribers.Where(o => o.DateTime.Date == DateTime.Today).ToList().Count;

            double yesterdaySub = _dbcontext.Subscribers.Where(o => o.DateTime.Date == DateTime.Today.AddDays(-1)).ToList().Count;

            if (todaySub > 0 && yesterdaySub > 0)
            {
                double subIncrease = ((todaySub - yesterdaySub) / yesterdaySub) * 100;
                return subIncrease;
            }
            else
            {
                return 0;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}