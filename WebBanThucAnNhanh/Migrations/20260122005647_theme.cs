using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class theme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdTheme",
                table: "FastFoods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    IdTheme = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameTheme = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.IdTheme);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FastFoods_IdTheme",
                table: "FastFoods",
                column: "IdTheme");

            migrationBuilder.AddForeignKey(
                name: "FK_FastFoods_Themes_IdTheme",
                table: "FastFoods",
                column: "IdTheme",
                principalTable: "Themes",
                principalColumn: "IdTheme",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FastFoods_Themes_IdTheme",
                table: "FastFoods");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropIndex(
                name: "IX_FastFoods_IdTheme",
                table: "FastFoods");

            migrationBuilder.DropColumn(
                name: "IdTheme",
                table: "FastFoods");
        }
    }
}
