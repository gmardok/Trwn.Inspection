using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trwn.Inspection.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InspectionReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    InspectionType = table.Column<int>(type: "INTEGER", nullable: false),
                    ReportNo = table.Column<string>(type: "TEXT", nullable: false),
                    Client = table.Column<string>(type: "TEXT", nullable: false),
                    ContractNo = table.Column<string>(type: "TEXT", nullable: false),
                    ArticleName = table.Column<string>(type: "TEXT", nullable: false),
                    Supplier = table.Column<string>(type: "TEXT", nullable: false),
                    Factory = table.Column<string>(type: "TEXT", nullable: false),
                    InspectionPlace = table.Column<string>(type: "TEXT", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    QualityMark = table.Column<string>(type: "TEXT", nullable: false),
                    InspectionStandard = table.Column<string>(type: "TEXT", nullable: false),
                    InspectionSampling = table.Column<string>(type: "TEXT", nullable: false),
                    InspectionQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    SampleSize = table.Column<int>(type: "INTEGER", nullable: false),
                    InspectionCartonNo = table.Column<string>(type: "TEXT", nullable: false),
                    InspectionResult = table.Column<int>(type: "INTEGER", nullable: false),
                    InspectorName = table.Column<string>(type: "TEXT", nullable: false),
                    FactoryRepresentative = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InspectionOrderArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LotNo = table.Column<int>(type: "INTEGER", nullable: false),
                    ArticleNumber = table.Column<string>(type: "TEXT", nullable: false),
                    OrderQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipmentQuantityPcs = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipmentQuantityCartons = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitsPacked = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitsFinishedNotPacked = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitsNotFinished = table.Column<int>(type: "INTEGER", nullable: false),
                    InspectionReportId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionOrderArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionOrderArticles_InspectionReports_InspectionReportId",
                        column: x => x.InspectionReportId,
                        principalTable: "InspectionReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoDocumentations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PhotoType = table.Column<int>(type: "INTEGER", nullable: false),
                    Code = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    PicturePath = table.Column<string>(type: "TEXT", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    InspectionReportId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoDocumentations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoDocumentations_InspectionReports_InspectionReportId",
                        column: x => x.InspectionReportId,
                        principalTable: "InspectionReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InspectionOrderArticles_InspectionReportId",
                table: "InspectionOrderArticles",
                column: "InspectionReportId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionReports_ReportNo",
                table: "InspectionReports",
                column: "ReportNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhotoDocumentations_InspectionReportId",
                table: "PhotoDocumentations",
                column: "InspectionReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InspectionOrderArticles");

            migrationBuilder.DropTable(
                name: "PhotoDocumentations");

            migrationBuilder.DropTable(
                name: "InspectionReports");
        }
    }
}
