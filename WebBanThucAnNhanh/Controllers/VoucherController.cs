using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;

namespace WebBanThucAnNhanh.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VoucherController : Controller
    {
        private readonly AppDbContext _context;

        public VoucherController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Voucher
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vouchers.OrderByDescending(v => v.Id).ToListAsync());
        }

        // GET: Voucher/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Voucher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,DiscountType,DiscountValue,MaxDiscountAmount,MinOrderValue,ExpiryDate,IsActive")] Voucher voucher)
        {
            // Custom Validation
            if (voucher.DiscountType == 1 && (voucher.DiscountValue < 1 || voucher.DiscountValue > 100))
            {
                ModelState.AddModelError("DiscountValue", "Giảm theo phần trăm phải từ 1 đến 100.");
            }
            if (voucher.DiscountType == 2 && voucher.DiscountValue <= 0)
            {
                ModelState.AddModelError("DiscountValue", "Số tiền giảm phải lớn hơn 0.");
            }
            if (voucher.MinOrderValue < 0)
            {
                ModelState.AddModelError("MinOrderValue", "Giá trị đơn tối thiểu không được âm.");
            }
            if (voucher.DiscountType == 1 && voucher.MaxDiscountAmount < 0)
            {
                ModelState.AddModelError("MaxDiscountAmount", "Số tiền giảm tối đa không được âm.");
            }

            if (ModelState.IsValid)
            {
                // Check for duplicate code
                if (await _context.Vouchers.AnyAsync(v => v.Code == voucher.Code))
                {
                    ModelState.AddModelError("Code", "Mã giảm giá này đã tồn tại.");
                    return View(voucher);
                }

                _context.Add(voucher);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Tạo mã giảm giá thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(voucher);
        }

        // GET: Voucher/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }
            return View(voucher);
        }

        // POST: Voucher/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DiscountType,DiscountValue,MaxDiscountAmount,MinOrderValue,ExpiryDate,IsActive")] Voucher voucher)
        {
            if (id != voucher.Id)
            {
                return NotFound();
            }

            // Custom Validation
            if (voucher.DiscountType == 1 && (voucher.DiscountValue < 1 || voucher.DiscountValue > 100))
            {
                ModelState.AddModelError("DiscountValue", "Giảm theo phần trăm phải từ 1 đến 100.");
            }
            if (voucher.DiscountType == 2 && voucher.DiscountValue <= 0)
            {
                ModelState.AddModelError("DiscountValue", "Số tiền giảm phải lớn hơn 0.");
            }
            if (voucher.MinOrderValue < 0)
            {
                ModelState.AddModelError("MinOrderValue", "Giá trị đơn tối thiểu không được âm.");
            }
            if (voucher.DiscountType == 1 && voucher.MaxDiscountAmount < 0)
            {
                ModelState.AddModelError("MaxDiscountAmount", "Số tiền giảm tối đa không được âm.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check for duplicate code (excluding current)
                    if (await _context.Vouchers.AnyAsync(v => v.Code == voucher.Code && v.Id != voucher.Id))
                    {
                        ModelState.AddModelError("Code", "Mã giảm giá này đã tồn tại.");
                        return View(voucher);
                    }

                    _context.Update(voucher);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật mã giảm giá thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherExists(voucher.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(voucher);
        }

        // GET: Voucher/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // POST: Voucher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher != null)
            {
                _context.Vouchers.Remove(voucher);
                TempData["Success"] = "Đã xóa mã giảm giá!";
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoucherExists(int id)
        {
            return _context.Vouchers.Any(e => e.Id == id);
        }
    }
}
