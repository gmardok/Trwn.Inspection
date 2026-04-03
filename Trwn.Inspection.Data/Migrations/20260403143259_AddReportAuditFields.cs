using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trwn.Inspection.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReportAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "InspectionReports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "InspectionReports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedByUserId",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionReports_UpdatedByUserId",
                table: "InspectionReports",
                column: "UpdatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionReports_Users_UpdatedByUserId",
                table: "InspectionReports",
                column: "UpdatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InspectionReports_Users_UpdatedByUserId",
                table: "InspectionReports");

            migrationBuilder.DropIndex(
                name: "IX_InspectionReports_UpdatedByUserId",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "InspectionReports");
        }
    }
}
