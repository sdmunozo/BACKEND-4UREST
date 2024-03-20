using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryAndRelationWithCatalogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "MasterBase",
                table: "Categories");

            migrationBuilder.AddColumn<Guid>(
                name: "CatalogId",
                schema: "MasterBase",
                table: "Categories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "MasterBase",
                table: "Categories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "MasterBase",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsScheduleActive",
                schema: "MasterBase",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Sort",
                schema: "MasterBase",
                table: "Categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CatalogId",
                schema: "MasterBase",
                table: "Categories",
                column: "CatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Catalogs_CatalogId",
                schema: "MasterBase",
                table: "Categories",
                column: "CatalogId",
                principalSchema: "MasterBase",
                principalTable: "Catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Catalogs_CatalogId",
                schema: "MasterBase",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CatalogId",
                schema: "MasterBase",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CatalogId",
                schema: "MasterBase",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "MasterBase",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "MasterBase",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsScheduleActive",
                schema: "MasterBase",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Sort",
                schema: "MasterBase",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "MasterBase",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
