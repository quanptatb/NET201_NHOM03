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
using System.IO; // Thêm thư viện để xử lý file

namespace WebBanThucAnNhanh.Controllers
{
    // 1. XÓA [Authorize] Ở ĐÂY ĐỂ KHÁCH CÓ THỂ XEM MÓN ĂN
    public class FastFoodController : Controller
    {
        private readonly AppDbContext _context;
        // Khai báo môi trường để lấy đường dẫn lưu file
        private readonly IWebHostEnvironment _environment;

        public FastFoodController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: FastFood (Ai cũng xem được)
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.FastFoods.Include(f => f.Theme).Include(f => f.TypeOfFastFood);
            return View(await appDbContext.ToListAsync());
        }

        // GET: FastFood/Details/5 (Ai cũng xem được)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var fastFood = await _context.FastFoods
                .Include(f => f.Theme)
                .Include(f => f.TypeOfFastFood)
                .FirstOrDefaultAsync(m => m.IdFastFood == id);

            if (fastFood == null) return NotFound();

            return View(fastFood);
        }

        // ==========================================================
        // KHU VỰC ADMIN (Thêm [Authorize] vào từng hàm)
        // ==========================================================

        // GET: FastFood/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["IdTheme"] = new SelectList(_context.Themes, "IdTheme", "NameTheme");
            ViewData["IdTypeOfFastFood"] = new SelectList(_context.TypeOfFastFoods, "IdTypeOfFastFood", "NameTypeOfFastFood");
            return View();
        }

        // POST: FastFood/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(FastFood fastFood, IFormFile imageFile)
        {
            // LOẠI BỎ VALIDATION CHO CÁC TRƯỜNG KHÔNG GỬI TỪ VIEW
            ModelState.Remove("imageFile");
            ModelState.Remove("Image");
            ModelState.Remove("TypeOfFastFood");
            ModelState.Remove("Theme");
            ModelState.Remove("OrderDetails"); // Nếu model có thuộc tính này

            if (ModelState.IsValid)
            {
                // Xử lý Upload hình ảnh
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

                    // Đảm bảo thư mục tồn tại
                    if (!Directory.Exists(Path.Combine(_environment.WebRootPath, "images")))
                    {
                        Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, "images"));
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    fastFood.Image = fileName;
                }
                else
                {
                    fastFood.Image = "default.png";
                }

                _context.Add(fastFood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu quay lại View do lỗi, cần nạp lại SelectList
            ViewData["IdTheme"] = new SelectList(_context.Themes, "IdTheme", "NameTheme", fastFood.IdTheme);
            ViewData["IdTypeOfFastFood"] = new SelectList(_context.TypeOfFastFoods, "IdTypeOfFastFood", "NameTypeOfFastFood", fastFood.IdTypeOfFastFood);
            return View(fastFood);
        }

        // POST: FastFood/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, FastFood fastFood, IFormFile imageFile)
        {
            if (id != fastFood.IdFastFood) return NotFound();

            // LOẠI BỎ VALIDATION TƯƠNG TỰ CREATE
            ModelState.Remove("imageFile");
            ModelState.Remove("Image");
            ModelState.Remove("TypeOfFastFood");
            ModelState.Remove("Theme");
            ModelState.Remove("OrderDetails");

            if (ModelState.IsValid)
            {
                try
                {
                    var oldFood = await _context.FastFoods.AsNoTracking().FirstOrDefaultAsync(x => x.IdFastFood == id);

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        fastFood.Image = fileName;
                    }
                    else
                    {
                        fastFood.Image = oldFood.Image;
                    }

                    _context.Update(fastFood);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FastFoodExists(fastFood.IdFastFood)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTheme"] = new SelectList(_context.Themes, "IdTheme", "NameTheme", fastFood.IdTheme);
            ViewData["IdTypeOfFastFood"] = new SelectList(_context.TypeOfFastFoods, "IdTypeOfFastFood", "NameTypeOfFastFood", fastFood.IdTypeOfFastFood);
            return View(fastFood);
        }

        // GET: FastFood/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var fastFood = await _context.FastFoods
                .Include(f => f.Theme)
                .Include(f => f.TypeOfFastFood)
                .FirstOrDefaultAsync(m => m.IdFastFood == id);
            if (fastFood == null) return NotFound();

            return View(fastFood);
        }

        // POST: FastFood/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fastFood = await _context.FastFoods.FindAsync(id);
            if (fastFood != null)
            {
                _context.FastFoods.Remove(fastFood);

                // (Tùy chọn) Xóa file ảnh khỏi thư mục images để tiết kiệm dung lượng
                // if (!string.IsNullOrEmpty(fastFood.Image)) { ... logic xóa file ... }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FastFoodExists(int id)
        {
            return _context.FastFoods.Any(e => e.IdFastFood == id);
        }
    }
}