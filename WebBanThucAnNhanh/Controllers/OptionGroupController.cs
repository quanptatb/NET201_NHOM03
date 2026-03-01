using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;

using Microsoft.AspNetCore.Authorization;

namespace WebBanThucAnNhanh.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OptionGroupController : Controller
    {
        private readonly AppDbContext _context;

        public OptionGroupController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Hiển thị danh sách Topping/Size
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách nhóm tuỳ chọn và kèm theo các tuỳ chọn con của nó
            var optionGroups = await _context.OptionGroups
                .Include(o => o.OptionItems)
                .ToListAsync();
            return View(optionGroups);
        }

        // 2. Thêm mới Nhóm tùy chọn (VD: "Size", "Topping", "Mức Đá")
        public IActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup(OptionGroup optionGroup)
        {
            ModelState.Remove("OptionItems");
            if (ModelState.IsValid)
            {
                _context.Add(optionGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(optionGroup);
        }

        // 3. Thêm mới Tùy chọn con vào Nhóm (VD: Thêm "Size L" vào nhóm "Size")
        public IActionResult CreateItem(int groupId)
        {
            ViewBag.GroupId = groupId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem(OptionItem optionItem)
        {
            ModelState.Remove("OptionGroup");
            if (ModelState.IsValid)
            {
                _context.Add(optionItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.GroupId = optionItem.OptionGroupId;
            return View(optionItem);
        }

        // ==========================================
        // THÊM CHỨC NĂNG SỬA (UPDATE)
        // ==========================================

        // 6. Sửa Nhóm tùy chọn (Group)
        public async Task<IActionResult> EditGroup(int id)
        {
            var group = await _context.OptionGroups.FindAsync(id);
            if (group == null) return NotFound();
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(int id, OptionGroup optionGroup)
        {
            if (id != optionGroup.Id) return NotFound();

            ModelState.Remove("OptionItems");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(optionGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.OptionGroups.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(optionGroup);
        }

        // 7. Sửa Tùy chọn con (Item)
        public async Task<IActionResult> EditItem(int id)
        {
            var item = await _context.OptionItems.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(int id, OptionItem optionItem)
        {
            if (id != optionItem.Id) return NotFound();

            ModelState.Remove("OptionGroup");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(optionItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.OptionItems.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(optionItem);
        }

        // 4. Xóa tùy chọn con
        [HttpPost]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.OptionItems.FindAsync(id);
            if (item != null)
            {
                _context.OptionItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // 5. Xóa nhóm tùy chọn
        [HttpPost]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = await _context.OptionGroups.Include(g => g.OptionItems).FirstOrDefaultAsync(g => g.Id == id);
            if (group != null)
            {
                _context.OptionGroups.Remove(group);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Menu()
        {
            // Lấy danh sách để hiển thị cho khách
            var optionGroups = await _context.OptionGroups
                .Include(o => o.OptionItems)
                .ToListAsync();
            return View(optionGroups);
        }
    }
}