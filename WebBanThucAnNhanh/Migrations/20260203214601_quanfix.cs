using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class quanfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FastFoods",
                columns: new[] { "IdFastFood", "Description", "IdTheme", "IdTypeOfFastFood", "Image", "NameFastFood", "Price", "Quantity", "Status" },
                values: new object[,]
                {
                    { 1, "Burger bò ngon tuyệt vời", 1, 1, "burger_bo.jpg", "Burger Bò", 50000m, 100, true },
                    { 2, "Nước ngọt giải khát", 2, 2, "coca_cola.jpg", "Coca Cola", 15000m, 200, true },
                    { 3, "Gà rán giòn rụm", 2, 1, "ga_ran.jpg", "Gà Rán", 75000m, 150, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FastFoods",
                keyColumn: "IdFastFood",
                keyValue: 3);
        }
    }
}
