using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class CrossSellBundle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrossSellBundles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainFastFoodId = table.Column<int>(type: "int", nullable: false),
                    AddOnFastFoodId = table.Column<int>(type: "int", nullable: false),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrossSellBundles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrossSellBundles_FastFoods_AddOnFastFoodId",
                        column: x => x.AddOnFastFoodId,
                        principalTable: "FastFoods",
                        principalColumn: "IdFastFood",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CrossSellBundles_FastFoods_MainFastFoodId",
                        column: x => x.MainFastFoodId,
                        principalTable: "FastFoods",
                        principalColumn: "IdFastFood",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrossSellBundles_AddOnFastFoodId",
                table: "CrossSellBundles",
                column: "AddOnFastFoodId");

            migrationBuilder.CreateIndex(
                name: "IX_CrossSellBundles_MainFastFoodId",
                table: "CrossSellBundles",
                column: "MainFastFoodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrossSellBundles");
        }
    }
}
