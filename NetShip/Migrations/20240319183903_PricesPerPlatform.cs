using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class PricesPerPlatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PricePerItemPerPlatforms",
                schema: "MasterBase",
                columns: table => new
                {
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlatformId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricePerItemPerPlatforms", x => new { x.PlatformId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_PricePerItemPerPlatforms_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "MasterBase",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PricePerItemPerPlatforms_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalSchema: "MasterBase",
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PricePerModifierPerPlatforms",
                schema: "MasterBase",
                columns: table => new
                {
                    ModifierId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlatformId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricePerModifierPerPlatforms", x => new { x.PlatformId, x.ModifierId });
                    table.ForeignKey(
                        name: "FK_PricePerModifierPerPlatforms_Modifiers_ModifierId",
                        column: x => x.ModifierId,
                        principalSchema: "MasterBase",
                        principalTable: "Modifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PricePerModifierPerPlatforms_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalSchema: "MasterBase",
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PricePerItemPerPlatforms_ItemId",
                schema: "MasterBase",
                table: "PricePerItemPerPlatforms",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PricePerModifierPerPlatforms_ModifierId",
                schema: "MasterBase",
                table: "PricePerModifierPerPlatforms",
                column: "ModifierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PricePerItemPerPlatforms",
                schema: "MasterBase");

            migrationBuilder.DropTable(
                name: "PricePerModifierPerPlatforms",
                schema: "MasterBase");
        }
    }
}
