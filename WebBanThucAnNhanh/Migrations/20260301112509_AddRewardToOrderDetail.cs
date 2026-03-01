using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class AddRewardToOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReward",
                table: "OrderDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "OrderDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReward",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "OrderDetails");
        }
    }
}
