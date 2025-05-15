using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Keys_And_Price : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PricePerItemPerPlatforms",
                schema: "MasterBase",
                table: "PricePerItemPerPlatforms");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "MasterBase",
                table: "Products",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "MasterBase",
                table: "Modifiers",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "MasterBase",
                table: "Items",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PricePerItemPerPlatforms",
                schema: "MasterBase",
                table: "PricePerItemPerPlatforms",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PricePerItemPerPlatforms_PlatformId_ItemId",
                schema: "MasterBase",
                table: "PricePerItemPerPlatforms",
                columns: new[] { "PlatformId", "ItemId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PricePerItemPerPlatforms",
                schema: "MasterBase",
                table: "PricePerItemPerPlatforms");

            migrationBuilder.DropIndex(
                name: "IX_PricePerItemPerPlatforms_PlatformId_ItemId",
                schema: "MasterBase",
                table: "PricePerItemPerPlatforms");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "MasterBase",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "MasterBase",
                table: "Modifiers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "MasterBase",
                table: "Items",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PricePerItemPerPlatforms",
                schema: "MasterBase",
                table: "PricePerItemPerPlatforms",
                columns: new[] { "PlatformId", "ItemId" });
        }
    }
}
