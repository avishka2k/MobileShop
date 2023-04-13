using Microsoft.AspNetCore.Mvc;
using ShopClient.Data;
using ShopClient.Helpers;
using ShopClient.Models;

namespace ShopClient.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly ProductRepository _productRepository;

        public CartController()
        {
            _productRepository = new ProductRepository();
        }
        public IActionResult Index()
        {
            string adminWebUrl = Environment.GetEnvironmentVariable("ASPNETCORE_WEB_URL");
            ViewBag.AdminWebUrl = adminWebUrl;

            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            if (cart != null)
            {
                ViewBag.total = cart.Sum(item => Convert.ToInt32(item.Product.Price) * item.Quantity);
            }
            else
            {
                ViewBag.total = 0;
            }
            return View();
        }

        [Route("buy/{id}")]
        public IActionResult Buy(int id)
        {
            ProductRepository productRepository = new ProductRepository();
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                List<CartItem> cart = new List<CartItem>();
                cart.Add(new CartItem { Product = productRepository.Find(id), Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                TempData["AlertMessage"] = "Success";
            }
            else
            {
                List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { Product = productRepository.Find(id), Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index", "Products");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index", "Cart");
        }

        private int isExist(int id)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id == id)
                {
                    return i;
                }
            }
            return -1;
        }

        [HttpPost]
        public IActionResult UpdateCartQuantity(int productId, int quantityChange)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = isExist(productId);
            if (index != -1)
            {
                cart[index].Quantity += quantityChange;
                if (cart[index].Quantity < 1)
                {
                    cart.RemoveAt(index);
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                return Json(new { success = true, cartQuantity = cart.Sum(item => item.Quantity) });
            }
            return Json(new { success = false });
        }
        [Route("count")]
        public int Count()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                return 0;
            }
            return cart.Sum(item => item.Quantity);
        }


    }
}
