using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Thêm thư viện này
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
            HttpContext.Session.Remove(CART_KEY);
            return RedirectToAction("Index", "Home");
        }
    }
}