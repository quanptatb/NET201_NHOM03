using Microsoft.AspNetCore.Mvc;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization; // Thêm nếu muốn chặn khách xem giỏ (tùy chọn)

namespace WebBanThucAnNhanh.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        // 1. Khai báo hằng số cho Key Session để tránh gõ sai
        public const string CART_KEY = "Cart";

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách giỏ hàng từ Session
        public List<CartItem> GetCart()
        {
            var sessionCart = HttpContext.Session.GetString(CART_KEY);
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
            HttpContext.Session.SetString(CART_KEY, sessionCart);
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            // Tính tổng tiền để hiển thị (Nếu CartItem chưa có property Total)
            ViewBag.TotalAmount = cart.Sum(item => item.Price * item.Quantity);
            return View(cart);
        }

        [Authorize] 
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
            return RedirectToAction("Index", "Home");
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
                // Nếu số lượng <= 0 thì xóa luôn sản phẩm
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove(CART_KEY);
            return RedirectToAction("Index", "Home");
        }
    }
}