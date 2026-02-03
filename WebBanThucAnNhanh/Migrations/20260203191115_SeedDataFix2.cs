using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "202cb962ac59075b964b07152d234b70");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "123");
        }
    }
}
