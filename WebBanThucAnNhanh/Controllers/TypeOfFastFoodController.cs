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
    public class TypeOfFastFoodController : Controller
    {
        private readonly AppDbContext _context;

        public TypeOfFastFoodController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TypeOfFastFood
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypeOfFastFoods.ToListAsync());
        }

        // GET: TypeOfFastFood/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfFastFood = await _context.TypeOfFastFoods
                .FirstOrDefaultAsync(m => m.IdTypeOfFastFood == id);
            if (typeOfFastFood == null)
            {
                return NotFound();
            }

            return View(typeOfFastFood);
        }

        // GET: TypeOfFastFood/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeOfFastFood/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTypeOfFastFood,NameTypeOfFastFood")] TypeOfFastFood typeOfFastFood)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeOfFastFood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfFastFood);
        }

        // GET: TypeOfFastFood/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfFastFood = await _context.TypeOfFastFoods.FindAsync(id);
            if (typeOfFastFood == null)
            {
                return NotFound();
            }
            return View(typeOfFastFood);
        }

        // POST: TypeOfFastFood/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTypeOfFastFood,NameTypeOfFastFood")] TypeOfFastFood typeOfFastFood)
        {
            if (id != typeOfFastFood.IdTypeOfFastFood)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeOfFastFood);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfFastFoodExists(typeOfFastFood.IdTypeOfFastFood))
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
            return View(typeOfFastFood);
        }

        // GET: TypeOfFastFood/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfFastFood = await _context.TypeOfFastFoods
                .FirstOrDefaultAsync(m => m.IdTypeOfFastFood == id);
            if (typeOfFastFood == null)
            {
                return NotFound();
            }

            return View(typeOfFastFood);
        }

        // POST: TypeOfFastFood/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeOfFastFood = await _context.TypeOfFastFoods.FindAsync(id);
            if (typeOfFastFood == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // 1. KIỂM TRA AN TOÀN: Có món ăn nào đang thuộc Loại này không?
            // Kiểm tra trong bảng FastFoods xem có ai dùng IdTypeOfFastFood này không
            bool isInUse = await _context.FastFoods.AnyAsync(f => f.IdTypeOfFastFood == id);

            if (isInUse)
            {
                // Nếu đang có món ăn dùng loại này thì không cho xóa
                // Bạn cần thêm dòng này vào View Delete.cshtml để hiện lỗi: <div class="text-danger">@ViewBag.Error</div>
                ViewBag.Error = "Không thể xóa loại thức ăn này vì đang có món ăn thuộc về nó. Hãy xóa các món ăn đó trước.";
                return View("Delete", typeOfFastFood);
            }

            // 2. Nếu không có ai dùng thì mới xóa
            _context.TypeOfFastFoods.Remove(typeOfFastFood);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeOfFastFoodExists(int id)
        {
            return _context.TypeOfFastFoods.Any(e => e.IdTypeOfFastFood == id);
        }
    }
}
