using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class AddUrlNormalizedNamToBrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlNormalizedName",
                schema: "MasterBase",
                table: "Brands",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlNormalizedName",
                schema: "MasterBase",
                table: "Branches",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlNormalizedName",
                schema: "MasterBase",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "UrlNormalizedName",
                schema: "MasterBase",
                table: "Branches");
        }
    }
}
