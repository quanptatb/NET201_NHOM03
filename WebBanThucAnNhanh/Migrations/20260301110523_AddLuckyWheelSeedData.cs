using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class AddLuckyWheelSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WheelPrizes",
                columns: new[] { "Id", "FastFoodId", "IsActive", "PrizeName", "PrizeType", "Probability", "RemainingQuantity" },
                values: new object[,]
                {
                    { 1, 2, true, "Coca Cola miễn phí", 1, 15.0, 50 },
                    { 2, 4, true, "Trà Sữa miễn phí", 1, 12.0, 40 },
                    { 3, 6, true, "Nước Cam Ép miễn phí", 1, 14.0, 45 },
                    { 4, 8, true, "Sinh Tố Bơ miễn phí", 1, 8.0, 30 },
                    { 5, 10, true, "Trà Đào miễn phí", 1, 12.0, 40 },
                    { 6, 18, true, "Matcha Latte miễn phí", 1, 7.0, 25 },
                    { 7, 20, true, "Cà Phê Sữa Đá miễn phí", 1, 12.0, 45 },
                    { 8, null, true, "Chúc may mắn lần sau!", 1, 20.0, 9999 },
                    { 9, 1, true, "Burger Bò miễn phí", 2, 10.0, 30 },
                    { 10, 3, true, "Gà Rán miễn phí", 2, 8.0, 25 },
                    { 11, 5, true, "Khoai Tây Chiên miễn phí", 2, 14.0, 50 },
                    { 12, 7, true, "Pizza Hải Sản miễn phí", 2, 4.0, 15 },
                    { 13, 11, true, "Hot Dog miễn phí", 2, 12.0, 40 },
                    { 14, 12, true, "Gà Viên Chiên miễn phí", 2, 10.0, 35 },
                    { 15, 16, true, "Sandwich Gà miễn phí", 2, 10.0, 30 },
                    { 16, null, true, "Chúc may mắn lần sau!", 2, 32.0, 9999 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "WheelPrizes",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
