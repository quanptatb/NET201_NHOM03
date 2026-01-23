using Microsoft.AspNetCore.Mvc;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;
using Newtonsoft.Json; 

namespace WebBanThucAnNhanh.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách giỏ hàng từ Session
        public List<CartItem> GetCart()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");
            if (sessionCart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(sessionCart);
            }
            return new List<CartItem>();
        }

        // Lưu danh sách giỏ hàng vào Session
        public void SaveCart(List<CartItem> cart)
        {
            var sessionCart = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", sessionCart);
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            ViewBag.TotalAmount = cart.Sum(item => item.Total);
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _context.FastFoods.FindAsync(id);
            if (product == null) return NotFound();

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(p => p.Id == id);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    Id = product.IdFastFood,
                    Name = product.NameFastFood,
                    Price = product.Price,
                    Image = product.Image,
                    Quantity = 1
                });
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart(int id, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                item.Quantity = quantity;
                if(item.Quantity <= 0) cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }
        
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index", "Home");
        }
    }
}