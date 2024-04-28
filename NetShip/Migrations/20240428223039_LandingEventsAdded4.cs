using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class LandingEventsAdded4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details_WhyUsViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "WhyUsViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "Details_TrustElementsViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "TrustElementsViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "Details_TestimonialsViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "TestimonialsViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "Details_SuscriptionsViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "SuscriptionsViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "Details_Status",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Details_PlaybackTime",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "PlaybackTime");

            migrationBuilder.RenameColumn(
                name: "Details_MenuScreensViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "MenuScreensViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "Details_MenuHighlightsViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "MenuHighlightsViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "Details_LinkLabel",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "LinkLabel");

            migrationBuilder.RenameColumn(
                name: "Details_LinkDestination",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "LinkDestination");

            migrationBuilder.RenameColumn(
                name: "Details_ImageId",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "ImageId");

            migrationBuilder.RenameColumn(
                name: "Details_ForWhoViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "ForWhoViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "Details_FaqViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "FaqViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "Details_FAQId",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "FAQId");

            migrationBuilder.RenameColumn(
                name: "Details_Duration",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Duration");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "MasterBase",
                table: "LandingUserEvents",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LinkLabel",
                schema: "MasterBase",
                table: "LandingUserEvents",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LinkDestination",
                schema: "MasterBase",
                table: "LandingUserEvents",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                schema: "MasterBase",
                table: "LandingUserEvents",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FAQId",
                schema: "MasterBase",
                table: "LandingUserEvents",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhyUsViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_WhyUsViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "TrustElementsViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_TrustElementsViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "TestimonialsViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_TestimonialsViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "SuscriptionsViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_SuscriptionsViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_Status");

            migrationBuilder.RenameColumn(
                name: "PlaybackTime",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_PlaybackTime");

            migrationBuilder.RenameColumn(
                name: "MenuScreensViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_MenuScreensViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "MenuHighlightsViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_MenuHighlightsViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "LinkLabel",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_LinkLabel");

            migrationBuilder.RenameColumn(
                name: "LinkDestination",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_LinkDestination");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_ImageId");

            migrationBuilder.RenameColumn(
                name: "ForWhoViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_ForWhoViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "FaqViewSecondsElapsed",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_FaqViewSecondsElapsed");

            migrationBuilder.RenameColumn(
                name: "FAQId",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_FAQId");

            migrationBuilder.RenameColumn(
                name: "Duration",
                schema: "MasterBase",
                table: "LandingUserEvents",
                newName: "Details_Duration");

            migrationBuilder.AlterColumn<string>(
                name: "Details_Status",
                schema: "MasterBase",
                table: "LandingUserEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Details_LinkLabel",
                schema: "MasterBase",
                table: "LandingUserEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Details_LinkDestination",
                schema: "MasterBase",
                table: "LandingUserEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Details_ImageId",
                schema: "MasterBase",
                table: "LandingUserEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Details_FAQId",
                schema: "MasterBase",
                table: "LandingUserEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);
        }
    }
}
