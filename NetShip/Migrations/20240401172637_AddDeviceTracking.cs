using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceTrackings",
                schema: "MasterBase",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Browser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrowserVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PixelRatio = table.Column<double>(type: "float", nullable: true),
                    ScreenWidth = table.Column<int>(type: "int", nullable: true),
                    ScreenHeight = table.Column<int>(type: "int", nullable: true),
                    Orientation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceTrackings_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "MasterBase",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTrackings_BranchId",
                schema: "MasterBase",
                table: "DeviceTrackings",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceTrackings",
                schema: "MasterBase");
        }
    }
}
