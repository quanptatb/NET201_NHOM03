using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "IdTheme", "NameTheme" },
                values: new object[,]
                {
                    { 1, "Bữa sáng" },
                    { 2, "Tiệc tùng" }
                });

            migrationBuilder.InsertData(
                table: "TypeOfFastFoods",
                columns: new[] { "IdTypeOfFastFood", "NameTypeOfFastFood" },
                values: new object[,]
                {
                    { 1, "Đồ ăn nhanh" },
                    { 2, "Đồ uống" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Email", "FullName", "Password", "PhoneNumber", "Role", "Status", "Username" },
                values: new object[] { 1, "Hà Nội", "admin@fastfood.com", "Administrator", "123", "0123456789", "Admin", true, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "IdTheme",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "IdTheme",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TypeOfFastFoods",
                keyColumn: "IdTypeOfFastFood",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TypeOfFastFoods",
                keyColumn: "IdTypeOfFastFood",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
