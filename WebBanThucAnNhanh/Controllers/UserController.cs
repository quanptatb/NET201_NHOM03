using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;
using System.Security.Cryptography;
using System.Text;

namespace WebBanThucAnNhanh.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // --- ĐĂNG KÝ ---
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                var check = await _context.Users.FirstOrDefaultAsync(s => s.Email == user.Email || s.Username == user.Username);
                if (check != null)
                {
                    ModelState.AddModelError("", "Email hoặc Username đã tồn tại.");
                    return View(user);
                }
                user.Password = GetMD5(user.Password);
                user.Role = "Customer"; // Mặc định là khách hàng
                user.Status = true;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }

        // --- ĐĂNG NHẬP ---
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var passHash = GetMD5(password);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == passHash);

            if (user != null)
            {
                if (!user.Status) {
                    ModelState.AddModelError("", "Tài khoản đã bị khóa.");
                    return View();
                }
                
                // Tạo Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("FullName", user.FullName ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu.");
            return View();
        }

        // --- ĐĂNG XUẤT ---
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // --- PROFILE ---
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var user = await _context.Users.FindAsync(userId);
            return View(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(User user)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var userInDb = await _context.Users.FindAsync(userId);

            if (userInDb != null)
            {
                userInDb.FullName = user.FullName;
                userInDb.PhoneNumber = user.PhoneNumber;
                userInDb.Address = user.Address;
                
                _context.Update(userInDb);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Cập nhật thành công!";
                return RedirectToAction("Profile");
            }
            return View("Profile", user);
        }

        public static string GetMD5(string str)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] fromData = Encoding.UTF8.GetBytes(str);
                byte[] targetData = md5.ComputeHash(fromData);
                StringBuilder byte2String = new StringBuilder();
                for (int i = 0; i < targetData.Length; i++)
                    byte2String.Append(targetData[i].ToString("x2"));
                return byte2String.ToString();
            }
        }
    }
}