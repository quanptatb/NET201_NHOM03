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
using Microsoft.AspNetCore.Identity;

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

        // 1. Đổi tên thành Index để đường dẫn là /Checkout thay vì /Checkout/Checkout
        [HttpGet]
        public IActionResult Index()
        {
            // Kiểm tra giỏ hàng
            var sessionCart = HttpContext.Session.GetString("Cart");
            List<CartItem> cartItems = new List<CartItem>();

            if (sessionCart != null)
            {
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(sessionCart);
            }

            if (cartItems.Count == 0)
            {
                return RedirectToAction("Index", "Home"); // Về trang chủ nếu giỏ rỗng
            }

            // Lấy User ID từ Claim (Nhớ sửa AccountController như hướng dẫn trên)
            var userIdClaim = User.FindFirst("UserId");

            // Phòng hờ trường hợp cookie cũ chưa có UserId, bắt đăng nhập lại
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            // Tính tổng tiền (Ép kiểu về decimal nếu CartItem dùng decimal)
            decimal total = cartItems.Sum(item => item.Total);

            // Tạo model Order để truyền data sang View
            var orderModel = new Order
            {
                TotalPrice = total,
                UserId = int.Parse(userIdClaim.Value)
            };

            return View(orderModel); // View này cần đặt tên là Index.cshtml trong thư mục Views/Checkout
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Đặt tên ActionName để trùng khớp với form post
        public async Task<IActionResult> Index([Bind("ShippingAddress,PhoneNumber")] Order order)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            order.UserId = int.Parse(userIdClaim.Value);
            order.DateCreated = DateTime.Now;
            order.Status = 0; // 0: Chờ xử lý

            // Kiểm tra lại giỏ hàng trong Session (tránh hack sửa HTML)
            var sessionCart = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(sessionCart))
            {
                return RedirectToAction("Index", "Home");
            }

            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(sessionCart);

            // Tính toán lại tổng tiền ở Server (Bảo mật)
            order.TotalPrice = cartItems.Sum(x => x.Total);

            // 1. Lưu Order
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Lưu để sinh ra OrderId

            // 2. Lưu OrderDetail
            foreach (var item in cartItems)
            {
                var detail = new OrderDetail
                {
                    OrderId = order.IdOrder,
                    FastFoodId = item.Id,
                    Quantity = item.Quantity,
                    Price = item.Price // Giá tại thời điểm mua
                };
                _context.OrderDetails.Add(detail);
            }
            await _context.SaveChangesAsync();

            // 3. Xóa Session giỏ hàng
            HttpContext.Session.Remove("Cart");

            // Chuyển hướng đến trang lịch sử
            return RedirectToAction(nameof(History));
        }

        public async Task<IActionResult> History()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);

            var myOrders = await _context.Orders
                .Include(o => o.OrderDetails) // Nên Include thêm chi tiết nếu muốn xem sâu hơn
                .ThenInclude(od => od.FastFood) // Include tên món ăn để hiển thị
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.DateCreated)
                .ToListAsync();

            return View(myOrders);
        }
    }
}