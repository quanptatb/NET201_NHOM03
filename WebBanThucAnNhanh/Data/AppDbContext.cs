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
        

        // Fluent API (nếu cần thiết lập thêm các ràng buộc phức tạp theo Y6)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ví dụ: Set giá trị mặc định cho ngày tạo đơn
            modelBuilder.Entity<Order>()
                .Property(o => o.DateCreated)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}