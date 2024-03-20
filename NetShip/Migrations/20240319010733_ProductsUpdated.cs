using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class ProductsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "MasterBase",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "MasterBase",
                table: "Products",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                schema: "MasterBase",
                table: "Products",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "MasterBase",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "MasterBase",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScheduleActive",
                schema: "MasterBase",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Sort",
                schema: "MasterBase",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "MasterBase",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "MasterBase",
                table: "Products",
                column: "CategoryId",
                principalSchema: "MasterBase",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "MasterBase",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                schema: "MasterBase",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "MasterBase",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "MasterBase",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsScheduleActive",
                schema: "MasterBase",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Sort",
                schema: "MasterBase",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "MasterBase",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                schema: "MasterBase",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "MasterBase",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
