using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSampleOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OptionGroups",
                columns: new[] { "Id", "IsMultiSelect", "Name" },
                values: new object[,]
                {
                    { 5, false, "Mức đường" },
                    { 6, false, "Mức đá" },
                    { 7, true, "Thêm đồ ăn kèm" }
                });

            migrationBuilder.InsertData(
                table: "FastFoodOptionGroups",
                columns: new[] { "FastFoodId", "OptionGroupId" },
                values: new object[,]
                {
                    { 1, 7 },
                    { 2, 6 },
                    { 4, 5 },
                    { 4, 6 },
                    { 6, 5 },
                    { 6, 6 },
                    { 7, 7 },
                    { 8, 5 },
                    { 8, 6 },
                    { 10, 5 },
                    { 10, 6 },
                    { 11, 7 },
                    { 16, 7 },
                    { 18, 5 },
                    { 18, 6 },
                    { 20, 5 },
                    { 20, 6 },
                    { 23, 5 },
                    { 23, 6 },
                    { 24, 5 },
                    { 24, 6 }
                });

            migrationBuilder.InsertData(
                table: "OptionItems",
                columns: new[] { "Id", "AdditionalPrice", "Name", "OptionGroupId" },
                values: new object[,]
                {
                    { 16, 0m, "100% Đường", 5 },
                    { 17, 0m, "70% Đường", 5 },
                    { 18, 0m, "50% Đường", 5 },
                    { 19, 0m, "30% Đường", 5 },
                    { 20, 0m, "0% Đường", 5 },
                    { 21, 0m, "100% Đá", 6 },
                    { 22, 0m, "50% Đá", 6 },
                    { 23, 0m, "0% Đá", 6 },
                    { 24, 10000m, "Thêm phô mai", 7 },
                    { 25, 15000m, "Thêm xúc xích", 7 },
                    { 26, 8000m, "Thêm trứng ốp la", 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 1, 7 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 4, 6 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 6, 5 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 7, 7 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 8, 5 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 8, 6 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 10, 5 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 10, 6 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 11, 7 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 16, 7 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 18, 5 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 18, 6 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 20, 5 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 20, 6 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 23, 5 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 23, 6 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 24, 5 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 24, 6 });

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "OptionGroups",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OptionGroups",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "OptionGroups",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
