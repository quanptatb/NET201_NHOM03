using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class fastfood_typeoffastfood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeOfFastFoods",
                columns: table => new
                {
                    IdTypeOfFastFood = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameTypeOfFastFood = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfFastFoods", x => x.IdTypeOfFastFood);
                });

            migrationBuilder.CreateTable(
                name: "FastFoods",
                columns: table => new
                {
                    IdFastFood = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameFastFood = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdTypeOfFastFood = table.Column<int>(type: "int", nullable: false),
                    TypeOfFastFoodIdTypeOfFastFood = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastFoods", x => x.IdFastFood);
                    table.ForeignKey(
                        name: "FK_FastFoods_TypeOfFastFoods_TypeOfFastFoodIdTypeOfFastFood",
                        column: x => x.TypeOfFastFoodIdTypeOfFastFood,
                        principalTable: "TypeOfFastFoods",
                        principalColumn: "IdTypeOfFastFood",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FastFoods_TypeOfFastFoodIdTypeOfFastFood",
                table: "FastFoods",
                column: "TypeOfFastFoodIdTypeOfFastFood");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FastFoods");

            migrationBuilder.DropTable(
                name: "TypeOfFastFoods");
        }
    }
}
