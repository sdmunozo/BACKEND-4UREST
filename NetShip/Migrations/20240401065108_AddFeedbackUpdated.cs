using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Branches_BranchId",
                schema: "MasterBase",
                table: "Feedback");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedback",
                schema: "MasterBase",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "Mood",
                schema: "MasterBase",
                table: "Feedback");

            migrationBuilder.RenameTable(
                name: "Feedback",
                schema: "MasterBase",
                newName: "Feedbacks",
                newSchema: "MasterBase");

            migrationBuilder.RenameIndex(
                name: "IX_Feedback_BranchId",
                schema: "MasterBase",
                table: "Feedbacks",
                newName: "IX_Feedbacks_BranchId");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                schema: "MasterBase",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedbacks",
                schema: "MasterBase",
                table: "Feedbacks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Branches_BranchId",
                schema: "MasterBase",
                table: "Feedbacks",
                column: "BranchId",
                principalSchema: "MasterBase",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Branches_BranchId",
                schema: "MasterBase",
                table: "Feedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedbacks",
                schema: "MasterBase",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Score",
                schema: "MasterBase",
                table: "Feedbacks");

            migrationBuilder.RenameTable(
                name: "Feedbacks",
                schema: "MasterBase",
                newName: "Feedback",
                newSchema: "MasterBase");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_BranchId",
                schema: "MasterBase",
                table: "Feedback",
                newName: "IX_Feedback_BranchId");

            migrationBuilder.AddColumn<string>(
                name: "Mood",
                schema: "MasterBase",
                table: "Feedback",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedback",
                schema: "MasterBase",
                table: "Feedback",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Branches_BranchId",
                schema: "MasterBase",
                table: "Feedback",
                column: "BranchId",
                principalSchema: "MasterBase",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
