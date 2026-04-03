using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trwn.Inspection.Data.Migrations
{
    /// <inheritdoc />
    public partial class SyncPendingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contract",
                table: "InspectionReports",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "DprQuantityVerification",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FalseProducts",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MeasuringEquipmentCheck",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherRemarks",
                table: "InspectionReports",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PiecesCuttingCount",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PiecesInBagsCount",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PiecesSewingCount",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ProductEvaluationForm",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ProductIdentification",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "InspectionReports",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<bool>(
                name: "Specification",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TestReport",
                table: "InspectionReports",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "TrimCard",
                table: "InspectionReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contract",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "DprQuantityVerification",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "FalseProducts",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "MeasuringEquipmentCheck",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "OtherRemarks",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "PiecesCuttingCount",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "PiecesInBagsCount",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "PiecesSewingCount",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "ProductEvaluationForm",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "ProductIdentification",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "Specification",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "TestReport",
                table: "InspectionReports");

            migrationBuilder.DropColumn(
                name: "TrimCard",
                table: "InspectionReports");
        }
    }
}
