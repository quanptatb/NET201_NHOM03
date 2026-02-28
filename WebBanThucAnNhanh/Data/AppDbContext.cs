using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Models;

namespace WebBanThucAnNhanh.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<OptionGroup> OptionGroups { get; set; }
        public DbSet<OptionItem> OptionItems { get; set; }
        public DbSet<FastFoodOptionGroup> FastFoodOptionGroups { get; set; }
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
            modelBuilder.Entity<FastFoodOptionGroup>()
    .HasKey(fog => new { fog.FastFoodId, fog.OptionGroupId });
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
            // 4. Tạo một số món ăn nhanh mẫu
            modelBuilder.Entity<FastFood>().HasData(
                new FastFood
                {
                    IdFastFood = 1,
                    NameFastFood = "Burger Bò",
                    Price = 50000m,
                    Quantity = 100,
                    Image = "burger_bo.jpg",
                    Status = true,
                    Description = "Burger bò ngon tuyệt vời",
                    IdTypeOfFastFood = 1,
                    IdTheme = 1
                },
                new FastFood
                {
                    IdFastFood = 2,
                    NameFastFood = "Coca Cola",
                    Price = 15000m,
                    Quantity = 200,
                    Image = "coca_cola.jpg",
                    Status = true,
                    Description = "Nước ngọt giải khát",
                    IdTypeOfFastFood = 2,
                    IdTheme = 2
                },

                new FastFood
                {
                    IdFastFood = 3,
                    NameFastFood = "Gà Rán",
                    Price = 75000m,
                    Quantity = 150,
                    Image = "ga_ran.jpg",
                    Status = true,
                    Description = "Gà rán giòn rụm",
                    IdTypeOfFastFood = 1,
                    IdTheme = 2
                },

                new FastFood
                {
                    IdFastFood = 4,
                    NameFastFood = "Trà Sữa",
                    Price = 30000m,
                    Quantity = 180,
                    Image = "tra_sua.jpg",
                    Status = true,
                    Description = "Trà sữa thơm ngon",
                    IdTypeOfFastFood = 2,
                    IdTheme = 1
                },

                new FastFood
                {
                    IdFastFood = 5,
                    NameFastFood = "Khoai Tây Chiên",
                    Price = 25000m,
                    Quantity = 120,
                    Image = "khoai_tay_chien.jpg",
                    Status = true,
                    Description = "Khoai tây chiên giòn tan",
                    IdTypeOfFastFood = 1,
                    IdTheme = 1
                },

                new FastFood
                {
                    IdFastFood = 6,
                    NameFastFood = "Nước Cam Ép",
                    Price = 20000m,
                    Quantity = 160,
                    Image = "nuoc_cam_ep.jpg",
                    Status = true,
                    Description = "Nước cam ép tươi mát",
                    IdTypeOfFastFood = 2,
                    IdTheme = 2
                },

                new FastFood
                {
                    IdFastFood = 7,
                    NameFastFood = "Pizza Hải Sản",
                    Price = 120000m,
                    Quantity = 100,
                    Image = "pizza_hai_san.jpg",
                    Status = true,
                    Description = "Pizza hải sản hấp dẫn",
                    IdTypeOfFastFood = 1,
                    IdTheme = 1
                },

                new FastFood
                {
                    IdFastFood = 8,
                    NameFastFood = "Sinh Tố Bơ",
                    Price = 35000m,
                    Quantity = 140,
                    Image = "sinh_to_bo.jpg",
                    Status = true,
                    Description = "Sinh tố bơ béo ngậy",
                    IdTypeOfFastFood = 2,
                    IdTheme = 2
                },

                new FastFood
                {
                    IdFastFood = 9,
                    NameFastFood = "Mì Ý",
                    Price = 80000m,
                    Quantity = 110,
                    Image = "mi_y.jpg",
                    Status = true,
                    Description = "Mì Ý sốt cà chua",
                    IdTypeOfFastFood = 1,
                    IdTheme = 1
                },

                new FastFood
                {
                    IdFastFood = 10,
                    NameFastFood = "Trà Đào",
                    Price = 30000m,
                    Quantity = 170,
                    Image = "tra_dao.jpg",
                    Status = true,
                    Description = "Trà đào mát lạnh",
                    IdTypeOfFastFood = 2,
                    IdTheme = 2
                },

                new FastFood
                {
                    IdFastFood = 11,
                    NameFastFood = "Hot Dog",
                    Price = 45000m,
                    Quantity = 120,
                    Image = "hotdog.png",
                    Status = true,
                    Description = "Hot dog truyền thống với xúc xích, mù tạt và tương cà",
                    IdTypeOfFastFood = 1,
                    IdTheme = 2
                },

                new FastFood
                {
                    IdFastFood = 12,
                    NameFastFood = "Gà Viên Chiên",
                    Price = 55000m,
                    Quantity = 130,
                    Image = "chicken_nuggets.png",
                    Status = true,
                    Description = "Gà viên chiên giòn rụm kèm sốt chấm đặc biệt",
                    IdTypeOfFastFood = 1,
                    IdTheme = 2
                },

                new FastFood
                {
                    IdFastFood = 13,
                    NameFastFood = "Salad Caesar",
                    Price = 60000m,
                    Quantity = 90,
                    Image = "salad_caesar.png",
                    Status = true,
                    Description = "Salad Caesar tươi mát với rau xà lách, phô mai và sốt đặc trưng",
                    IdTypeOfFastFood = 1,
                    IdTheme = 1
                },

                new FastFood
                {
                    IdFastFood = 14,
                    NameFastFood = "Taco Bò",
                    Price = 65000m,
                    Quantity = 100,
                    Image = "taco_bo.png",
                    Status = true,
                    Description = "Taco nhân thịt bò xào rau củ tươi ngon kiểu Mexico",
                    IdTypeOfFastFood = 1,
                    IdTheme = 2
                },

                new FastFood
                {
                    IdFastFood = 15,
                    NameFastFood = "Hành Tây Chiên",
                    Price = 30000m,
                    Quantity = 150,
                    Image = "onion_rings.png",
                    Status = true,
                    Description = "Hành tây chiên giòn vàng ươm thơm phức",
                    IdTypeOfFastFood = 1,
                    IdTheme = 2
                },

                new FastFood
                {
                    IdFastFood = 16,
                    NameFastFood = "Sandwich Gà",
                    Price = 55000m,
                    Quantity = 110,
                    Image = "sandwich_ga.png",
                    Status = true,
                    Description = "Sandwich gà nướng với rau xà lách, cà chua và phô mai",
                    IdTypeOfFastFood = 1,
                    IdTheme = 1
                },

                new FastFood
                {
                    IdFastFood = 17,
                    NameFastFood = "Cánh Gà Chiên",
                    Price = 70000m,
                    Quantity = 120,
                    Image = "canh_ga.png",
                    Status = true,
                    Description = "Cánh gà chiên giòn sốt cay đậm đà",
                    IdTypeOfFastFood = 1,
                    IdTheme = 2
                },

                new FastFood
                {
                    IdFastFood = 18,
                    NameFastFood = "Matcha Latte",
                    Price = 40000m,
                    Quantity = 160,
                    Image = "matcha_latte.png",
                    Status = true,
                    Description = "Matcha latte thơm ngon, vị trà xanh thanh mát",
                    IdTypeOfFastFood = 2,
                    IdTheme = 1
                },

                new FastFood
                {
                    IdFastFood = 19,
                    NameFastFood = "Cơm Gà",
                    Price = 55000m,
                    Quantity = 100,
                    Image = "com_ga.png",
                    Status = true,
                    Description = "Cơm gà Hải Nam thơm dẻo với gà luộc mềm ngọt",
                    IdTypeOfFastFood = 1,
                    IdTheme = 1
                },

                new FastFood
                {
                    IdFastFood = 20,
                    NameFastFood = "Cà Phê Sữa Đá",
                    Price = 25000m,
                    Quantity = 200,
                    Image = "cafe_sua_da.png",
                    Status = true,
                    Description = "Cà phê sữa đá truyền thống Việt Nam đậm đà",
                    IdTypeOfFastFood = 2,
                    IdTheme = 1
                },

                new FastFood
                {
                    IdFastFood = 21,
                    NameFastFood = "Bánh Mì Thịt",
                    Price = 35000m,
                    Quantity = 150,
                    Image = "banh_mi_thit.jpg",
                    Status = true,
                    Description = "Bánh mì thịt truyền thống Việt Nam với patê, thịt nguội và rau sống",
                    IdTypeOfFastFood = 1,
                    IdTheme = 1
                },

                new FastFood
                {
                    IdFastFood = 22,
                    NameFastFood = "Xúc Xích Nướng",
                    Price = 40000m,
                    Quantity = 130,
                    Image = "xuc_xich_nuong.jpg",
                    Status = true,
                    Description = "Xúc xích nướng than hoa thơm lừng kèm mù tạt",
                    IdTypeOfFastFood = 1,
                    IdTheme = 2
                },

                new FastFood
                {
                    IdFastFood = 23,
                    NameFastFood = "Nước Chanh Dây",
                    Price = 25000m,
                    Quantity = 180,
                    Image = "nuoc_chanh_day.jpg",
                    Status = true,
                    Description = "Nước chanh dây tươi mát, chua ngọt tự nhiên",
                    IdTypeOfFastFood = 2,
                    IdTheme = 2
                },

                new FastFood
                {
                    IdFastFood = 24,
                    NameFastFood = "Trà Vải",
                    Price = 30000m,
                    Quantity = 160,
                    Image = "tra_vai.jpg",
                    Status = true,
                    Description = "Trà vải thanh mát, hương vải thơm ngọt tự nhiên",
                    IdTypeOfFastFood = 2,
                    IdTheme = 1
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