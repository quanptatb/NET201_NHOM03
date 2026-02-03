using WebBanThucAnNhanh.Models;
using WebBanThucAnNhanh.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Đường dẫn khi chưa đăng nhập
        options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn khi không đúng quyền
    });

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();