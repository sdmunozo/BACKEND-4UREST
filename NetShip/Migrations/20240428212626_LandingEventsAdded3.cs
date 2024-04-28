using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class LandingEventsAdded3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LandingUserEvents",
                schema: "MasterBase",
                table: "LandingUserEvents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LandingUserEvents",
                schema: "MasterBase",
                table: "LandingUserEvents",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LandingUserEvents",
                schema: "MasterBase",
                table: "LandingUserEvents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LandingUserEvents",
                schema: "MasterBase",
                table: "LandingUserEvents",
                column: "UserId");
        }
    }
}
