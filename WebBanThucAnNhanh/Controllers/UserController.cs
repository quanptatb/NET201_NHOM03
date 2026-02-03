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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; // Để lấy ID người đang đăng nhập

namespace WebBanThucAnNhanh.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            // 1. Kiểm tra trùng lặp
            if (_context.Users.Any(u => u.Username == user.Username || u.Email == user.Email))
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc Email đã tồn tại!");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                // Mã hóa mật khẩu
                user.Password = GetMD5(user.Password);
                // Mặc định Status là true nếu chưa set
                user.Status = true;

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id) return NotFound();

            // Loại bỏ validate Password vì nếu không đổi pass thì ô này để trống
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                try
                {
                    // 2. Lấy thông tin cũ từ Database (dùng AsNoTracking để không bị lock)
                    var existingUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

                    if (existingUser != null)
                    {
                        // Logic xử lý Password thông minh:
                        if (!string.IsNullOrEmpty(user.Password))
                        {
                            // Nếu Admin nhập pass mới -> Mã hóa pass mới
                            user.Password = GetMD5(user.Password);
                        }
                        else
                        {
                            // Nếu để trống -> Giữ nguyên pass cũ
                            user.Password = existingUser.Password;
                        }
                    }

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 3. Ngăn chặn tự xóa chính mình
            var currentUserId = User.FindFirst("UserId")?.Value;
            if (currentUserId != null && int.Parse(currentUserId) == id)
            {
                // Báo lỗi ra view (cần hiển thị ViewBag.Error bên View Delete)
                ViewBag.Error = "Bạn không thể xóa chính tài khoản đang đăng nhập!";
                var selfUser = await _context.Users.FindAsync(id);
                return View(selfUser);
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                // Kiểm tra ràng buộc (nếu User đã có đơn hàng thì nên Soft Delete - Khóa tài khoản thay vì Xóa vĩnh viễn)
                bool hasOrders = await _context.Orders.AnyAsync(o => o.UserId == id);
                if (hasOrders)
                {
                    ViewBag.Error = "User này đã có đơn hàng, không thể xóa. Hãy chuyển trạng thái sang Vô hiệu hóa (Status = false).";
                    return View(user);
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

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