using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebBanThucAnNhanh.Migrations
{
    /// <inheritdoc />
    public partial class SeedOptionGroupsAndToppings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM FastFoodOptionGroups;");
            migrationBuilder.Sql("DELETE FROM OptionItems;");
            migrationBuilder.Sql("DELETE FROM OptionGroups;");

            migrationBuilder.InsertData(
                table: "OptionGroups",
                columns: new[] { "Id", "IsMultiSelect", "Name" },
                values: new object[,]
                {
                    { 1, false, "Size" },
                    { 2, true, "Topping" },
                    { 3, true, "Sốt chấm" },
                    { 4, false, "Đế bánh" }
                });

            migrationBuilder.InsertData(
                table: "FastFoodOptionGroups",
                columns: new[] { "FastFoodId", "OptionGroupId" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 2, 1 },
                    { 3, 3 },
                    { 4, 1 },
                    { 4, 2 },
                    { 5, 3 },
                    { 6, 1 },
                    { 7, 3 },
                    { 7, 4 },
                    { 8, 1 },
                    { 10, 1 },
                    { 10, 2 },
                    { 11, 3 },
                    { 12, 3 },
                    { 15, 3 },
                    { 16, 3 },
                    { 17, 3 },
                    { 18, 1 },
                    { 18, 2 },
                    { 20, 1 },
                    { 22, 3 },
                    { 23, 1 },
                    { 24, 1 },
                    { 24, 2 }
                });

            migrationBuilder.InsertData(
                table: "OptionItems",
                columns: new[] { "Id", "AdditionalPrice", "Name", "OptionGroupId" },
                values: new object[,]
                {
                    { 1, 0m, "Size S", 1 },
                    { 2, 5000m, "Size M", 1 },
                    { 3, 10000m, "Size L", 1 },
                    { 4, 5000m, "Trân châu đen", 2 },
                    { 5, 5000m, "Trân châu trắng", 2 },
                    { 6, 5000m, "Thạch dừa", 2 },
                    { 7, 8000m, "Pudding", 2 },
                    { 8, 10000m, "Kem cheese", 2 },
                    { 9, 0m, "Sốt BBQ", 3 },
                    { 10, 3000m, "Sốt Cay", 3 },
                    { 11, 5000m, "Sốt Phô mai", 3 },
                    { 12, 0m, "Sốt Mù tạt", 3 },
                    { 13, 0m, "Đế mỏng truyền thống", 4 },
                    { 14, 10000m, "Đế dày xốp", 4 },
                    { 15, 15000m, "Đế viền phô mai", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 7, 3 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 7, 4 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 8, 1 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 10, 1 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 10, 2 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 11, 3 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 12, 3 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 15, 3 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 16, 3 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 17, 3 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 18, 1 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 18, 2 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 20, 1 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 22, 3 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 23, 1 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 24, 1 });

            migrationBuilder.DeleteData(
                table: "FastFoodOptionGroups",
                keyColumns: new[] { "FastFoodId", "OptionGroupId" },
                keyValues: new object[] { 24, 2 });

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "OptionItems",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "OptionGroups",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OptionGroups",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OptionGroups",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OptionGroups",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
