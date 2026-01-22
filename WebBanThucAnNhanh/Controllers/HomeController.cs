using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;

namespace WebBanThucAnNhanh.Controllers;

public class HomeController : Controller
{
    // 1. KHAI BÁO BIẾN _context
    private readonly AppDbContext _context;
    // (Tuỳ chọn) Khai báo thêm Logger nếu template cũ có dùng, nhưng ở đây bạn chưa dùng nên có thể bỏ qua
    // private readonly ILogger<HomeController> _logger; 

    // 2. TẠO CONSTRUCTOR ĐỂ GÁN GIÁ TRỊ CHO _context
    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    // Thêm tham số id (cho phép null) để nhận biết người dùng chọn loại nào
    // Thay thế hàm Index() cũ bằng hàm này
    public async Task<IActionResult> Index(string keyword, int? themeId, decimal? minPrice, decimal? maxPrice)
    {
        // 1. Tạo query và nối bảng để lấy thông tin Loại và Chủ đề
        var query = _context.FastFoods
            .Include(f => f.TypeOfFastFood)
            .Include(f => f.Theme)
            .AsQueryable();

        // 2. LOGIC TÌM KIẾM THÔNG MINH
        if (!string.IsNullOrEmpty(keyword))
        {
            // Tìm trong Tên món HOẶC Mô tả HOẶC Tên loại (Ví dụ nhập "Burger" vẫn ra)
            query = query.Where(f => f.NameFastFood.Contains(keyword)
                                  || f.Description.Contains(keyword)
                                  || f.TypeOfFastFood.NameTypeOfFastFood.Contains(keyword));
        }

        // 3. Lọc theo Chủ đề
        if (themeId.HasValue)
        {
            query = query.Where(f => f.IdTheme == themeId.Value);
        }

        // 4. Lọc theo Giá
        if (minPrice.HasValue)
        {
            query = query.Where(f => f.Price >= minPrice.Value);
        }
        if (maxPrice.HasValue)
        {
            query = query.Where(f => f.Price <= maxPrice.Value);
        }

        // 5. Chuẩn bị dữ liệu hiển thị (Chỉ cần Theme, bỏ Type)
        ViewBag.Themes = await _context.Themes.ToListAsync();

        // Lưu trạng thái để View biết là đang tìm kiếm hay không
        bool isSearch = !string.IsNullOrEmpty(keyword) || themeId.HasValue || minPrice.HasValue || maxPrice.HasValue;
        ViewBag.IsSearch = isSearch;
        ViewBag.Keyword = keyword;

        var result = await query.ToListAsync();
        return View(result);
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