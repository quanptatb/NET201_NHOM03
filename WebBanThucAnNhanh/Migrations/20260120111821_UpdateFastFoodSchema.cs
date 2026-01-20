using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFastFoodSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FastFoods_TypeOfFastFoods_TypeOfFastFoodIdTypeOfFastFood",
                table: "FastFoods");

            migrationBuilder.DropIndex(
                name: "IX_FastFoods_TypeOfFastFoodIdTypeOfFastFood",
                table: "FastFoods");

            migrationBuilder.DropColumn(
                name: "TypeOfFastFoodIdTypeOfFastFood",
                table: "FastFoods");

            migrationBuilder.AlterColumn<string>(
                name: "NameFastFood",
                table: "FastFoods",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_FastFoods_IdTypeOfFastFood",
                table: "FastFoods",
                column: "IdTypeOfFastFood");

            migrationBuilder.AddForeignKey(
                name: "FK_FastFoods_TypeOfFastFoods_IdTypeOfFastFood",
                table: "FastFoods",
                column: "IdTypeOfFastFood",
                principalTable: "TypeOfFastFoods",
                principalColumn: "IdTypeOfFastFood",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FastFoods_TypeOfFastFoods_IdTypeOfFastFood",
                table: "FastFoods");

            migrationBuilder.DropIndex(
                name: "IX_FastFoods_IdTypeOfFastFood",
                table: "FastFoods");

            migrationBuilder.AlterColumn<string>(
                name: "NameFastFood",
                table: "FastFoods",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "TypeOfFastFoodIdTypeOfFastFood",
                table: "FastFoods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FastFoods_TypeOfFastFoodIdTypeOfFastFood",
                table: "FastFoods",
                column: "TypeOfFastFoodIdTypeOfFastFood");

            migrationBuilder.AddForeignKey(
                name: "FK_FastFoods_TypeOfFastFoods_TypeOfFastFoodIdTypeOfFastFood",
                table: "FastFoods",
                column: "TypeOfFastFoodIdTypeOfFastFood",
                principalTable: "TypeOfFastFoods",
                principalColumn: "IdTypeOfFastFood",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
