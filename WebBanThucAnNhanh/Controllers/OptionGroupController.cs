using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;

namespace WebBanThucAnNhanh.Controllers
{
    // Bạn có thể thêm [Authorize(Roles = "Admin")] để bảo mật trang này
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
            if (ModelState.IsValid)
            {
                _context.Add(optionItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.GroupId = optionItem.OptionGroupId;
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
    }
}