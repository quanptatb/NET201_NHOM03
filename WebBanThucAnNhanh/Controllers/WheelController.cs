using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using WebBanThucAnNhanh.Hubs;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;
using Newtonsoft.Json;

namespace WebBanThucAnNhanh.Controllers
{
    [Authorize]
    public class WheelController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<WheelHub> _wheelHub;

        public WheelController(AppDbContext context, IHubContext<WheelHub> wheelHub)
        {
            _context = context;
            _wheelHub = wheelHub;
        }

        // GET: /Wheel — Trang vòng quay
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);
            var currentUser = await _context.Users.FindAsync(userId);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            // Lấy giải thưởng active VÀ món ăn liên kết còn hàng (hoặc không có liên kết - VD: 'Chúc may mắn')
            var drinkPrizes = await _context.WheelPrizes
                .Where(p => p.PrizeType == 1 && p.IsActive)
                .Include(p => p.FastFood)
                .Where(p => p.FastFoodId == null || (p.FastFood.Quantity > 0 && p.FastFood.Status))
                .ToListAsync();

            var foodPrizes = await _context.WheelPrizes
                .Where(p => p.PrizeType == 2 && p.IsActive)
                .Include(p => p.FastFood)
                .Where(p => p.FastFoodId == null || (p.FastFood.Quantity > 0 && p.FastFood.Status))
                .ToListAsync();

            ViewBag.DrinkSpins = currentUser.DrinkSpins;
            ViewBag.FoodSpins = currentUser.FoodSpins;
            ViewBag.DrinkPrizes = drinkPrizes;
            ViewBag.FoodPrizes = foodPrizes;

            return View();
        }

        // POST: /Wheel/Spin — API quay thưởng (trả JSON)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("SpinLimit")]
        public async Task<IActionResult> Spin([FromBody] SpinRequest request)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập." });

            int userId = int.Parse(userIdClaim.Value);
            var currentUser = await _context.Users.FindAsync(userId);
            if (currentUser == null)
                return Json(new { success = false, message = "Không tìm thấy người dùng." });

            int spinType = request.SpinType; // 1: Nước, 2: Đồ ăn

            // === 1. Kiểm tra lượt quay ===
            if (spinType == 1 && currentUser.DrinkSpins <= 0)
                return Json(new { success = false, message = "Bạn không còn lượt quay Nước nào!" });

            if (spinType == 2 && currentUser.FoodSpins <= 0)
                return Json(new { success = false, message = "Bạn không còn lượt quay Đồ ăn nào!" });

            // === 2. Trừ 1 lượt quay ===
            if (spinType == 1)
                currentUser.DrinkSpins -= 1;
            else
                currentUser.FoodSpins -= 1;

            // === 3. Lấy giải thưởng hợp lệ (còn số lượng VÀ món ăn liên kết còn hàng) ===
            var availablePrizes = await _context.WheelPrizes
                .Include(p => p.FastFood)
                .Where(p => p.PrizeType == spinType && p.IsActive && p.RemainingQuantity > 0)
                .Where(p => p.FastFoodId == null || (p.FastFood.Quantity > 0 && p.FastFood.Status))
                .ToListAsync();

            if (!availablePrizes.Any())
            {
                await _context.SaveChangesAsync(); // Vẫn trừ lượt quay
                return Json(new { success = true, wonPrizeId = 0, hasRealPrize = false, prizeName = "Chúc bạn may mắn lần sau!", drinkSpins = currentUser.DrinkSpins, foodSpins = currentUser.FoodSpins });
            }

            // === 4. Random giải thưởng dựa trên Probability ===
            var wonPrize = RandomPrizeByProbability(availablePrizes);

            if (wonPrize == null)
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true, wonPrizeId = 0, hasRealPrize = false, prizeName = "Chúc bạn may mắn lần sau!", drinkSpins = currentUser.DrinkSpins, foodSpins = currentUser.FoodSpins });
            }

            // === 5. Trừ số lượng giải và đồng bộ kho FastFood ===
            wonPrize.RemainingQuantity -= 1;

            // Trừ kho FastFood liên kết
            if (wonPrize.FastFood != null)
            {
                wonPrize.FastFood.Quantity -= 1;
                if (wonPrize.FastFood.Quantity <= 0)
                {
                    wonPrize.FastFood.Quantity = 0;
                    wonPrize.FastFood.Status = false; // Hết hàng
                    wonPrize.IsActive = false;         // Tắt giải trong vòng quay
                }
            }

            var reward = new UserReward
            {
                UserId = userId,
                PrizeId = wonPrize.Id,
                IsUsed = false,
                DateWon = DateTime.Now
            };
            _context.UserRewards.Add(reward);
            await _context.SaveChangesAsync();

            // === SIGNALR: Broadcast thông báo real-time khi trúng giải thật ===
            if (wonPrize.FastFoodId != null)
            {
                string displayName = User.Identity?.Name ?? "Khách hàng";
                await _wheelHub.Clients.All.SendAsync("ReceivePrizeNotification",
                    displayName, wonPrize.PrizeName);
            }

            // Trả về wonPrizeId để frontend tìm đúng ô (kể cả đã shuffle/sort)
            return Json(new
            {
                success = true,
                wonPrizeId = wonPrize.Id,
                hasRealPrize = wonPrize.FastFoodId != null, // false = "Chúc may mắn"
                prizeName = wonPrize.PrizeName,
                drinkSpins = currentUser.DrinkSpins,
                foodSpins = currentUser.FoodSpins
            });
        }

        // GET: /Wheel/MyRewards — Xem quà đã trúng
        public async Task<IActionResult> MyRewards()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);

            var rewards = await _context.UserRewards
                .Include(r => r.WheelPrize)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.DateWon)
                .ToListAsync();

            return View(rewards);
        }

        // POST: /Wheel/RedeemReward — Sử dụng quà (thêm vào giỏ hàng giá 0đ)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RedeemReward(int rewardId)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);

            // 1. Tìm UserReward
            var reward = await _context.UserRewards
                .Include(r => r.WheelPrize)
                    .ThenInclude(wp => wp.FastFood)
                .FirstOrDefaultAsync(r => r.Id == rewardId && r.UserId == userId && !r.IsUsed);

            if (reward == null)
            {
                TempData["Error"] = "Không tìm thấy quà hoặc quà đã được sử dụng!";
                return RedirectToAction(nameof(MyRewards));
            }

            // 1.5 Kiểm tra hạn sử dụng (3 ngày)
            if (reward.DateWon.AddDays(3) < DateTime.Now)
            {
                TempData["Error"] = "Quà đã hết hạn sử dụng (quá 3 ngày)!";
                return RedirectToAction(nameof(MyRewards));
            }

            // 2. Kiểm tra giải có liên kết với FastFood không
            if (reward.WheelPrize?.FastFoodId == null || reward.WheelPrize?.FastFood == null)
            {
                TempData["Error"] = "Giải thưởng này không có món ăn/nước liên kết!";
                return RedirectToAction(nameof(MyRewards));
            }

            var foodItem = reward.WheelPrize.FastFood;

            // 3. Thêm vào giỏ hàng với giá = 0 và đánh dấu IsReward
            // Dùng đúng key cookie theo user (giống CartController)
            var userName = User.Identity?.IsAuthenticated == true ? User.Identity.Name : "Anonymous";
            var safeUserName = Convert.ToHexString(System.Text.Encoding.UTF8.GetBytes(userName));
            var cartKey = $"Cart_{safeUserName}";

            var cookieCart = Request.Cookies[cartKey];
            List<CartItem> cart = new List<CartItem>();
            if (cookieCart != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartItem>>(cookieCart);
            }

            cart.Add(new CartItem
            {
                Id = foodItem.IdFastFood,
                Name = foodItem.NameFastFood + " (Quà tặng 🎁)",
                BasePrice = 0, // MIỄN PHÍ
                Image = foodItem.Image,
                Quantity = 1,
                IsReward = true,
                SelectedOptions = new List<CartItemOption>()
            });

            var cartJson = JsonConvert.SerializeObject(cart);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true, // Tăng cường bảo mật
                IsEssential = true
            };
            Response.Cookies.Append(cartKey, cartJson, cookieOptions);

            // 4. Đánh dấu đã sử dụng
            reward.IsUsed = true;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã thêm \"{foodItem.NameFastFood}\" miễn phí vào giỏ hàng!";
            return RedirectToAction("Index", "Cart");
        }

        // === HÀM RANDOM GIẢI THƯỞNG THEO TỶ LỆ PROBABILITY ===
        private WheelPrize? RandomPrizeByProbability(System.Collections.Generic.List<WheelPrize> prizes)
        {
            // Tính tổng tỷ lệ
            double totalProbability = prizes.Sum(p => p.Probability);
            if (totalProbability <= 0) return null;

            var random = new Random();
            double roll = random.NextDouble() * totalProbability; // Random từ 0 -> tổng

            double cumulative = 0;
            foreach (var prize in prizes)
            {
                cumulative += prize.Probability;
                if (roll <= cumulative)
                {
                    return prize;
                }
            }

            // Fallback: trả về giải cuối cùng
            return prizes.Last();
        }
    }

    // Model nhận request từ Frontend
    public class SpinRequest
    {
        public int SpinType { get; set; }
    }
}
