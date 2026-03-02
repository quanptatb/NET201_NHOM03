using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class InsertCrossSellBundleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CrossSellBundles",
                columns: new[] { "Id", "AddOnFastFoodId", "DiscountPercentage", "MainFastFoodId" },
                values: new object[,]
                {
                    { 1, 5, 10.0, 1 },      // Burger Bò + Khoai Tây Chiên
                    { 2, 2, 5.0, 1 },      // Burger Bò + Coca Cola
                    { 3, 4, 5.0, 1 },      // Burger Bò + Trà Sữa
                    
                    { 4, 5, 10.0, 3 },     // Gà Rán + Khoai Tây Chiên
                    { 5, 2, 5.0, 3 },      // Gà Rán + Coca Cola
                    { 6, 6, 8.0, 3 },      // Gà Rán + Nước Cam Ép
                    
                    { 7, 6, 8.0, 7 },      // Pizza Hải Sản + Nước Cam Ép
                    { 8, 2, 5.0, 7 },      // Pizza Hải Sản + Coca Cola
                    { 9, 4, 5.0, 7 },      // Pizza Hải Sản + Trà Sữa
                    
                    { 10, 5, 10.0, 11 },   // Hot Dog + Khoai Tây Chiên
                    { 11, 2, 5.0, 11 },    // Hot Dog + Coca Cola
                    { 12, 10, 5.0, 11 },   // Hot Dog + Trà Đào
                    
                    { 13, 5, 8.0, 12 },    // Gà Viên Chiên + Khoai Tây Chiên
                    { 14, 2, 5.0, 12 },    // Gà Viên Chiên + Coca Cola
                    { 15, 4, 5.0, 12 },    // Gà Viên Chiên + Trà Sữa
                    
                    { 16, 4, 8.0, 16 },    // Sandwich Gà + Trà Sữa
                    { 17, 2, 5.0, 16 },    // Sandwich Gà + Coca Cola
                    { 18, 20, 5.0, 16 }    // Sandwich Gà + Cà Phê
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 1);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 2);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 3);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 4);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 5);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 6);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 7);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 8);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 9);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 10);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 11);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 12);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 13);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 14);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 15);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 16);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 17);
            migrationBuilder.DeleteData(
                table: "CrossSellBundles",
                keyColumn: "Id",
                keyValue: 18);
        }
    }
}
