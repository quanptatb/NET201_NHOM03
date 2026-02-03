using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace WebBanThucAnNhanh.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {

        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }
        // 1. Mở trang Đăng Ký
        // URL: /Customer/Register
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Password,Email,FullName,PhoneNumber,Address,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra Email hoặc Username đã tồn tại chưa
                var check = await _context.Users.FirstOrDefaultAsync(s => s.Email == user.Email || s.Username == user.Username);
                if (check != null)
                {
                    ModelState.AddModelError("", "Email hoặc Username đã tồn tại.");
                    return View(user);
                }

                // Mã hóa mật khẩu
                user.Password = GetMD5(user.Password);

                user.Status = true;
                if (string.IsNullOrEmpty(user.Role))
                {
                    user.Role = "Customer";
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }
        // GET: User/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password) // Hoặc dùng Model LoginViewModel
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            // Mã hóa password nhập vào để so sánh với DB (vì Register bạn dùng GetMD5)
            string f_password = GetMD5(password);

            // Tìm user trong DB
            var user = await _context.Users.FirstOrDefaultAsync(s => (s.Username == username || s.Email == username) && s.Password == f_password);

            if (user != null)
            {
                // Kiểm tra trạng thái hoạt động
                if (user.Status == false)
                {
                    ViewBag.Error = "Tài khoản đang bị khóa.";
                    return View();
                }

                // Tạo danh sách quyền (Claims)
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role), // Quan trọng: Lấy Role từ DB (Admin/Customer)
            new Claim("FullName", user.FullName ?? "") // Lưu thêm thông tin phụ nếu cần
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties(); // Có thể cấu hình Remember Me tại đây

                // Ghi Cookie xác thực
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Lưu Session hiển thị tên (giữ logic cũ của bạn để hiển thị trên Layout)
                HttpContext.Session.SetString("UserName", user.Username);

                // Điều hướng dựa trên Role
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "User", new { area = "Admin" }); // Hoặc Controller Admin cụ thể
                                                                                      // Ví dụ: return RedirectToAction("Index", "User"); // Nếu Controller User nằm chung
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

        // Action trang từ chối truy cập (nếu User cố vào trang Admin)
        public IActionResult AccessDenied()
        {
            return View(); //thông báo "Bạn không có quyền truy cập"
        }
        // LOGOUT
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
        public IActionResult Profile()
        {
            return View();
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        /// <summary>
        /// Hàm mã hóa chuỗi sang MD5
        /// </summary>
        public static string GetMD5(string str)
        {
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