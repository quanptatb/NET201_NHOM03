using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Models;

namespace WebBanThucAnNhanh.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TypeOfFastFood> TypeOfFastFoods { get; set; }
        public DbSet<FastFood> FastFoods { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Theme> Themes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SEED DATA (Y1)
            // 1. Tạo loại món ăn mẫu
            modelBuilder.Entity<TypeOfFastFood>().HasData(
                new TypeOfFastFood { IdTypeOfFastFood = 1, NameTypeOfFastFood = "Đồ ăn nhanh" },
                new TypeOfFastFood { IdTypeOfFastFood = 2, NameTypeOfFastFood = "Đồ uống" }
            );

            // 2. Tạo chủ đề mẫu
            modelBuilder.Entity<Theme>().HasData(
                new Theme { IdTheme = 1, NameTheme = "Bữa sáng" },
                new Theme { IdTheme = 2, NameTheme = "Tiệc tùng" }
            );

            // 3. Tạo tài khoản Admin mặc định
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "202cb962ac59075b964b07152d234b70", // Mã hóa MD5 của "123"
                    FullName = "Administrator",
                    Email = "admin@fastfood.com",
                    Role = "Admin",
                    Status = true,
                    Address = "Hà Nội",
                    PhoneNumber = "0123456789"
                }
            );

            // 1. Set giá trị mặc định cho ngày tạo đơn
            modelBuilder.Entity<Order>()
                .Property(o => o.DateCreated)
                .HasDefaultValueSql("GETDATE()");

            // 2. Cấu hình kiểu dữ liệu tiền tệ (decimal) để tránh cảnh báo và sai số
            // (Chỉnh sửa tên Property cho khớp với Model thực tế của bạn)
            modelBuilder.Entity<FastFood>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>().Property(p => p.TotalPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderDetail>().Property(p => p.Price).HasColumnType("decimal(18,2)");

            // 3. CẤU HÌNH QUAN HỆ ĐỂ BẢO VỆ DỮ LIỆU (QUAN TRỌNG)

            // Quan hệ: Một OrderDetail thuộc về một FastFood
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.FastFood)
                .WithMany() // Một món ăn có thể nằm trong nhiều chi tiết đơn
                .HasForeignKey(od => od.FastFoodId)
                .OnDelete(DeleteBehavior.Restrict);
            // ^ RESTRICT: Nếu món ăn này đã có trong đơn hàng, Cấm xóa món ăn đó.
            // Admin phải ẩn món (Status = false) thay vì xóa vĩnh viễn.

            // Quan hệ: Một OrderDetail thuộc về một Order
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            // ^ CASCADE: Nếu xóa Đơn hàng, thì xóa luôn các Chi tiết của đơn đó (Hợp lý).
        }
    }
}