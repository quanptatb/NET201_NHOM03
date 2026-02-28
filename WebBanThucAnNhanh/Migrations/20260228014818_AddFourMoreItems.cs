using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class AddFourMoreItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FastFoods",
                columns: new[] { "IdFastFood", "Description", "IdTheme", "IdTypeOfFastFood", "Image", "NameFastFood", "Price", "Quantity", "Status" },
                values: new object[,]
                {
                    { 21, "Bánh mì thịt truyền thống Việt Nam với patê, thịt nguội và rau sống", 1, 1, "banh_mi_thit.jpg", "Bánh Mì Thịt", 35000m, 150, true },
                    { 22, "Xúc xích nướng than hoa thơm lừng kèm mù tạt", 2, 1, "xuc_xich_nuong.jpg", "Xúc Xích Nướng", 40000m, 130, true },
                    { 23, "Nước chanh dây tươi mát, chua ngọt tự nhiên", 2, 2, "nuoc_chanh_day.jpg", "Nước Chanh Dây", 25000m, 180, true },
                    { 24, "Trà vải thanh mát, hương vải thơm ngọt tự nhiên", 1, 2, "tra_vai.jpg", "Trà Vải", 30000m, 160, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 24);
        }
    }
}
