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
        public DbSet<CrossSellBundle> CrossSellBundles { get; set; }

        // === LUCKY WHEEL ===
        public DbSet<WheelPrize> WheelPrizes { get; set; }
        public DbSet<UserReward> UserRewards { get; set; }
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
                    PhoneNumber = "0123456789",
                    DrinkSpins = 0,
                    FoodSpins = 0
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

            // 5. Seed Data cho Giải thưởng Vòng quay
            modelBuilder.Entity<WheelPrize>().HasData(
                // === VÒNG QUAY NƯỚC (PrizeType = 1) ===
                new WheelPrize { Id = 1, PrizeName = "Coca Cola miễn phí", PrizeType = 1, FastFoodId = 2, Probability = 15, RemainingQuantity = 50, IsActive = true },
                new WheelPrize { Id = 2, PrizeName = "Trà Sữa miễn phí", PrizeType = 1, FastFoodId = 4, Probability = 12, RemainingQuantity = 40, IsActive = true },
                new WheelPrize { Id = 3, PrizeName = "Nước Cam Ép miễn phí", PrizeType = 1, FastFoodId = 6, Probability = 14, RemainingQuantity = 45, IsActive = true },
                new WheelPrize { Id = 4, PrizeName = "Sinh Tố Bơ miễn phí", PrizeType = 1, FastFoodId = 8, Probability = 8, RemainingQuantity = 30, IsActive = true },
                new WheelPrize { Id = 5, PrizeName = "Trà Đào miễn phí", PrizeType = 1, FastFoodId = 10, Probability = 12, RemainingQuantity = 40, IsActive = true },
                new WheelPrize { Id = 6, PrizeName = "Matcha Latte miễn phí", PrizeType = 1, FastFoodId = 18, Probability = 7, RemainingQuantity = 25, IsActive = true },
                new WheelPrize { Id = 7, PrizeName = "Cà Phê Sữa Đá miễn phí", PrizeType = 1, FastFoodId = 20, Probability = 12, RemainingQuantity = 45, IsActive = true },
                new WheelPrize { Id = 8, PrizeName = "Chúc may mắn lần sau!", PrizeType = 1, FastFoodId = null, Probability = 20, RemainingQuantity = 9999, IsActive = true },

                // === VÒNG QUAY ĐỒ ĂN (PrizeType = 2) ===
                new WheelPrize { Id = 9, PrizeName = "Burger Bò miễn phí", PrizeType = 2, FastFoodId = 1, Probability = 10, RemainingQuantity = 30, IsActive = true },
                new WheelPrize { Id = 10, PrizeName = "Gà Rán miễn phí", PrizeType = 2, FastFoodId = 3, Probability = 8, RemainingQuantity = 25, IsActive = true },
                new WheelPrize { Id = 11, PrizeName = "Khoai Tây Chiên miễn phí", PrizeType = 2, FastFoodId = 5, Probability = 14, RemainingQuantity = 50, IsActive = true },
                new WheelPrize { Id = 12, PrizeName = "Pizza Hải Sản miễn phí", PrizeType = 2, FastFoodId = 7, Probability = 4, RemainingQuantity = 15, IsActive = true },
                new WheelPrize { Id = 13, PrizeName = "Hot Dog miễn phí", PrizeType = 2, FastFoodId = 11, Probability = 12, RemainingQuantity = 40, IsActive = true },
                new WheelPrize { Id = 14, PrizeName = "Gà Viên Chiên miễn phí", PrizeType = 2, FastFoodId = 12, Probability = 10, RemainingQuantity = 35, IsActive = true },
                new WheelPrize { Id = 15, PrizeName = "Sandwich Gà miễn phí", PrizeType = 2, FastFoodId = 16, Probability = 10, RemainingQuantity = 30, IsActive = true },
                new WheelPrize { Id = 16, PrizeName = "Chúc may mắn lần sau!", PrizeType = 2, FastFoodId = null, Probability = 32, RemainingQuantity = 9999, IsActive = true }
            );

            // 6. Seed Data cho Cross Sell Bundle (Mua Kèm)
            modelBuilder.Entity<CrossSellBundle>().HasData(
                // === COMBO BURGER BÒ ===
                new CrossSellBundle { Id = 1, MainFastFoodId = 1, AddOnFastFoodId = 5, DiscountPercentage = 10 },  // Burger + Khoai Tây Chiên
                new CrossSellBundle { Id = 2, MainFastFoodId = 1, AddOnFastFoodId = 2, DiscountPercentage = 5 },   // Burger + Coca Cola
                new CrossSellBundle { Id = 3, MainFastFoodId = 1, AddOnFastFoodId = 4, DiscountPercentage = 5 },   // Burger + Trà Sữa

                // === COMBO GÀ RÁN ===
                new CrossSellBundle { Id = 4, MainFastFoodId = 3, AddOnFastFoodId = 5, DiscountPercentage = 10 },  // Gà Rán + Khoai Tây Chiên
                new CrossSellBundle { Id = 5, MainFastFoodId = 3, AddOnFastFoodId = 2, DiscountPercentage = 5 },   // Gà Rán + Coca Cola
                new CrossSellBundle { Id = 6, MainFastFoodId = 3, AddOnFastFoodId = 6, DiscountPercentage = 8 },   // Gà Rán + Nước Cam Ép

                // === COMBO PIZZA HẢI SẢN ===
                new CrossSellBundle { Id = 7, MainFastFoodId = 7, AddOnFastFoodId = 6, DiscountPercentage = 8 },   // Pizza + Nước Cam Ép
                new CrossSellBundle { Id = 8, MainFastFoodId = 7, AddOnFastFoodId = 2, DiscountPercentage = 5 },   // Pizza + Coca Cola
                new CrossSellBundle { Id = 9, MainFastFoodId = 7, AddOnFastFoodId = 4, DiscountPercentage = 5 },   // Pizza + Trà Sữa

                // === COMBO HOT DOG ===
                new CrossSellBundle { Id = 10, MainFastFoodId = 11, AddOnFastFoodId = 5, DiscountPercentage = 10 }, // Hot Dog + Khoai Tây Chiên
                new CrossSellBundle { Id = 11, MainFastFoodId = 11, AddOnFastFoodId = 2, DiscountPercentage = 5 },  // Hot Dog + Coca Cola
                new CrossSellBundle { Id = 12, MainFastFoodId = 11, AddOnFastFoodId = 10, DiscountPercentage = 5 }, // Hot Dog + Trà Đào

                // === COMBO GÀ VIÊN CHIÊN ===
                new CrossSellBundle { Id = 13, MainFastFoodId = 12, AddOnFastFoodId = 5, DiscountPercentage = 8 },  // Gà Viên + Khoai Tây Chiên
                new CrossSellBundle { Id = 14, MainFastFoodId = 12, AddOnFastFoodId = 2, DiscountPercentage = 5 },  // Gà Viên + Coca Cola
                new CrossSellBundle { Id = 15, MainFastFoodId = 12, AddOnFastFoodId = 4, DiscountPercentage = 5 },  // Gà Viên + Trà Sữa

                // === COMBO SANDWICH GÀ ===
                new CrossSellBundle { Id = 16, MainFastFoodId = 16, AddOnFastFoodId = 4, DiscountPercentage = 8 },  // Sandwich + Trà Sữa
                new CrossSellBundle { Id = 17, MainFastFoodId = 16, AddOnFastFoodId = 2, DiscountPercentage = 5 },  // Sandwich + Coca Cola
                new CrossSellBundle { Id = 18, MainFastFoodId = 16, AddOnFastFoodId = 20, DiscountPercentage = 5 }  // Sandwich + Cà Phê
            );

            // =============================================
            // 7. Seed Data cho Nhóm tùy chọn (Size, Topping, Sốt...)
            // =============================================
            modelBuilder.Entity<OptionGroup>().HasData(
                new OptionGroup { Id = 1, Name = "Size", IsMultiSelect = false },
                new OptionGroup { Id = 2, Name = "Topping", IsMultiSelect = true },
                new OptionGroup { Id = 3, Name = "Sốt chấm", IsMultiSelect = true },
                new OptionGroup { Id = 4, Name = "Đế bánh", IsMultiSelect = false },
                new OptionGroup { Id = 5, Name = "Mức đường", IsMultiSelect = false },
                new OptionGroup { Id = 6, Name = "Mức đá", IsMultiSelect = false },
                new OptionGroup { Id = 7, Name = "Thêm đồ ăn kèm", IsMultiSelect = true }
            );

            // 8. Seed Data cho các lựa chọn trong từng nhóm
            modelBuilder.Entity<OptionItem>().HasData(
                // === SIZE (OptionGroupId = 1) ===
                new OptionItem { Id = 1, OptionGroupId = 1, Name = "Size S", AdditionalPrice = 0m },
                new OptionItem { Id = 2, OptionGroupId = 1, Name = "Size M", AdditionalPrice = 5000m },
                new OptionItem { Id = 3, OptionGroupId = 1, Name = "Size L", AdditionalPrice = 10000m },

                // === TOPPING (OptionGroupId = 2) ===
                new OptionItem { Id = 4, OptionGroupId = 2, Name = "Trân châu đen", AdditionalPrice = 5000m },
                new OptionItem { Id = 5, OptionGroupId = 2, Name = "Trân châu trắng", AdditionalPrice = 5000m },
                new OptionItem { Id = 6, OptionGroupId = 2, Name = "Thạch dừa", AdditionalPrice = 5000m },
                new OptionItem { Id = 7, OptionGroupId = 2, Name = "Pudding", AdditionalPrice = 8000m },
                new OptionItem { Id = 8, OptionGroupId = 2, Name = "Kem cheese", AdditionalPrice = 10000m },

                // === SỐT CHẤM (OptionGroupId = 3) ===
                new OptionItem { Id = 9, OptionGroupId = 3, Name = "Sốt BBQ", AdditionalPrice = 0m },
                new OptionItem { Id = 10, OptionGroupId = 3, Name = "Sốt Cay", AdditionalPrice = 3000m },
                new OptionItem { Id = 11, OptionGroupId = 3, Name = "Sốt Phô mai", AdditionalPrice = 5000m },
                new OptionItem { Id = 12, OptionGroupId = 3, Name = "Sốt Mù tạt", AdditionalPrice = 0m },

                // === ĐẾ BÁNH (OptionGroupId = 4) ===
                new OptionItem { Id = 13, OptionGroupId = 4, Name = "Đế mỏng truyền thống", AdditionalPrice = 0m },
                new OptionItem { Id = 14, OptionGroupId = 4, Name = "Đế dày xốp", AdditionalPrice = 10000m },
                new OptionItem { Id = 15, OptionGroupId = 4, Name = "Đế viền phô mai", AdditionalPrice = 15000m },

                // === MỨC ĐƯỜNG (OptionGroupId = 5) ===
                new OptionItem { Id = 16, OptionGroupId = 5, Name = "100% Đường", AdditionalPrice = 0m },
                new OptionItem { Id = 17, OptionGroupId = 5, Name = "70% Đường", AdditionalPrice = 0m },
                new OptionItem { Id = 18, OptionGroupId = 5, Name = "50% Đường", AdditionalPrice = 0m },
                new OptionItem { Id = 19, OptionGroupId = 5, Name = "30% Đường", AdditionalPrice = 0m },
                new OptionItem { Id = 20, OptionGroupId = 5, Name = "0% Đường", AdditionalPrice = 0m },

                // === MỨC ĐÁ (OptionGroupId = 6) ===
                new OptionItem { Id = 21, OptionGroupId = 6, Name = "100% Đá", AdditionalPrice = 0m },
                new OptionItem { Id = 22, OptionGroupId = 6, Name = "50% Đá", AdditionalPrice = 0m },
                new OptionItem { Id = 23, OptionGroupId = 6, Name = "0% Đá", AdditionalPrice = 0m },

                // === THÊM ĐỒ ĂN KÈM (OptionGroupId = 7) ===
                new OptionItem { Id = 24, OptionGroupId = 7, Name = "Thêm phô mai", AdditionalPrice = 10000m },
                new OptionItem { Id = 25, OptionGroupId = 7, Name = "Thêm xúc xích", AdditionalPrice = 15000m },
                new OptionItem { Id = 26, OptionGroupId = 7, Name = "Thêm trứng ốp la", AdditionalPrice = 8000m }
            );

            // 9. Seed Data liên kết món ăn với nhóm tùy chọn
            modelBuilder.Entity<FastFoodOptionGroup>().HasData(
                // === ĐỒ UỐNG: Size + Topping ===
                // Trà Sữa (Id=4)
                new FastFoodOptionGroup { FastFoodId = 4, OptionGroupId = 1 },
                new FastFoodOptionGroup { FastFoodId = 4, OptionGroupId = 2 },
                new FastFoodOptionGroup { FastFoodId = 4, OptionGroupId = 5 },
                new FastFoodOptionGroup { FastFoodId = 4, OptionGroupId = 6 },
                // Trà Đào (Id=10)
                new FastFoodOptionGroup { FastFoodId = 10, OptionGroupId = 1 },
                new FastFoodOptionGroup { FastFoodId = 10, OptionGroupId = 2 },
                new FastFoodOptionGroup { FastFoodId = 10, OptionGroupId = 5 },
                new FastFoodOptionGroup { FastFoodId = 10, OptionGroupId = 6 },
                // Matcha Latte (Id=18)
                new FastFoodOptionGroup { FastFoodId = 18, OptionGroupId = 1 },
                new FastFoodOptionGroup { FastFoodId = 18, OptionGroupId = 2 },
                new FastFoodOptionGroup { FastFoodId = 18, OptionGroupId = 5 },
                new FastFoodOptionGroup { FastFoodId = 18, OptionGroupId = 6 },
                // Trà Vải (Id=24)
                new FastFoodOptionGroup { FastFoodId = 24, OptionGroupId = 1 },
                new FastFoodOptionGroup { FastFoodId = 24, OptionGroupId = 2 },
                new FastFoodOptionGroup { FastFoodId = 24, OptionGroupId = 5 },
                new FastFoodOptionGroup { FastFoodId = 24, OptionGroupId = 6 },

                // === ĐỒ UỐNG: Chỉ Size ===
                // Coca Cola (Id=2)
                new FastFoodOptionGroup { FastFoodId = 2, OptionGroupId = 1 },
                new FastFoodOptionGroup { FastFoodId = 2, OptionGroupId = 6 },
                // Nước Cam Ép (Id=6)
                new FastFoodOptionGroup { FastFoodId = 6, OptionGroupId = 1 },
                new FastFoodOptionGroup { FastFoodId = 6, OptionGroupId = 5 },
                new FastFoodOptionGroup { FastFoodId = 6, OptionGroupId = 6 },
                // Sinh Tố Bơ (Id=8)
                new FastFoodOptionGroup { FastFoodId = 8, OptionGroupId = 1 },
                new FastFoodOptionGroup { FastFoodId = 8, OptionGroupId = 5 },
                new FastFoodOptionGroup { FastFoodId = 8, OptionGroupId = 6 },
                // Cà Phê Sữa Đá (Id=20)
                new FastFoodOptionGroup { FastFoodId = 20, OptionGroupId = 1 },
                new FastFoodOptionGroup { FastFoodId = 20, OptionGroupId = 5 },
                new FastFoodOptionGroup { FastFoodId = 20, OptionGroupId = 6 },
                // Nước Chanh Dây (Id=23)
                new FastFoodOptionGroup { FastFoodId = 23, OptionGroupId = 1 },
                new FastFoodOptionGroup { FastFoodId = 23, OptionGroupId = 5 },
                new FastFoodOptionGroup { FastFoodId = 23, OptionGroupId = 6 },

                // === ĐỒ ĂN: Sốt chấm ===
                // Burger Bò (Id=1)
                new FastFoodOptionGroup { FastFoodId = 1, OptionGroupId = 3 },
                new FastFoodOptionGroup { FastFoodId = 1, OptionGroupId = 7 },
                // Gà Rán (Id=3)
                new FastFoodOptionGroup { FastFoodId = 3, OptionGroupId = 3 },
                // Khoai Tây Chiên (Id=5)
                new FastFoodOptionGroup { FastFoodId = 5, OptionGroupId = 3 },
                // Hot Dog (Id=11)
                new FastFoodOptionGroup { FastFoodId = 11, OptionGroupId = 3 },
                new FastFoodOptionGroup { FastFoodId = 11, OptionGroupId = 7 },
                // Gà Viên Chiên (Id=12)
                new FastFoodOptionGroup { FastFoodId = 12, OptionGroupId = 3 },
                // Hành Tây Chiên (Id=15)
                new FastFoodOptionGroup { FastFoodId = 15, OptionGroupId = 3 },
                // Sandwich Gà (Id=16)
                new FastFoodOptionGroup { FastFoodId = 16, OptionGroupId = 3 },
                new FastFoodOptionGroup { FastFoodId = 16, OptionGroupId = 7 },
                // Cánh Gà Chiên (Id=17)
                new FastFoodOptionGroup { FastFoodId = 17, OptionGroupId = 3 },
                // Xúc Xích Nướng (Id=22)
                new FastFoodOptionGroup { FastFoodId = 22, OptionGroupId = 3 },

                // === PIZZA: Đế bánh + Sốt ===
                // Pizza Hải Sản (Id=7)
                new FastFoodOptionGroup { FastFoodId = 7, OptionGroupId = 4 },
                new FastFoodOptionGroup { FastFoodId = 7, OptionGroupId = 3 },
                new FastFoodOptionGroup { FastFoodId = 7, OptionGroupId = 7 }
            );

            // === CẤU HÌNH CROSS SELL BUNDLE ===
            // Quan hệ: CrossSellBundle -> MainFastFood
            modelBuilder.Entity<CrossSellBundle>()
                .HasOne(c => c.MainFastFood)
                .WithMany()
                .HasForeignKey(c => c.MainFastFoodId)
                .OnDelete(DeleteBehavior.NoAction);

            // Quan hệ: CrossSellBundle -> AddOnFastFood
            modelBuilder.Entity<CrossSellBundle>()
                .HasOne(c => c.AddOnFastFood)
                .WithMany()
                .HasForeignKey(c => c.AddOnFastFoodId)
                .OnDelete(DeleteBehavior.NoAction);

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

            // === CẤU HÌNH LUCKY WHEEL ===

            // Quan hệ: WheelPrize -> FastFood (nullable)
            modelBuilder.Entity<WheelPrize>()
                .HasOne(wp => wp.FastFood)
                .WithMany()
                .HasForeignKey(wp => wp.FastFoodId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ: UserReward -> User
            modelBuilder.Entity<UserReward>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRewards)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ: UserReward -> WheelPrize
            modelBuilder.Entity<UserReward>()
                .HasOne(ur => ur.WheelPrize)
                .WithMany()
                .HasForeignKey(ur => ur.PrizeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Giá trị mặc định cho DateWon
            modelBuilder.Entity<UserReward>()
                .Property(ur => ur.DateWon)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}