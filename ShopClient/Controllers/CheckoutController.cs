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

            // Initialize the Carts property with a new list
            model.Carts = new List<CartItem>();

            model.DateAndTime = orderDate;

            await _context.Orders.AddAsync(model);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                var orderItem = new CartItem
                {
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                };
                // Add the orderItem to the Carts property
                model.Carts.Add(orderItem);
            }

            // Update the Order record with the new list of CartItems
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
