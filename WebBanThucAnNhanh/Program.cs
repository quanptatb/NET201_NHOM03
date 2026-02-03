using WebBanThucAnNhanh.Models;
using WebBanThucAnNhanh.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());

// 1. THÊM DỊCH VỤ SESSION VÀ HTTPCONTEXTACCESSOR
builder.Services.AddDistributedMemoryCache(); // Cần cho Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian giữ đăng nhập (ví dụ 30 phút)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor(); // Để dùng Session trong View (_Layout)
// ============================================================

// Gộp tất cả cấu hình xác thực vào đây
builder.Services.AddAuthentication(options =>
{
    // Đặt scheme mặc định là Cookies
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // Lưu ý: Dòng dưới sẽ chuyển hướng người dùng chưa đăng nhập thẳng sang Google.
    // Nếu bạn muốn họ vào trang đăng nhập của web mình trước, hãy bỏ dòng này đi.
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    // Cấu hình cho Cookie
    options.LoginPath = "/Account/Login"; // Đường dẫn khi chưa đăng nhập
    options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn khi không đúng quyền
})
.AddGoogle(options =>
{
    // Cấu hình Google
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ============================================================
// 2. KÍCH HOẠT SESSION (ĐẶT TRƯỚC MapControllerRoute)
// ============================================================
app.UseSession();
// ============================================================

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();