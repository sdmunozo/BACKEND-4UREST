using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryAndRelationWithCatalogs4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "catalogs_background",
                schema: "MasterBase",
                table: "Brands",
                newName: "CatalogsBackground");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CatalogsBackground",
                schema: "MasterBase",
                table: "Brands",
                newName: "catalogs_background");
        }
    }
}
