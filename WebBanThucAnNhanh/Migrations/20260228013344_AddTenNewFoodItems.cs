using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class AddTenNewFoodItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FastFoods",
                columns: new[] { "IdFastFood", "Description", "IdTheme", "IdTypeOfFastFood", "Image", "NameFastFood", "Price", "Quantity", "Status" },
                values: new object[,]
                {
                    { 11, "Hot dog truyền thống với xúc xích, mù tạt và tương cà", 2, 1, "hotdog.png", "Hot Dog", 45000m, 120, true },
                    { 12, "Gà viên chiên giòn rụm kèm sốt chấm đặc biệt", 2, 1, "chicken_nuggets.png", "Gà Viên Chiên", 55000m, 130, true },
                    { 13, "Salad Caesar tươi mát với rau xà lách, phô mai và sốt đặc trưng", 1, 1, "salad_caesar.png", "Salad Caesar", 60000m, 90, true },
                    { 14, "Taco nhân thịt bò xào rau củ tươi ngon kiểu Mexico", 2, 1, "taco_bo.png", "Taco Bò", 65000m, 100, true },
                    { 15, "Hành tây chiên giòn vàng ươm thơm phức", 2, 1, "onion_rings.png", "Hành Tây Chiên", 30000m, 150, true },
                    { 16, "Sandwich gà nướng với rau xà lách, cà chua và phô mai", 1, 1, "sandwich_ga.png", "Sandwich Gà", 55000m, 110, true },
                    { 17, "Cánh gà chiên giòn sốt cay đậm đà", 2, 1, "canh_ga.png", "Cánh Gà Chiên", 70000m, 120, true },
                    { 18, "Matcha latte thơm ngon, vị trà xanh thanh mát", 1, 2, "matcha_latte.png", "Matcha Latte", 40000m, 160, true },
                    { 19, "Cơm gà Hải Nam thơm dẻo với gà luộc mềm ngọt", 1, 1, "com_ga.png", "Cơm Gà", 55000m, 100, true },
                    { 20, "Cà phê sữa đá truyền thống Việt Nam đậm đà", 1, 2, "cafe_sua_da.png", "Cà Phê Sữa Đá", 25000m, 200, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 20);
        }
    }
}
