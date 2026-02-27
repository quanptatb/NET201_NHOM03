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

            // 5. Chart Data: Revenue for the last 7 days including today
            var last7Days = Enumerable.Range(0, 7).Select(i => today.AddDays(-i)).Reverse().ToList();
            var chartLabels = last7Days.Select(d => d.ToString("dd/MM")).ToList();
            var chartData = new List<decimal>();

            foreach (var d in last7Days)
            {
                var dailyTotal = orders
                    .Where(o => o.DateCreated.Date == d && o.Status != 3)
                    .Sum(o => o.TotalPrice);
                chartData.Add(dailyTotal);
            }

            ViewBag.ChartLabels = System.Text.Json.JsonSerializer.Serialize(chartLabels);
            ViewBag.ChartData = System.Text.Json.JsonSerializer.Serialize(chartData);

            ViewBag.DailyRevenue = dailyRevenue;
            ViewBag.MonthlyRevenue = monthlyRevenue;
            ViewBag.NewOrdersCount = newOrdersCount;
            ViewBag.SuccessRate = Math.Round(successRate, 1);
            ViewBag.SuccessOrdersCount = successOrdersCount;
            ViewBag.DailyRevenue = dailyRevenue;
            ViewBag.MonthlyRevenue = monthlyRevenue;
            ViewBag.NewOrdersCount = newOrdersCount;
            ViewBag.SuccessRate = Math.Round(successRate, 1);
            ViewBag.SuccessOrdersCount = successOrdersCount;
            ViewBag.CancelledOrdersCount = cancelledOrdersCount;

            // 6. Top Products (By Quantity Sold, ignoring cancelled orders)
            // Lấy danh sách OrderId của các đơn không bị hủy
            var validOrderIds = orders.Where(o => o.Status != 3).Select(o => o.IdOrder).ToList();
            
            // Query các OrderDetail thuộc các đơn hàng hợp lệ, group theo mòn ăn
            var topProducts = await _context.OrderDetails
                .Where(od => validOrderIds.Contains(od.OrderId) && od.FastFoodId != null)
                .GroupBy(od => new { od.FastFoodId, od.FastFood.NameFastFood, od.FastFood.Image })
                .Select(g => new
                {
                    FastFoodId = g.Key.FastFoodId,
                    Name = g.Key.NameFastFood,
                    Image = g.Key.Image,
                    TotalQuantity = g.Sum(od => od.Quantity),
                    TotalRevenue = g.Sum(od => od.Quantity * od.Price)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(5)
                .ToListAsync();

            ViewBag.TopProducts = topProducts;

            return View();
        }
    }
}
