using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class BrandFieldsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                schema: "MasterBase",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                schema: "MasterBase",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                schema: "MasterBase",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slogan",
                schema: "MasterBase",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                schema: "MasterBase",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "catalogs_background",
                schema: "MasterBase",
                table: "Brands",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facebook",
                schema: "MasterBase",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Instagram",
                schema: "MasterBase",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Logo",
                schema: "MasterBase",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Slogan",
                schema: "MasterBase",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Website",
                schema: "MasterBase",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "catalogs_background",
                schema: "MasterBase",
                table: "Brands");
        }
    }
}
