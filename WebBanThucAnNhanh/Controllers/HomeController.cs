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
        ViewBag.Types = await _context.TypeOfFastFoods.ToListAsync();
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

    // GET: Home/Details/5 - Trang chi tiết sản phẩm cho User
public async Task<IActionResult> Details(int? id)
{
    if (id == null) return NotFound();

    var fastFood = await _context.FastFoods
        .Include(f => f.Theme)
        .Include(f => f.TypeOfFastFood)
        .FirstOrDefaultAsync(m => m.IdFastFood == id);

    if (fastFood == null) return NotFound();

    // 1. LẤY DANH SÁCH TOPPING / SIZE truyền sang View
    var options = await _context.FastFoodOptionGroups
        .Where(fog => fog.FastFoodId == id)
        .Include(fog => fog.OptionGroup)
            .ThenInclude(g => g.OptionItems)
        .Select(fog => fog.OptionGroup)
        .ToListAsync();
        
    ViewBag.Options = options;

    // 2. Lấy các sản phẩm liên quan (cùng loại, trừ sản phẩm hiện tại)
    ViewBag.RelatedProducts = await _context.FastFoods
        .Include(f => f.TypeOfFastFood)
        .Where(f => f.IdTypeOfFastFood == fastFood.IdTypeOfFastFood && f.IdFastFood != id)
        .Take(4)
        .ToListAsync();

    // ==========================================
    // 3. GỢI Ý MUA KÈM ĐỘNG (THÊM VÀO ĐÂY)
    // ==========================================
    // Lấy 4 sản phẩm ngẫu nhiên để làm món mua kèm giảm giá 50%
    var randomProducts = await _context.FastFoods
        .Where(f => f.IdFastFood != id && f.Status == true)
        .OrderBy(r => Guid.NewGuid()) 
        .Take(4)
        .ToListAsync();

    var addOnItems = new List<CrossSellBundle>();
    foreach (var p in randomProducts)
    {
        addOnItems.Add(new CrossSellBundle
        {
            MainFastFoodId = fastFood.IdFastFood,
            AddOnFastFoodId = p.IdFastFood,
            AddOnFastFood = p, 
            DiscountPercentage = 50 // Giảm 50%
        });
    }
    ViewBag.AddOnItems = addOnItems;
    // ==========================================

    return View(fastFood);
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