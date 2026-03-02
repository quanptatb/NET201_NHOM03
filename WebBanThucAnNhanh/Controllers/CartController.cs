using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Thêm thư viện này
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization; // Thêm nếu muốn chặn khách xem giỏ (tùy chọn)

namespace WebBanThucAnNhanh.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        // 1. HÀM TẠO KEY RIÊNG CHO TỪNG ACC
        // 1. HÀM TẠO KEY RIÊNG CHO TỪNG ACC
        private string GetUserCartKey()
        {
            var userName = User.Identity?.IsAuthenticated == true ? User.Identity.Name : "Anonymous";

            // Mã hóa tên đăng nhập thành chuỗi Hex (VD: "NhưBùi" -> "E1BBA..."). An toàn tuyệt đối 100%.
            var safeUserName = Convert.ToHexString(System.Text.Encoding.UTF8.GetBytes(userName));

            return $"Cart_{safeUserName}";
        }

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách giỏ hàng từ Session
        // Đọc giỏ hàng từ Cookie thay vì Session
        public List<CartItem> GetCart()
        {
            // Lấy giỏ hàng theo đúng acc đang đăng nhập
            var cookieCart = Request.Cookies[GetUserCartKey()];
            if (cookieCart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(cookieCart);
            }
            return new List<CartItem>();
        }

        // Lưu danh sách giỏ hàng vào Session
        // Lưu giỏ hàng vào Cookie thay vì Session
        public void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                IsEssential = true
            };
            // Lưu giỏ hàng theo đúng acc
            Response.Cookies.Append(GetUserCartKey(), cartJson, cookieOptions);
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            // Tính tổng tiền để hiển thị (Nếu CartItem chưa có property Total)
            ViewBag.TotalAmount = cart.Sum(item => item.Total);
            return View(cart);
        }

        [Authorize]
        public async Task<IActionResult> AddToCart(int id, int quantity = 1, List<int> selectedOptionIds = null)
        {
            var product = await _context.FastFoods.FindAsync(id);
            if (product == null) return NotFound();

            // 1. Khởi tạo danh sách option cho item này
            var cartItemOptions = new List<CartItemOption>();

            if (selectedOptionIds != null && selectedOptionIds.Any())
            {
                // Lấy thông tin giá, tên của các option từ DB
                var options = await _context.OptionItems
                                            .Where(o => selectedOptionIds.Contains(o.Id))
                                            .ToListAsync();
                foreach (var opt in options)
                {
                    cartItemOptions.Add(new CartItemOption
                    {
                        OptionItemId = opt.Id,
                        OptionName = opt.Name,
                        AdditionalPrice = opt.AdditionalPrice
                    });
                }
            }

            var cart = GetCart();

            // 2. Tạo một đối tượng giả lập để sinh ra chữ ký (Signature)
            var tempItem = new CartItem
            {
                Id = product.IdFastFood,
                SelectedOptions = cartItemOptions
            };
            string targetSignature = tempItem.CartItemSignature;

            // 3. THUẬT TOÁN CỐT LÕI: Kiểm tra trùng chữ ký
            // Tìm trong giỏ xem có sản phẩm nào CÙNG MÃ MÓN và CÙNG ĐÚNG CÁC OPTION hay không
            var existingItem = cart.FirstOrDefault(p => p.CartItemSignature == targetSignature);

            if (existingItem != null)
            {
                // TRƯỜNG HỢP GỘP DÒNG: Cùng món, cùng size/topping -> Chỉ tăng số lượng
                existingItem.Quantity += quantity;
            }
            else
            {
                // TRƯỜNG HỢP TÁCH DÒNG: Khác size/topping -> Thêm 1 dòng mới hoàn toàn
                cart.Add(new CartItem
                {
                    Id = product.IdFastFood,
                    Name = product.NameFastFood,
                    BasePrice = product.Price, // Đổi từ Price thành BasePrice theo model mới
                    Image = product.Image,
                    Quantity = quantity,
                    SelectedOptions = cartItemOptions // Lưu danh sách topping đi kèm
                });
            }

            SaveCart(cart);

            // Nếu gọi qua AJAX có thể trả về JSON
            // return Json(new { success = true, cartCount = cart.Sum(c => c.Quantity) });
            return RedirectToAction("Index", "Cart");
        }

        // --- LƯU Ý KHI UPDATE & REMOVE ---
        // Khi xóa (Remove) hoặc Cập nhật (UpdateCart), bạn không nên tìm qua p.Id nữa
        // mà phải truyền Signature vào để tìm chính xác dòng nào cần xóa.

        [HttpPost] // Nên dùng HttpPost cho các hành động thay đổi dữ liệu
        public IActionResult Remove(string signature)
        {
            var cart = GetCart();
            // Tìm chính xác dòng cần xóa thông qua chữ ký duy nhất
            var item = cart.FirstOrDefault(p => p.CartItemSignature == signature);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCart(string signature, int quantity)
        {
            var cart = GetCart();
            // Tìm chính xác dòng cần cập nhật thông qua chữ ký
            var item = cart.FirstOrDefault(p => p.CartItemSignature == signature);

            if (item != null)
            {
                item.Quantity = quantity;

                // Nếu số lượng <= 0 thì xóa luôn sản phẩm khỏi giỏ
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
            // Xóa đúng giỏ hàng của acc đó
            Response.Cookies.Delete(GetUserCartKey());
            return RedirectToAction("Index", "Home");
        }
    }
}