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
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            // Chỉ cập nhật trạng thái
            order.Status = Status; // Ví dụ: 0=Mới, 1=Đang giao, 2=Hoàn tất, 3=Hủy

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

        // Bỏ hàm Create vì Admin không nên tạo đơn hàng thủ công (Đơn hàng phải từ Checkout)
        // Nếu cần test thì giữ lại, nhưng thực tế ít dùng.

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.IdOrder == id);
        }
    }
}