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
using System.Security.Claims; // Cần thiết để lấy UserID từ Cookie

namespace WebBanThucAnNhanh.Controllers
{
    [Authorize] // Yêu cầu: Phải đăng nhập mới được vào Controller này
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // KHU VỰC CHO ADMIN (QUẢN LÝ)
        // ==========================================

        // GET: Order (Admin xem toàn bộ đơn hàng)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Orders.Include(o => o.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Order/Edit/5 (Admin cập nhật trạng thái đơn hàng)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username", order.UserId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("IdOrder,DateCreated,TotalPrice,Status,ShippingAddress,PhoneNumber,UserId")] Order order)
        {
            if (id != order.IdOrder) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.IdOrder)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username", order.UserId);
            return View(order);
        }

        // GET: Order/Delete/5 (Admin xóa đơn)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.IdOrder == id);
            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // KHU VỰC CHO CUSTOMER (KHÁCH HÀNG)
        // ==========================================

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

        // ==========================================
        // CHUNG (Shared)
        // ==========================================

        // GET: Order/Details/5
        // Ai cũng xem được chi tiết đơn (nhưng cần logic check chủ sở hữu nếu kỹ hơn)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.IdOrder == id);

            if (order == null) return NotFound();

            // Bảo mật: Nếu là Customer, chỉ cho xem đơn của chính mình
            if (!User.IsInRole("Admin"))
            {
                var currentUserId = int.Parse(User.FindFirst("UserId").Value);
                if (order.UserId != currentUserId)
                {
                    return Forbid(); // Hoặc Redirect to AccessDenied
                }
            }

            return View(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.IdOrder == id);
        }
    }
}