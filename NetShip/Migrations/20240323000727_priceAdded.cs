using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class priceAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "MasterBase",
                table: "Modifiers",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "MasterBase",
                table: "Items",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                schema: "MasterBase",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "MasterBase",
                table: "Items");
        }
    }
}
