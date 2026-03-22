using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trwn.Inspection.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInspectionReportAuthSessionFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthSessionId",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionReports_AuthSessionId",
                table: "InspectionReports",
                column: "AuthSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionReports_AuthSessions_AuthSessionId",
                table: "InspectionReports",
                column: "AuthSessionId",
                principalTable: "AuthSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InspectionReports_AuthSessions_AuthSessionId",
                table: "InspectionReports");

            migrationBuilder.DropIndex(
                name: "IX_InspectionReports_AuthSessionId",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "AuthSessionId",
                table: "InspectionReports");
        }
    }
}
