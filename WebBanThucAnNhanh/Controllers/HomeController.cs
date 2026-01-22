using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<IActionResult> Index()
    {
        // Bây giờ biến _context đã có giá trị và dùng được
        var monAn = await _context.FastFoods
                                  .Include(f => f.TypeOfFastFood)
                                  .ToListAsync();
        return View(monAn); 
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