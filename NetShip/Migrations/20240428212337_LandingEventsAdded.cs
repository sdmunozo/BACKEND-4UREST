using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetShip.Migrations
{
    /// <inheritdoc />
    public partial class LandingEventsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEvents",
                schema: "MasterBase");

            migrationBuilder.CreateTable(
                name: "LandingUserEvents",
                schema: "MasterBase",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventTimestamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    PresentationViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_MenuHighlightsViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_MenuScreensViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_ForWhoViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_WhyUsViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_SuscriptionsViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_TestimonialsViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_FaqViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_TrustElementsViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_LinkDestination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details_LinkLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details_PlaybackTime = table.Column<int>(type: "int", nullable: true),
                    Details_Duration = table.Column<int>(type: "int", nullable: true),
                    Details_ImageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details_FAQId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details_Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandingUserEvents", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandingUserEvents",
                schema: "MasterBase");

            migrationBuilder.CreateTable(
                name: "UserEvents",
                schema: "MasterBase",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventTimestamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details_Duration = table.Column<int>(type: "int", nullable: true),
                    Details_FAQId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details_FaqViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_ForWhoViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_ImageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details_LinkDestination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details_LinkLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details_MenuHighlightsViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_MenuScreensViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_PlaybackTime = table.Column<int>(type: "int", nullable: true),
                    PresentationViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details_SuscriptionsViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_TestimonialsViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_TrustElementsViewSecondsElapsed = table.Column<int>(type: "int", nullable: true),
                    Details_WhyUsViewSecondsElapsed = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvents", x => x.UserId);
                });
        }
    }
}
