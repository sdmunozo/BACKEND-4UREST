using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class AddDigitalMenuFieldsToBranch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DigitalMenuLink",
                schema: "MasterBase",
                table: "Branches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QrCodePath",
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
                name: "DigitalMenuLink",
                schema: "MasterBase",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "QrCodePath",
                schema: "MasterBase",
                table: "Branches");
        }
    }
}
