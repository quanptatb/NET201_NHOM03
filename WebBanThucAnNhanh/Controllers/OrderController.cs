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

namespace WebBanThucAnNhanh.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Order
        public async Task<IActionResult> Index()
        {
            // Sắp xếp đơn mới nhất lên đầu
            var appDbContext = _context.Orders.Include(o => o.User).OrderByDescending(o => o.DateCreated);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                // QUAN TRỌNG: Include thêm chi tiết đơn và thông tin món ăn
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.FastFood)
                .FirstOrDefaultAsync(m => m.IdOrder == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            // Chỉ cần truyền User để hiển thị tên (nếu view cần), không cho sửa User
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", order.UserId);
            return View(order);
        }

        // POST: Admin/Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int Status)
        {
            // Tìm đơn hàng gốc trong DB
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.IdOrder == id);
            if (order == null) return NotFound();

            int oldStatus = order.Status;

            // Chỉ cập nhật trạng thái
            order.Status = Status; // Ví dụ: 0=Mới, 1=Đang giao, 2=Hoàn tất, 3=Hủy

            // Trừ kho khi xác nhận đơn (chuyển từ trạng thái 0 sang 1)
            if (oldStatus == 0 && Status == 1)
            {
                foreach (var detail in order.OrderDetails)
                {
                    var product = await _context.FastFoods.FindAsync(detail.FastFoodId);
                    if (product != null)
                    {
                        product.Quantity -= detail.Quantity;
                        if (product.Quantity < 0) product.Quantity = 0;
                    }
                }
            }

            // Hoàn kho khi hủy đơn (chuyển sang trạng thái 3)
            if (oldStatus != 3 && Status == 3)
            {
                // Chỉ hoàn kho nếu đơn đã được xác nhận trước đó (đã trừ kho)
                if (oldStatus >= 1)
                {
                    foreach (var detail in order.OrderDetails)
                    {
                        var product = await _context.FastFoods.FindAsync(detail.FastFoodId);
                        if (product != null)
                        {
                            product.Quantity += detail.Quantity;
                        }
                    }
                }
            }

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

        // GET: Admin/Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.IdOrder == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                // Cần đảm bảo Database có thiết lập Cascade Delete cho OrderDetails
                // Nếu không, bạn phải xóa OrderDetails bằng tay trước khi xóa Order
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.IdOrder == id);
            if (order != null && order.Status == 0)
            {
                order.Status = 1; // Chuyển sang trạng thái Đang giao

                // Trừ số lượng sản phẩm trong kho
                foreach (var detail in order.OrderDetails)
                {
                    var product = await _context.FastFoods.FindAsync(detail.FastFoodId);
                    if (product != null)
                    {
                        product.Quantity -= detail.Quantity;
                        if (product.Quantity < 0) product.Quantity = 0;
                    }
                }

                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Bỏ hàm Create vì Admin không nên tạo đơn hàng thủ công (Đơn hàng phải từ Checkout)
        // Nếu cần test thì giữ lại, nhưng thực tế ít dùng.

        // Controllers/OrderController.cs
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, int status)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.IdOrder == id);

            if (order == null) return NotFound();

            // Logic: Khi đơn hàng chuyển sang trạng thái "Đã giao thành công" (Status = 2)
            if (status == 2 && order.Status != 2)
            {
                var user = order.User;
                if (user != null)
                {
                    // 1. Cộng điểm tích lũy: Cứ 50đ = 1 điểm
                    int earnedPoints = (int)(order.TotalPrice / 50);
                    user.LoyaltyPoints += earnedPoints;

                    // 2. Cập nhật tổng chi tiêu để xét hạng
                    user.TotalSpent += order.TotalPrice;

                    // 3. Thuật toán tự động nâng hạng theo chi tiêu
                    // Kim cương: 10tr, Vàng: 5tr, Bạc: 2tr, Đồng: 500k
                    if (user.TotalSpent >= 10000000m) user.MembershipLevel = "Kim Cương";
                    else if (user.TotalSpent >= 5000000m) user.MembershipLevel = "Vàng";
                    else if (user.TotalSpent >= 2000000m) user.MembershipLevel = "Bạc";
                    else if (user.TotalSpent >= 500000m) user.MembershipLevel = "Đồng";

                    _context.Update(user);
                }
            }

            order.Status = status;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.IdOrder == id);
        }
    }
}