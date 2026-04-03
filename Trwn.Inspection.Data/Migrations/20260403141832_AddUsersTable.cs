using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trwn.Inspection.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AuthSessions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InspectionReports_UserId",
                table: "InspectionReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthSessions_UserId",
                table: "AuthSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthSessions_Users_UserId",
                table: "AuthSessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionReports_Users_UserId",
                table: "InspectionReports",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // Back-fill: create one User per distinct email found in existing AuthSessions.
            migrationBuilder.Sql(@"
                INSERT INTO Users (Email, CreatedAtUtc)
                SELECT Email, MIN(CreatedAtUtc) FROM AuthSessions GROUP BY Email
            ");

            // Link every AuthSession to its User.
            migrationBuilder.Sql(@"
                UPDATE AuthSessions
                SET UserId = (SELECT Id FROM Users WHERE Users.Email = AuthSessions.Email)
            ");

            // Link every InspectionReport to its User via its AuthSession.
            migrationBuilder.Sql(@"
                UPDATE InspectionReports
                SET UserId = (
                    SELECT s.UserId
                    FROM AuthSessions s
                    WHERE s.Id = InspectionReports.AuthSessionId
                )
                WHERE AuthSessionId IS NOT NULL
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthSessions_Users_UserId",
                table: "AuthSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_InspectionReports_Users_UserId",
                table: "InspectionReports");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_InspectionReports_UserId",
                table: "InspectionReports");

            migrationBuilder.DropIndex(
                name: "IX_AuthSessions_UserId",
                table: "AuthSessions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AuthSessions");
        }
    }
}
