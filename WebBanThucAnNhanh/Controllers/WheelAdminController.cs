using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;

namespace WebBanThucAnNhanh.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WheelAdminController : Controller
    {
        private readonly AppDbContext _context;

        public WheelAdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /WheelAdmin — Danh sách giải thưởng
        public async Task<IActionResult> Index()
        {
            var prizes = await _context.WheelPrizes
                .Include(p => p.FastFood)
                .OrderBy(p => p.PrizeType)
                .ThenBy(p => p.Id)
                .ToListAsync();

            return View(prizes);
        }

        // GET: /WheelAdmin/Create — Form thêm giải thưởng
        public IActionResult Create()
        {
            // SelectList cho Loại giải
            ViewBag.PrizeTypeList = new SelectList(new[]
            {
                new { Value = 1, Text = "Nước (Đồ uống)" },
                new { Value = 2, Text = "Đồ ăn" }
            }, "Value", "Text");

            // SelectList cho món ăn/nước liên kết
            ViewBag.FastFoodList = new SelectList(
                _context.FastFoods.Where(f => f.Status).OrderBy(f => f.NameFastFood),
                "IdFastFood", "NameFastFood");

            return View();
        }

        // POST: /WheelAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WheelPrize prize)
        {
            ModelState.Remove("FastFood");

            if (prize.FastFoodId.HasValue)
            {
                var food = await _context.FastFoods.FindAsync(prize.FastFoodId.Value);
                if (food != null) prize.RemainingQuantity = food.Quantity;
            }

            if (ModelState.IsValid)
            {
                _context.WheelPrizes.Add(prize);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nạp lại SelectList nếu có lỗi
            ViewBag.PrizeTypeList = new SelectList(new[]
            {
                new { Value = 1, Text = "Nước (Đồ uống)" },
                new { Value = 2, Text = "Đồ ăn" }
            }, "Value", "Text", prize.PrizeType);

            ViewBag.FastFoodList = new SelectList(
                _context.FastFoods.Where(f => f.Status).OrderBy(f => f.NameFastFood),
                "IdFastFood", "NameFastFood", prize.FastFoodId);

            return View(prize);
        }

        // GET: /WheelAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var prize = await _context.WheelPrizes.FindAsync(id);
            if (prize == null) return NotFound();

            ViewBag.PrizeTypeList = new SelectList(new[]
            {
                new { Value = 1, Text = "Nước (Đồ uống)" },
                new { Value = 2, Text = "Đồ ăn" }
            }, "Value", "Text", prize.PrizeType);

            ViewBag.FastFoodList = new SelectList(
                _context.FastFoods.Where(f => f.Status).OrderBy(f => f.NameFastFood),
                "IdFastFood", "NameFastFood", prize.FastFoodId);

            return View(prize);
        }

        // POST: /WheelAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WheelPrize prize)
        {
            if (id != prize.Id) return NotFound();

            ModelState.Remove("FastFood");

            if (prize.FastFoodId.HasValue)
            {
                var food = await _context.FastFoods.FindAsync(prize.FastFoodId.Value);
                if (food != null) prize.RemainingQuantity = food.Quantity;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prize);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.WheelPrizes.Any(e => e.Id == prize.Id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PrizeTypeList = new SelectList(new[]
            {
                new { Value = 1, Text = "Nước (Đồ uống)" },
                new { Value = 2, Text = "Đồ ăn" }
            }, "Value", "Text", prize.PrizeType);

            ViewBag.FastFoodList = new SelectList(
                _context.FastFoods.Where(f => f.Status).OrderBy(f => f.NameFastFood),
                "IdFastFood", "NameFastFood", prize.FastFoodId);

            return View(prize);
        }

        // GET: /WheelAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var prize = await _context.WheelPrizes
                .Include(p => p.FastFood)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prize == null) return NotFound();

            return View(prize);
        }

        // POST: /WheelAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prize = await _context.WheelPrizes.FindAsync(id);
            if (prize != null)
            {
                _context.WheelPrizes.Remove(prize);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: /WheelAdmin/ToggleActive/5 — Bật/Tắt nhanh giải thưởng
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var prize = await _context.WheelPrizes.FindAsync(id);
            if (prize != null)
            {
                prize.IsActive = !prize.IsActive;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: /WheelAdmin/GenerateSampleData
        [HttpPost]
        public async Task<IActionResult> GenerateSampleData()
        {
            try
            {
                // Kiểm tra nếu TypeOfFastFood và Theme chưa có, tạo mặc định
                if (!_context.TypeOfFastFoods.Any())
                {
                    _context.TypeOfFastFoods.Add(new TypeOfFastFood { NameTypeOfFastFood = "Đồ ăn nhanh" });
                    _context.TypeOfFastFoods.Add(new TypeOfFastFood { NameTypeOfFastFood = "Đồ uống" });
                    await _context.SaveChangesAsync();
                }

                if (!_context.Themes.Any())
                {
                    _context.Themes.Add(new Theme { NameTheme = "Bữa sáng" });
                    _context.Themes.Add(new Theme { NameTheme = "Bữa trưa" });
                    await _context.SaveChangesAsync();
                }

                int typeFoodId = _context.TypeOfFastFoods.First(t => t.NameTypeOfFastFood == "Đồ ăn nhanh").IdTypeOfFastFood;
                int typeDrinkId = _context.TypeOfFastFoods.First(t => t.NameTypeOfFastFood == "Đồ uống").IdTypeOfFastFood;
                int themeId = _context.Themes.First().IdTheme;

                // Tạo 3 Món Ăn Mẫu
                var sampleFoods = new List<FastFood>
                {
                    new FastFood { NameFastFood = "Gà Rán (Sample)", Price = 35000, Quantity = 50, Status = true, IdTypeOfFastFood = typeFoodId, IdTheme = themeId },
                    new FastFood { NameFastFood = "Burger (Sample)", Price = 45000, Quantity = 40, Status = true, IdTypeOfFastFood = typeFoodId, IdTheme = themeId },
                    new FastFood { NameFastFood = "CocaCola (Sample)", Price = 15000, Quantity = 100, Status = true, IdTypeOfFastFood = typeDrinkId, IdTheme = themeId }
                };

                foreach (var food in sampleFoods)
                {
                    if (!_context.FastFoods.Any(f => f.NameFastFood == food.NameFastFood))
                    {
                        _context.FastFoods.Add(food);
                    }
                }
                await _context.SaveChangesAsync(); // Cần lưu để lấy ID

                // Tạo 3 giải thưởng vòng quay liên kết
                foreach (var food in sampleFoods)
                {
                    if (!_context.WheelPrizes.Any(p => p.PrizeName == food.NameFastFood + " Miễn Phí" && p.FastFoodId == food.IdFastFood))
                    {
                        _context.WheelPrizes.Add(new WheelPrize
                        {
                            PrizeName = food.NameFastFood + " Miễn Phí",
                            PrizeType = food.IdTypeOfFastFood == typeDrinkId ? 1 : 2,
                            FastFoodId = food.IdFastFood,
                            Probability = 10,
                            RemainingQuantity = food.Quantity,
                            IsActive = true
                        });
                    }
                }
                
                await _context.SaveChangesAsync();
                TempData["Success"] = "Quá trình tạo dữ liệu mẫu hoàn tất!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
