using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ShopClient.Data;
using ShopClient.Helpers;
using ShopClient.Models;
using Stripe;

namespace ShopClient.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ProductDbContext _context;

        public CheckoutController(ProductDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(double deliveryFee)
        {
            string adminWebUrl = Environment.GetEnvironmentVariable("ASPNETCORE_WEB_URL");
            ViewBag.AdminWebUrl = adminWebUrl;
            var product = _context.Products.Take(4).OrderByDescending(c => c.Id).ToList();
            ViewBag.ProductForFooter = product;

            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");


            //var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;


            // Calculate the total cost of the order
            double subtotal;
            if (cart != null)
            {
                subtotal = cart.Sum(item => Convert.ToInt32(item.Product.Price) * item.Quantity);
            }
            else
            {
                subtotal = 0;
            }
            double total = subtotal + deliveryFee;
            // Create a new view model to pass to the view
            Order viewModel = new Order
            {
                Carts = cart,
                Delivery = deliveryFee,
                TotalPrice = total,
            };
            return View(viewModel);        
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(double deliveryFee, Order model)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            var product = _context.Products.Take(4).OrderByDescending(c => c.Id).ToList();
            ViewBag.ProductForFooter = product;
            double subtotal;
            if (cart != null)
            {
                subtotal = cart.Sum(item => Convert.ToInt32(item.Product.Price) * item.Quantity);
            }
            else
            {
                subtotal = 0;
            }
            double total = subtotal + deliveryFee;
            DateTime orderDate = DateTime.Now;

            model.Carts = new List<CartItem>();
            model.DateAndTime = orderDate;
            model.OrderStatus = 0;
            await _context.Orders.AddAsync(model);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                var orderItem = new CartItem
                {
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    Color = item.Color
                };
                model.Carts.Add(orderItem);
            }
            _context.Orders.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(OrderConfirmation));
        }


        [HttpGet]
        public IActionResult OrderConfirmation()
        {
            return View();
        }

    }
}
