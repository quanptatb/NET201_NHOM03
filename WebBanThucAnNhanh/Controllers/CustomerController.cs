using Microsoft.AspNetCore.Mvc;

namespace WebBanThucAnNhanh.Controllers
{
    public class CustomerController : Controller
    {
        // 1. Mở trang Đăng Ký
        // URL: /Customer/Register
        public IActionResult Register()
        {
            return View();
        }

        // 2. Mở trang Đăng Nhập
        // URL: /Customer/Login
        public IActionResult Login()
        {
            return View();
        }

        // 3. Mở trang Profile (Hồ sơ) - Làm sẵn để sau này dùng
        // URL: /Customer/Profile
        public IActionResult Profile()
        {
            return View();
        }
        
        // 4. Đăng xuất (Tạm thời chỉ cần quay về trang chủ)
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}