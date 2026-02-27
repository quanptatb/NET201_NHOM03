using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;

namespace WebBanThucAnNhanh.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            // Fetch all orders to compute stats
            // To optimize, we can just fetch the required fields or filter directly in the DB
            var orders = await _context.Orders.ToListAsync();

            // 1. Doanh thu ngày (Today's Revenue): Status != 3 (Hủy) và DateCreated là hôm nay
            var dailyRevenue = orders
                .Where(o => o.DateCreated.Date == today && o.Status != 3)
                .Sum(o => o.TotalPrice);

            // 2. Doanh thu tháng (Monthly Revenue): Status != 3, trong tháng hiện tại
            var monthlyRevenue = orders
                .Where(o => o.DateCreated.Date >= startOfMonth && o.DateCreated.Date <= endOfMonth && o.Status != 3)
                .Sum(o => o.TotalPrice);

            // 3. Số đơn mới (New Orders): Status == 0
            var newOrdersCount = orders.Count(o => o.Status == 0);

            // 4. Tỉ lệ đơn thành công/hủy (Success/Cancel Ratio): Status == 2 vs Status == 3
            var successOrdersCount = orders.Count(o => o.Status == 2);
            var cancelledOrdersCount = orders.Count(o => o.Status == 3);
            var totalProcessed = successOrdersCount + cancelledOrdersCount;
            
            double successRate = 0;
            if (totalProcessed > 0)
            {
                successRate = (double)successOrdersCount / totalProcessed * 100;
            }

            ViewBag.DailyRevenue = dailyRevenue;
            ViewBag.MonthlyRevenue = monthlyRevenue;
            ViewBag.NewOrdersCount = newOrdersCount;
            ViewBag.SuccessRate = Math.Round(successRate, 1);
            ViewBag.SuccessOrdersCount = successOrdersCount;
            ViewBag.CancelledOrdersCount = cancelledOrdersCount;

            return View();
        }
    }
}
