using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;

namespace WebBanThucAnNhanh.Controllers
{
    [Route("[controller]")]
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }
        // GET: Order/Checkout (Khách hàng đặt món)
        // Thay thế cho hàm Create cũ
        // GET: Order/Checkout (Khách hàng đặt món)
        public IActionResult Checkout()
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
                return RedirectToAction("Index", "FastFood"); // Giỏ rỗng thì về trang mua hàng
            }

            // Lấy thông tin User để điền sẵn vào form (nếu cần)
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "User");

            // Tính tổng tiền
            double total = cartItems.Sum(item => item.Total);

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
        public async Task<IActionResult> Checkout([Bind("ShippingAddress,PhoneNumber")] Order order)
        {
            // 1. Lấy User ID
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "User");
            order.UserId = int.Parse(userIdClaim.Value);
            order.DateCreated = DateTime.Now;
            order.Status = 0; // Chờ xử lý

            // 2. Lấy Giỏ hàng từ Session (Logic giống bên CartController)
            var sessionCart = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(sessionCart))
            {
                ModelState.AddModelError("", "Giỏ hàng đang trống!");
                return View(order);
            }

            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(sessionCart);
            if (cartItems.Count == 0) return RedirectToAction("Index", "FastFood");

            // 3. Tính tổng tiền thực tế
            order.TotalPrice = cartItems.Sum(x => x.Total);

            // 4. Lưu Order trước để có ID
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 5. Lưu OrderDetail (Chi tiết đơn hàng)
            foreach (var item in cartItems)
            {
                var detail = new OrderDetail
                {
                    OrderId = order.IdOrder,      // ID đơn hàng vừa tạo
                    FastFoodId = item.Id,         // ID món ăn
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _context.OrderDetails.Add(detail);
            }
            await _context.SaveChangesAsync();

            // 6. Xóa Session giỏ hàng sau khi đặt thành công
            HttpContext.Session.Remove("Cart");

            return RedirectToAction(nameof(History));
        }
        // GET: Order/History (Khách hàng xem lịch sử đơn của mình)
        public async Task<IActionResult> History()
        {
            // Lấy ID User hiện tại
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "User");

            int userId = int.Parse(userIdClaim.Value);

            // Chỉ lấy đơn hàng CỦA USER ĐÓ
            var myOrders = await _context.Orders
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.DateCreated) // Đơn mới nhất lên đầu
                .ToListAsync();

            return View(myOrders);
        }
    }
}