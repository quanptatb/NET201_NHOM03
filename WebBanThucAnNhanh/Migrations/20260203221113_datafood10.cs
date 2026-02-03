using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class datafood10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FastFoods",
                columns: new[] { "IdFastFood", "Description", "IdTheme", "IdTypeOfFastFood", "Image", "NameFastFood", "Price", "Quantity", "Status" },
                values: new object[,]
                {
                    { 4, "Trà sữa thơm ngon", 1, 2, "tra_sua.jpg", "Trà Sữa", 30000m, 180, true },
                    { 5, "Khoai tây chiên giòn tan", 1, 1, "khoai_tay_chien.jpg", "Khoai Tây Chiên", 25000m, 120, true },
                    { 6, "Nước cam ép tươi mát", 2, 2, "nuoc_cam_ep.jpg", "Nước Cam Ép", 20000m, 160, true },
                    { 7, "Pizza hải sản hấp dẫn", 1, 1, "pizza_hai_san.jpg", "Pizza Hải Sản", 120000m, 100, true },
                    { 8, "Sinh tố bơ béo ngậy", 2, 2, "sinh_to_bo.jpg", "Sinh Tố Bơ", 35000m, 140, true },
                    { 9, "Mì Ý sốt cà chua", 1, 1, "mi_y.jpg", "Mì Ý", 80000m, 110, true },
                    { 10, "Trà đào mát lạnh", 2, 2, "tra_dao.jpg", "Trà Đào", 30000m, 170, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 10);
        }
    }
}
