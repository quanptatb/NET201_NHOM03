using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication.Google;


namespace WebBanThucAnNhanh.Controllers
{
    // 1. XÓA [Authorize(Roles = "Admin")] Ở ĐÂY ĐỂ KHÁCH CÓ THỂ TRUY CẬP
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // ... Các hàm Register giữ nguyên ...
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Password,Email,FullName,PhoneNumber,Address,Role")] User user)
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
                user.Status = true;

                // Logic gán Role mặc định
                if (string.IsNullOrEmpty(user.Role)) user.Role = "Customer";

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }

        // ... Hàm Login GET giữ nguyên ...
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            string f_password = GetMD5(password);
            var user = await _context.Users.FirstOrDefaultAsync(s => (s.Username == username || s.Email == username) && s.Password == f_password);

            if (user != null)
            {
                if (user.Status == false)
                {
                    ViewBag.Error = "Tài khoản đang bị khóa.";
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("FullName", user.FullName ?? ""),
                    new Claim("UserId", user.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                HttpContext.Session.SetString("UserName", user.Username);

                if (user.Role == "Admin")
                {
                    // Đảm bảo bạn đã cấu hình Area trong Program.cs
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                    // Lưu ý: Thường Admin sẽ về Home/Index của Admin hoặc Dashboard
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu";
                return View();
            }
        }

        // 2. CHỈ BẢO VỆ CÁC TRANG CẦN ĐĂNG NHẬP
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("UserName"); // Xóa session hiển thị tên
            // 3. SỬA LẠI REDIRECT VỀ Account/Login
            return RedirectToAction("Login", "Account");
        }

        // ... Các hàm khác giữ nguyên ...
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult LoginGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Action nhận phản hồi từ Google
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded) return RedirectToAction("Login");

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            // Logic: Kiểm tra email này đã có trong DB chưa
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                // Y4: Nếu chưa có thì tự động đăng ký (hoặc chuyển trang đăng ký)
                user = new User
                {
                    Email = email,
                    Username = email, // Dùng email làm user tạm
                    FullName = name,
                    Password = "GoogleLoginDefault", // Set pass ngẫu nhiên hoặc cờ đánh dấu
                    Role = "Customer",
                    Status = true
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // Lưu Session đăng nhập hệ thống của bạn
            HttpContext.Session.SetString("UserName", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("Index", "Home");
        }

        public static string GetMD5(string str)
        {
            // ... (Giữ nguyên code MD5 của bạn) ...
            using (MD5 md5 = MD5.Create())
            {
                byte[] fromData = Encoding.UTF8.GetBytes(str);
                byte[] targetData = md5.ComputeHash(fromData);
                StringBuilder byte2String = new StringBuilder();
                for (int i = 0; i < targetData.Length; i++)
                {
                    byte2String.Append(targetData[i].ToString("x2"));
                }
                return byte2String.ToString();
            }
        }
    }
}