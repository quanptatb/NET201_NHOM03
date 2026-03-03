using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;

namespace WebBanThucAnNhanh.Controllers
{
    // Bắt buộc phải đăng nhập mới được vào trang thanh toán
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }

        // === HÀM TẠO KEY COOKIE RIÊNG CHO TỪNG ACC (giống CartController) ===
        private string GetUserCartKey()
        {
            var userName = User.Identity?.IsAuthenticated == true ? User.Identity.Name : "Anonymous";
            var safeUserName = Convert.ToHexString(System.Text.Encoding.UTF8.GetBytes(userName));
            return $"Cart_{safeUserName}";
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Kiểm tra giỏ hàng — dùng đúng key theo user
            var cartKey = GetUserCartKey();
            var cookieCart = Request.Cookies[cartKey];
            List<CartItem> cartItems = new List<CartItem>();

            if (cookieCart != null)
            {
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cookieCart);
            }

            if (cartItems.Count == 0)
            {
                return RedirectToAction("Index", "Home"); // Về trang chủ nếu giỏ rỗng
            }

            // Lấy User ID từ Claim
            var userIdClaim = User.FindFirst("UserId");

            // Phòng hờ trường hợp cookie cũ chưa có UserId, bắt đăng nhập lại
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            // Tính tổng tiền
            decimal total = cartItems.Sum(item => item.Total);

            // Tạo model Order để truyền data sang View
            var orderModel = new Order
            {
                TotalPrice = total,
                UserId = int.Parse(userIdClaim.Value)
            };

            return View(orderModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("ShippingAddress,PhoneNumber")] Order order)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            // Ignore validation for non-bound properties
            ModelState.Remove("User");
            ModelState.Remove("OrderDetails");
            ModelState.Remove("TotalPrice");

            var cartKey = GetUserCartKey();
            var cookieCart = Request.Cookies[cartKey];

            if (!ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(cookieCart))
                {
                    var cartItemsInvalid = JsonConvert.DeserializeObject<List<CartItem>>(cookieCart);
                    order.TotalPrice = cartItemsInvalid.Sum(x => x.Total);
                }
                return View(order);
            }

            order.UserId = int.Parse(userIdClaim.Value);
            order.DateCreated = DateTime.Now;
            order.Status = 0; // 0: Chờ xử lý

            // Kiểm tra lại giỏ hàng trong Cookie — dùng đúng key theo user
            if (string.IsNullOrEmpty(cookieCart))
            {
                return RedirectToAction("Index", "Home");
            }

            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cookieCart);

            // Tính toán lại tổng tiền ở Server (Bảo mật)
            order.TotalPrice = cartItems.Sum(x => x.Total);

            // 1. Lưu Order
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Lưu để sinh ra OrderId

            // 2. Lưu OrderDetail và Trừ tồn kho
            foreach (var item in cartItems)
            {
                // Xử lý nối chuỗi tên món + Topping/Size (nếu có)
                string fullProductName = item.Name;
                if (item.SelectedOptions != null && item.SelectedOptions.Any())
                {
                    fullProductName += " (" + string.Join(", ", item.SelectedOptions.Select(o => o.OptionName)) + ")";
                }

                var detail = new OrderDetail
                {
                    OrderId = order.IdOrder,
                    FastFoodId = item.Id,
                    ProductName = fullProductName, // Đảm bảo toàn vẹn dữ liệu kế toán
                    Quantity = item.Quantity,
                    Price = item.Price, // Giá tại thời điểm mua
                    IsReward = item.IsReward,
                    Note = item.IsReward ? "🎁 Quà tặng từ Vòng quay may mắn" : null
                };
                _context.OrderDetails.Add(detail);

                // Trừ tồn kho (không trừ nếu là quà tặng)
                if (!item.IsReward)
                {
                    var productInDb = await _context.FastFoods.FindAsync(item.Id);
                    if (productInDb != null)
                    {
                        productInDb.Quantity -= item.Quantity;
                        // Tự động đặt hết hàng nếu số lượng <= 0
                        if (productInDb.Quantity <= 0)
                        {
                            productInDb.Quantity = 0;
                            productInDb.Status = false; // Hết hàng
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();

            // === LUCKY WHEEL: Tích lũy lượt quay ===
            var currentUser = await _context.Users.FindAsync(order.UserId);
            if (currentUser != null)
            {
                int foodSpinCount = (int)(order.TotalPrice / 300000);
                int remainder = (int)(order.TotalPrice % 300000);

                if (foodSpinCount > 0)
                {
                    currentUser.FoodSpins += foodSpinCount;
                    currentUser.DrinkSpins += foodSpinCount;
                }

                if (remainder >= 100000)
                {
                    currentUser.DrinkSpins += 1;
                }
                await _context.SaveChangesAsync();
            }

            // 3. Xóa Cookie giỏ hàng — dùng đúng key theo user
            Response.Cookies.Delete(cartKey);

            // Chuyển hướng đến trang lịch sử
            return RedirectToAction(nameof(History));
        }

        public async Task<IActionResult> History()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);

            var myOrders = await _context.Orders
                .Include(o => o.OrderDetails) // Lấy thông tin chi tiết các món
                .ThenInclude(od => od.FastFood) 
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.DateCreated)
                .ToListAsync();

            return View(myOrders);
        }
    }
}