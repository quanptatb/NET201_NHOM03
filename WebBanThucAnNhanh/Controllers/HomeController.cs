using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;

namespace WebBanThucAnNhanh.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            var fastFoods = _context.FastFoods.Include(f => f.TypeOfFastFood).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                fastFoods = fastFoods.Where(s => s.NameFastFood.Contains(searchString));
            }

            if (categoryId.HasValue)
            {
                fastFoods = fastFoods.Where(x => x.IdTypeOfFastFood == categoryId);
            }

            ViewBag.Categories = await _context.TypeOfFastFoods.ToListAsync();
            
            ViewBag.CurrentFilter = searchString;

            return View(await fastFoods.ToListAsync());
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
}