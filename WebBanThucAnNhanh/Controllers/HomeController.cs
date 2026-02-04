using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;

namespace WebBanThucAnNhanh.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string keyword, int? themeId)
    {
        var query = _context.FastFoods
            .Include(f => f.TypeOfFastFood)
            .Include(f => f.Theme)
            .AsQueryable();

        // Thực hiện lọc nếu có tham số
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(f => f.NameFastFood.Contains(keyword) || f.Description.Contains(keyword));
        }

        if (themeId.HasValue)
        {
            query = query.Where(f => f.IdTheme == themeId.Value);
        }

        ViewBag.Themes = await _context.Themes.ToListAsync();
        var allFoods = await query.ToListAsync();

        return View(allFoods);
    }

    // Logic tìm kiếm của bạn RẤT TỐT (kết hợp keyword + theme + khoảng giá)
    public async Task<IActionResult> Search(string keyword, int? themeId, decimal? minPrice, decimal? maxPrice)
    {
        var query = _context.FastFoods
            .Include(f => f.TypeOfFastFood)
            .Include(f => f.Theme)
            .AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            // Tìm theo Tên món HOẶC Mô tả HOẶC Tên loại
            query = query.Where(f => f.NameFastFood.Contains(keyword)
                                  || f.Description.Contains(keyword)
                                  || f.TypeOfFastFood.NameTypeOfFastFood.Contains(keyword));
        }

        if (themeId.HasValue)
        {
            query = query.Where(f => f.IdTheme == themeId.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(f => f.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(f => f.Price <= maxPrice.Value);
        }

        // Đổ lại dữ liệu ra View để giữ trạng thái filter (người dùng biết mình đang tìm gì)
        ViewBag.Themes = await _context.Themes.ToListAsync();
        ViewBag.Keyword = keyword;
        ViewBag.CurrentTheme = themeId;
        ViewBag.MinPrice = minPrice;
        ViewBag.MaxPrice = maxPrice;

        var result = await query.ToListAsync();

        // Đảm bảo bạn có file Views/Home/Search.cshtml
        return View("Search", result);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}