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

        [HttpPost] // Đổi thành HttpPost vì có gửi mảng dữ liệu Topping lên
        [Authorize]
        public async Task<IActionResult> AddToCart(int id, List<int> selectedToppingIds, int quantity = 1)
        {
            var product = await _context.FastFoods.FindAsync(id);
            if (product == null) return NotFound();

            // 1. Lấy thông tin các Topping khách đã chọn từ Database 
            // (Giả sử bạn đã tạo bảng Toppings trong AppDbContext, nếu chưa xem phần 3)
            var toppings = _context.Toppings
                .Where(t => selectedToppingIds.Contains(t.IdTopping))
                .Select(t => new CartTopping { Id = t.IdTopping, Name = t.Name, Price = t.Price })
                .ToList();

            var cart = GetCart();

            // 2. Tạo một Item tạm để lấy chữ ký (CartItemKey)
            var tempItem = new CartItem
            {
                Id = product.IdFastFood,
                SelectedToppings = toppings
            };

            // 3. THUẬT TOÁN GỘP/TÁCH: Tìm xem trong giỏ đã có dòng nào trùng Key chưa
            var existingItem = cart.FirstOrDefault(p => p.CartItemKey == tempItem.CartItemKey);

            if (existingItem != null)
            {
                // CÙNG size/topping -> GỘP (Chỉ tăng số lượng)
                existingItem.Quantity += quantity;
            }
            else
            {
                // KHÁC size/topping hoặc món mới -> TÁCH (Thêm dòng mới)
                cart.Add(new CartItem
                {
                    Id = product.IdFastFood,
                    Name = product.NameFastFood,
                    Price = product.Price,
                    Image = product.Image,
                    Quantity = quantity,
                    SelectedToppings = toppings
                });
            }

            SaveCart(cart);
            return RedirectToAction("Index"); // Chuyển thẳng về trang Giỏ hàng để khách xem
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