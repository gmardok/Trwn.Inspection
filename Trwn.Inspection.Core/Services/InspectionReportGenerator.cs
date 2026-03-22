using System.IO;
using System.Linq;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Trwn.Inspection.Core.Interfaces;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Core.Services;

/// <summary>
/// Generates formatted reports from <see cref="InspectionReport"/> objects.
/// </summary>
public class InspectionReportGenerator : IInspectionReportGenerator
{
    private const string SeparatorLine = "════════════════════════════════════════════════════════════════";
    private const string SectionSeparator = "────────────────────────────────────────────────────────────────";

    /// <inheritdoc />
    public string GenerateTextReport(InspectionReport report)
    {
        if (report == null)
            throw new ArgumentNullException(nameof(report));

        var sb = new StringBuilder();

        // Header
        sb.AppendLine("INSPECTION REPORT");
        sb.AppendLine(SeparatorLine);
        sb.AppendLine();

        // Report identification
        sb.AppendLine("REPORT DETAILS");
        sb.AppendLine(SectionSeparator);
        sb.AppendLine($"Report No:        {report.ReportNo}");
        sb.AppendLine($"Report Name:      {report.Name}");
        sb.AppendLine($"Inspection Type:  {report.InspectionType}");
        sb.AppendLine($"Inspection Date:  {report.InspectionDate:yyyy-MM-dd}");
        sb.AppendLine();

        // Client & contract
        sb.AppendLine("CLIENT & CONTRACT");
        sb.AppendLine(SectionSeparator);
        sb.AppendLine($"Client:           {report.Client}");
        sb.AppendLine($"Contract No:      {report.ContractNo}");
        sb.AppendLine();

        // Article & supplier
        sb.AppendLine("ARTICLE & SUPPLIER");
        sb.AppendLine(SectionSeparator);
        sb.AppendLine($"Article Name:     {report.ArticleName}");
        sb.AppendLine($"Supplier:         {report.Supplier}");
        sb.AppendLine($"Factory:          {report.Factory}");
        sb.AppendLine($"Inspection Place: {report.InspectionPlace}");
        sb.AppendLine();

        // Inspection details
        sb.AppendLine("INSPECTION DETAILS");
        sb.AppendLine(SectionSeparator);
        sb.AppendLine($"Quality Mark:          {report.QualityMark}");
        sb.AppendLine($"Inspection Standard:   {report.InspectionStandard}");
        sb.AppendLine($"Inspection Sampling:   {report.InspectionSampling}");
        sb.AppendLine($"Inspection Quantity:   {report.InspectionQuantity}");
        sb.AppendLine($"Sample Size:           {report.SampleSize}");
        sb.AppendLine($"Inspection Carton No:  {report.InspectionCartonNo}");
        sb.AppendLine($"Inspection Result:     {report.InspectionResult}");
        sb.AppendLine();

        // Inspection order / articles
        if (report.InspectionOrder?.Count > 0)
        {
            sb.AppendLine("INSPECTION ORDER ARTICLES");
            sb.AppendLine(SectionSeparator);
            sb.AppendLine($"{"Lot",4} | {"Article No",-15} | {"Order Qty",10} | {"Ship Pcs",10} | {"Ship Cartons",12} | {"Packed",8} | {"Finished",10} | {"Not Finished",12}");
            sb.AppendLine(SectionSeparator);

            foreach (var article in report.InspectionOrder)
            {
                sb.AppendLine($"{article.LotNo,4} | {article.ArticleNumber,-15} | {article.OrderQuantity,10} | {article.ShipmentQuantityPcs,10} | {article.ShipmentQuantityCartons,12} | {article.UnitsPacked,8} | {article.UnitsFinishedNotPacked,10} | {article.UnitsNotFinished,12}");
            }
            sb.AppendLine();
        }

        // Photo documentation / defects
        if (report.PhotoDocumentation?.Count > 0)
        {
            sb.AppendLine("PHOTO DOCUMENTATION");
            sb.AppendLine(SectionSeparator);
            foreach (var photo in report.PhotoDocumentation)
            {
                sb.AppendLine($"Code: {photo.Code} | Type: {photo.PhotoType} | Count: {photo.Count}");
                sb.AppendLine($"  Description: {photo.Description}");
                sb.AppendLine($"  Path: {photo.PicturePath}");
                sb.AppendLine();
            }
        }

        // Signatures
        sb.AppendLine("SIGNATURES");
        sb.AppendLine(SectionSeparator);
        sb.AppendLine($"Inspector Name:        {report.InspectorName}");
        sb.AppendLine($"Factory Representative: {report.FactoryRepresentative}");
        sb.AppendLine();
        sb.AppendLine(SeparatorLine);
        sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        return sb.ToString();
    }

    /// <inheritdoc />
    public string GenerateHtmlReport(InspectionReport report)
    {
        if (report == null)
            throw new ArgumentNullException(nameof(report));

        var sb = new StringBuilder();

        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang=\"en\">");
        sb.AppendLine("<head>");
        sb.AppendLine("  <meta charset=\"UTF-8\">");
        sb.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        sb.AppendLine("  <title>Inspection Report - " + EscapeHtml(report.ReportNo) + "</title>");
        sb.AppendLine("  <style>");
        sb.AppendLine("    body { font-family: 'Segoe UI', Arial, sans-serif; margin: 20px; color: #333; }");
        sb.AppendLine("    h1 { color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 8px; }");
        sb.AppendLine("    h2 { color: #34495e; margin-top: 24px; }");
        sb.AppendLine("    table { border-collapse: collapse; width: 100%; margin: 16px 0; }");
        sb.AppendLine("    th, td { border: 1px solid #bdc3c7; padding: 8px 12px; text-align: left; }");
        sb.AppendLine("    th { background-color: #ecf0f1; font-weight: 600; }");
        sb.AppendLine("    .section { margin-bottom: 24px; }");
        sb.AppendLine("    .field { margin: 8px 0; }");
        sb.AppendLine("    .field-label { font-weight: 600; display: inline-block; width: 180px; }");
        sb.AppendLine("    .footer { margin-top: 32px; font-size: 0.9em; color: #7f8c8d; }");
        sb.AppendLine("    .result-pass { color: #27ae60; }");
        sb.AppendLine("    .result-pending { color: #f39c12; }");
        sb.AppendLine("    .result-fail { color: #e74c3c; }");
        sb.AppendLine("  </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");

        sb.AppendLine("  <h1>INSPECTION REPORT</h1>");

        sb.AppendLine("  <div class=\"section\">");
        sb.AppendLine("    <h2>Report Details</h2>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Report No:</span> " + EscapeHtml(report.ReportNo) + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Report Name:</span> " + EscapeHtml(report.Name) + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Inspection Type:</span> " + EscapeHtml(report.InspectionType.ToString()) + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Inspection Date:</span> " + report.InspectionDate.ToString("yyyy-MM-dd") + "</div>");
        sb.AppendLine("  </div>");

        sb.AppendLine("  <div class=\"section\">");
        sb.AppendLine("    <h2>Client & Contract</h2>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Client:</span> " + EscapeHtml(report.Client) + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Contract No:</span> " + EscapeHtml(report.ContractNo) + "</div>");
        sb.AppendLine("  </div>");

        sb.AppendLine("  <div class=\"section\">");
        sb.AppendLine("    <h2>Article & Supplier</h2>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Article Name:</span> " + EscapeHtml(report.ArticleName) + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Supplier:</span> " + EscapeHtml(report.Supplier) + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Factory:</span> " + EscapeHtml(report.Factory) + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Inspection Place:</span> " + EscapeHtml(report.InspectionPlace) + "</div>");
        sb.AppendLine("  </div>");

        sb.AppendLine("  <div class=\"section\">");
        sb.AppendLine("    <h2>Inspection Details</h2>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Quality Mark:</span> " + EscapeHtml(report.QualityMark) + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Inspection Standard:</span> " + EscapeHtml(report.InspectionStandard) + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Inspection Sampling:</span> " + EscapeHtml(report.InspectionSampling) + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Inspection Quantity:</span> " + report.InspectionQuantity + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Sample Size:</span> " + report.SampleSize + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Inspection Carton No:</span> " + EscapeHtml(report.InspectionCartonNo) + "</div>");
        var resultClass = report.InspectionResult switch
            {
                InspectionResultType.Passes => "result-pass",
                InspectionResultType.Pending => "result-pending",
                InspectionResultType.Fail => "result-fail",
                _ => ""
            };
            sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Inspection Result:</span> <span class=\"" + resultClass + "\">" + EscapeHtml(report.InspectionResult.ToString()) + "</span></div>");
        sb.AppendLine("  </div>");

        if (report.InspectionOrder?.Count > 0)
        {
            sb.AppendLine("  <div class=\"section\">");
            sb.AppendLine("    <h2>Inspection Order Articles</h2>");
            sb.AppendLine("    <table>");
            sb.AppendLine("      <thead><tr><th>Lot</th><th>Article No</th><th>Order Qty</th><th>Ship Pcs</th><th>Ship Cartons</th><th>Packed</th><th>Finished</th><th>Not Finished</th></tr></thead>");
            sb.AppendLine("      <tbody>");
            foreach (var article in report.InspectionOrder)
            {
                sb.AppendLine($"      <tr><td>{article.LotNo}</td><td>{EscapeHtml(article.ArticleNumber)}</td><td>{article.OrderQuantity}</td><td>{article.ShipmentQuantityPcs}</td><td>{article.ShipmentQuantityCartons}</td><td>{article.UnitsPacked}</td><td>{article.UnitsFinishedNotPacked}</td><td>{article.UnitsNotFinished}</td></tr>");
            }
            sb.AppendLine("      </tbody>");
            sb.AppendLine("    </table>");
            sb.AppendLine("  </div>");
        }

        if (report.PhotoDocumentation?.Count > 0)
        {
            sb.AppendLine("  <div class=\"section\">");
            sb.AppendLine("    <h2>Photo Documentation</h2>");
            sb.AppendLine("    <table>");
            sb.AppendLine("      <thead><tr><th>Code</th><th>Type</th><th>Count</th><th>Description</th><th>Path</th></tr></thead>");
            sb.AppendLine("      <tbody>");
            foreach (var photo in report.PhotoDocumentation)
            {
                sb.AppendLine($"      <tr><td>{photo.Code}</td><td>{photo.PhotoType}</td><td>{photo.Count}</td><td>{EscapeHtml(photo.Description)}</td><td>{EscapeHtml(photo.PicturePath)}</td></tr>");
            }
            sb.AppendLine("      </tbody>");
            sb.AppendLine("    </table>");
            sb.AppendLine("  </div>");
        }

        sb.AppendLine("  <div class=\"section\">");
        sb.AppendLine("    <h2>Signatures</h2>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Inspector Name:</span> " + EscapeHtml(report.InspectorName) + "</div>");
        sb.AppendLine("    <div class=\"field\"><span class=\"field-label\">Factory Representative:</span> " + EscapeHtml(report.FactoryRepresentative) + "</div>");
        sb.AppendLine("  </div>");

        sb.AppendLine("  <div class=\"footer\">Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return sb.ToString();
    }

    /// <inheritdoc />
    public byte[] GeneratePdfReport(InspectionReport report, string? photoStoragePath = null)
    {
        if (report == null)
            throw new ArgumentNullException(nameof(report));

        QuestPDF.Settings.License = LicenseType.Community;

        var majorCount = report.PhotoDocumentation?.Count(p => p.PhotoType == PhotoType.Major) ?? 0;
        var minorCount = report.PhotoDocumentation?.Count(p => p.PhotoType == PhotoType.Minor) ?? 0;
        var majorDefects = report.PhotoDocumentation?.Where(p => p.PhotoType == PhotoType.Major).OrderBy(p => p.Code).ToList() ?? new List<PhotoDocumentation>();
        var minorDefects = report.PhotoDocumentation?.Where(p => p.PhotoType == PhotoType.Minor).OrderBy(p => p.Code).ToList() ?? new List<PhotoDocumentation>();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(8).FontColor(Colors.Black));

                // Top-right logo-like header
                page.Header().Row(row =>
                {
                    row.RelativeItem();
                    row.ConstantItem(140).AlignRight().Column(col =>
                    {
                        col.Item().Text("TRAWENA")
                            .SemiBold().FontSize(18).FontColor(Colors.Black);
                    });
                });

                page.Content().Column(column =>
                {
                    column.Spacing(8);

                    // TYPE OF INSPECTION / RODZAJ INSPEKCJI
                    column.Item().Text("TYPE OF INSPECTION / RODZAJ INSPEKCJI:")
                        .SemiBold();

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Text(t =>
                        {
                            t.Span(report.InspectionType == InspectionType.Final ? "[X] " : "[ ] ");
                            t.Span("FINAL RANDOM INSPECTION / INSPEKCJA KOŃCOWA");
                        });
                        row.RelativeItem().Text(t =>
                        {
                            t.Span(report.InspectionType == InspectionType.ReInspection ? "[X] " : "[ ] ");
                            t.Span("RE-INSPECTION / PONOWNA INSPEKCJA");
                        });
                    });

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Text(t =>
                        {
                            t.Span(report.InspectionType == InspectionType.DuringProduction ? "[X] " : "[ ] ");
                            t.Span("DURING PRODUCTION INSPECTION / INSPEKCJA MIĘDZYOPERACYJNA");
                        });
                        row.RelativeItem();
                    });

                    // Header info lines
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(230).Text("REPORT NO. / NUMER RAPORTU:");
                        row.RelativeItem().Text(report.ReportNo ?? string.Empty);
                    });
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(230).Text("CLIENT/ KLIENT:");
                        row.RelativeItem().Text(report.Client ?? string.Empty);
                    });
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(230).Text("CONTRACT NO. / NUMER ZAMÓWIENIA:");
                        row.RelativeItem().Text(report.ContractNo ?? string.Empty);
                    });
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(230).Text("ARTICLE NAME/ NAZWA WYROBU:");
                        row.RelativeItem().Text(report.ArticleName ?? string.Empty);
                    });
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(230).Text("SUPPLIER/ DOSTAWCA:");
                        row.RelativeItem().Text(report.Supplier ?? string.Empty);
                    });
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(230).Text("DETAILS OF FACTORY/ NAZWA FABRYKI:");
                        row.RelativeItem().Text(report.Factory ?? string.Empty);
                    });
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(230).Text("PLACE OF INSPECTION/ MIEJSCE INSPEKCJI:");
                        row.RelativeItem().Text(report.InspectionPlace ?? string.Empty);
                    });
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(230).Text("DATE OF INSPECTION/ DATA INSPEKCJI:");
                        row.RelativeItem().Text(report.InspectionDate.ToString("dd.MM.yyyy"));
                    });

                    column.Item().PaddingTop(6).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    // Lot / article table
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn(3);
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(90);
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(80);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(3)
                                .Text("Lot No / Nr partii").SemiBold().FontSize(7);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(3)
                                .Text("Art Number / Nr artykułu").SemiBold().FontSize(7);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(3)
                                .Text("Order Quantity Units / Zamówione ilości [szt]").SemiBold().FontSize(7);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(3)
                                .Text("Shipment Quantity Units / Ilość do wysyłki [szt]").SemiBold().FontSize(7);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(3)
                                .Text("Shipment Quantity Cartons / Ilość w kartonach").SemiBold().FontSize(7);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(3)
                                .Text("Units Packed In Cartons / Ilość spakowana").SemiBold().FontSize(7);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(3)
                                .Text("Units Finished Not Packed / Ilość wyprodukowana").SemiBold().FontSize(7);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(3)
                                .Text("Units Not Finished / Ilość niewykończona").SemiBold().FontSize(7);
                        });

                        foreach (var article in report.InspectionOrder)
                        {
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3)
                                .Text(article.LotNo.ToString());
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3)
                                .Text(article.ArticleNumber);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3)
                                .Text(article.OrderQuantity.ToString());
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3)
                                .Text(article.ShipmentQuantityPcs.ToString());
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3)
                                .Text(article.ShipmentQuantityCartons.ToString());
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3)
                                .Text(article.UnitsPacked.ToString());
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3)
                                .Text(article.UnitsFinishedNotPacked.ToString());
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3)
                                .Text(article.UnitsNotFinished.ToString());
                        }
                    });

                    column.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    // Quality mark and standards
                    column.Item().Text("QUALITY MARK / ZNAK JAKOŚCI:")
                        .SemiBold();
                    column.Item().Text(report.QualityMark ?? string.Empty);

                    column.Item().Text("INSPECTION STANDARD / PODSTAWA INSPEKCJI:")
                        .SemiBold();
                    column.Item().Text(report.InspectionStandard ?? string.Empty);

                    column.Item().Text("SAMPLING STANDARD / KONTROLA STANDARDOWA:")
                        .SemiBold();
                    column.Item().Text(report.InspectionSampling ?? string.Empty);

                    column.Item().Row(r =>
                    {
                        r.RelativeItem().Text($"INSPECTION QUANTITY / LICZBA SZTUK DO INSPEKCJI: {report.InspectionQuantity}");
                        r.RelativeItem().Text($"SAMPLE SIZE / WIELKOŚĆ PRÓBY: {report.SampleSize}");
                    });

                    column.Item().Text(
                        $"INSP. CARTON NO. / NUMERY SPRAWDZONYCH KARTONÓW: {report.InspectionCartonNo}");

                    // Defects summary table
                    column.Item().PaddingTop(8).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(80);
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell();
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(3)
                                .Text("Allowed / Dozwolone").SemiBold().FontSize(7);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(3)
                                .Text("Result / Znalezione").SemiBold().FontSize(7);
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(3)
                                .Text("Notes / Notatki").SemiBold().FontSize(7);
                        });

                        // Critical
                        table.Cell().Padding(3).Text("Critical / Krytyczne");
                        table.Cell().Padding(3).Text(string.Empty);
                        table.Cell().Padding(3).Text("0");
                        table.Cell().Padding(3).Text(string.Empty);

                        // Major
                        table.Cell().Padding(3).Text("Major / Poważne – AQL 2.5");
                        table.Cell().Padding(3).Text(string.Empty);
                        table.Cell().Padding(3).Text(majorCount.ToString());
                        table.Cell().Padding(3).Text(string.Empty);

                        // Minor
                        table.Cell().Padding(3).Text("Minor / Średnie – AQL 4.0");
                        table.Cell().Padding(3).Text(string.Empty);
                        table.Cell().Padding(3).Text(minorCount.ToString());
                        table.Cell().Padding(3).Text(string.Empty);
                    });

                    // Result checkboxes
                    column.Item().PaddingTop(6).Row(r =>
                    {
                        r.RelativeItem().Text(t =>
                        {
                            t.Span(report.InspectionResult == InspectionResultType.Passes ? "[X] " : "[ ] ");
                            t.Span("INSPECTION PASSED / OCENA POZYTYWNA");
                        });
                    });
                    column.Item().Row(r =>
                    {
                        r.RelativeItem().Text(t =>
                        {
                            t.Span(report.InspectionResult == InspectionResultType.Pending ? "[X] " : "[ ] ");
                            t.Span("INSPECTION PENDING / OCENA NEUTRALNA");
                        });
                    });
                    column.Item().Row(r =>
                    {
                        r.RelativeItem().Text(t =>
                        {
                            t.Span(report.InspectionResult == InspectionResultType.Fail ? "[X] " : "[ ] ");
                            t.Span("INSPECTION FAILED / OCENA NEGATYWNA");
                        });
                    });

                    // Signatures
                    column.Item().PaddingTop(10).Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Inspector's name / signature or another form of authorization")
                                .FontSize(7);
                            c.Item().Text(
                                "Inspektor / imię i nazwisko / podpis lub inna forma autoryzacji")
                                .FontSize(7);
                            c.Item().PaddingTop(12).LineHorizontal(140).LineColor(Colors.Grey.Medium);
                            c.Item().Text(report.InspectorName ?? string.Empty).FontSize(8);
                            c.Item().Text($"Date / Data: {report.InspectionDate:dd.MM.yyyy}")
                                .FontSize(7);
                        });

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text(
                                    "Factory Representative name / signature or another form of authorization")
                                .FontSize(7);
                            c.Item().Text(
                                "Przedstawiciel fabryki / imię i nazwisko / podpis lub inna forma autoryzacji")
                                .FontSize(7);
                            c.Item().PaddingTop(12).LineHorizontal(140).LineColor(Colors.Grey.Medium);
                            c.Item().Text(report.FactoryRepresentative ?? string.Empty).FontSize(8);
                            c.Item().Text($"Date / Data: {report.InspectionDate:dd.MM.yyyy}")
                                .FontSize(7);
                        });
                    });

                    column.Item().PaddingTop(6).AlignCenter()
                        .Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                        .FontSize(7).FontColor(Colors.Grey.Medium);
                });

                page.Footer()
                    .AlignCenter()
                    .DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.Grey.Medium))
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });

            // Page 2: Major Defects, Minor Defects, Remarks
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(8).FontColor(Colors.Black));

                page.Header().Row(row =>
                {
                    row.RelativeItem();
                    row.ConstantItem(140).AlignRight().Column(col =>
                    {
                        col.Item().Text("TRAWENA").SemiBold().FontSize(18).FontColor(Colors.Black);
                        col.Item().PaddingTop(4).Text("*get a signature or if it is impossible to send in pdf and get confirmation by email / uzyskać podpis lub jeśli to niemożliwe wysłać w pdf-ie i uzyskać potwierdzenie mailem*")
                            .Italic().FontSize(7);
                    });
                });

                page.Content().Column(column =>
                {
                    column.Spacing(10);

                    // MAJOR DEFECTS
                    column.Item().Text("MAJOR DEFECTS / BŁĘDY POWAŻNE").SemiBold();
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4);
                            columns.ConstantColumn(70);
                            columns.ConstantColumn(60);
                        });
                        table.Header(header =>
                        {
                            header.Cell().BorderBottom(1).BorderColor(Colors.Black).Padding(4).Text("Code / Kod ev. Description / Opis").SemiBold();
                            header.Cell().BorderBottom(1).BorderColor(Colors.Black).Padding(4).Text("Picture No / Zdjęcie nr").SemiBold();
                            header.Cell().BorderBottom(1).BorderColor(Colors.Black).Padding(4).AlignRight().Text("Quantity / Ilość").SemiBold();
                        });
                        foreach (var photo in majorDefects)
                        {
                            var idx = majorDefects.IndexOf(photo) + 1;
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Medium).Padding(4).Text($"{idx}. {photo.Description}");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Medium).Padding(4).Text($"1.{idx}");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Medium).Padding(4).AlignRight().Text(photo.Count.ToString());
                        }
                        if (majorDefects.Count == 0)
                        {
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Medium).Padding(4).Text("");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Medium).Padding(4).Text("");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Medium).Padding(4).Text("");
                        }
                    });

                    // MINOR DEFECTS
                    column.Item().PaddingTop(12).Text("MINOR DEFECTS / BŁĘDY ŚREDNIE").SemiBold();
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4);
                            columns.ConstantColumn(70);
                            columns.ConstantColumn(60);
                        });
                        table.Header(header =>
                        {
                            header.Cell().BorderBottom(1).BorderColor(Colors.Black).Padding(4).Text("Code / Kod ev. Description / Opis").SemiBold();
                            header.Cell().BorderBottom(1).BorderColor(Colors.Black).Padding(4).Text("Picture No / Zdjęcie nr").SemiBold();
                            header.Cell().BorderBottom(1).BorderColor(Colors.Black).Padding(4).AlignRight().Text("Quantity / Ilość").SemiBold();
                        });
                        foreach (var photo in minorDefects)
                        {
                            var idx = minorDefects.IndexOf(photo) + 1;
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Medium).Padding(4).Text($"{idx}. {photo.Description}");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Medium).Padding(4).Text($"2.{idx}");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Medium).Padding(4).AlignRight().Text(photo.Count.ToString());
                        }
                        if (minorDefects.Count == 0)
                        {
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Medium).Padding(4).Text("");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Medium).Padding(4).Text("");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Medium).Padding(4).Text("");
                        }
                    });

                    // REMARKS
                    column.Item().PaddingTop(12).Text("REMARKS: / UWAGI:").SemiBold();
                    var remarks = new[]
                    {
                        "Wszystkie wyroby w których wystąpiły błędy poważne należy wymienić na inne bez wad, dziury są nieakceptowalne.",
                        "Asymetria jest niedopuszczalna.",
                        "Należy zwracać uwagę na równość szwów i wykończenie.",
                        "Produkt musi być zgodny z zatwierdzoną specyfikacją i próbką.",
                        "Wszystkie wady muszą być usunięte przed wysyłką.",
                        "Kontrola jakości powinna być przeprowadzona na każdym etapie produkcji.",
                        "Należy zapewnić odpowiednie warunki przechowywania i transportu.",
                        "Proszę o większą kontrolę przy czyszczeniu kompletów na końcowym etapie oraz zwracać uwagę by obcięte nitki nie zostawały na produkcie!"
                    };
                    for (var i = 0; i < remarks.Length; i++)
                    {
                        column.Item().Row(r =>
                        {
                            r.ConstantItem(20).Text($"{i + 1}.").FontColor(Colors.Red.Medium).SemiBold();
                            r.RelativeItem().Text(remarks[i]);
                        });
                    }
                });

                page.Footer().AlignCenter().DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.Grey.Medium))
                    .Text(x => { x.Span("Page "); x.CurrentPageNumber(); x.Span(" of "); x.TotalPages(); });
            });

            // Page 3: Foto Documentation - defects with images
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(8).FontColor(Colors.Black));

                page.Header().Row(row =>
                {
                    row.RelativeItem();
                    row.ConstantItem(140).AlignRight().Text("TRAWENA").SemiBold().FontSize(18).FontColor(Colors.Black);
                });

                page.Content().Column(column =>
                {
                    column.Spacing(10);
                    column.Item().Text("FOTO DOCUMENTATION - DEFECTS / DOKUMENTACJA ZDJĘCIOWA - BŁĘDY").SemiBold();
                    column.Item().PaddingTop(8).Text("1. Major defects / Błędy poważne").SemiBold();

                    var photosDir = !string.IsNullOrEmpty(photoStoragePath)
                        ? Path.Combine(photoStoragePath, report.Id.ToString())
                        : null;
                    var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

                    var filesFromFolder = photosDir != null && Directory.Exists(photosDir)
                        ? Directory.GetFiles(photosDir)
                            .Where(f => imageExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                            .OrderBy(Path.GetFileName)
                            .ToArray()
                        : Array.Empty<string>();

                    var photosToRender = new List<(string? Path, string Caption)>();
                    var majorIdx = 0;
                    var minorIdx = 0;
                    foreach (var photo in report.PhotoDocumentation ?? new List<PhotoDocumentation>())
                    {
                        var imgPath = !string.IsNullOrEmpty(photoStoragePath)
                            ? Path.Combine(photoStoragePath, photo.PicturePath)
                            : null;
                        var caption = photo.PhotoType == PhotoType.Major
                            ? $"{++majorIdx}. {photo.Description}"
                            : $"{++minorIdx}. {photo.Description}";
                        photosToRender.Add((imgPath, caption));
                    }
                    foreach (var filePath in filesFromFolder)
                    {
                        if (photosToRender.Any(p => string.Equals(p.Path, filePath, StringComparison.OrdinalIgnoreCase)))
                            continue;
                        photosToRender.Add((filePath, Path.GetFileNameWithoutExtension(filePath)));
                    }

                    for (var i = 0; i < photosToRender.Count; i += 2)
                    {
                        var left = photosToRender[i];
                        var right = i + 1 < photosToRender.Count ? photosToRender[i + 1] : (Path: (string?)null, Caption: "");
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().PaddingRight(8).Column(c =>
                            {
                                if (left.Path != null && File.Exists(left.Path))
                                {
                                    try { c.Item().MaxHeight(120).Image(left.Path); } catch { }
                                }
                                else
                                    c.Item().Height(80).Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle().Text("[Image]");
                                c.Item().PaddingTop(4).Text(left.Caption).FontSize(7);
                            });
                            row.RelativeItem().Column(c =>
                            {
                                if (right.Path != null && File.Exists(right.Path))
                                {
                                    try { c.Item().MaxHeight(120).Image(right.Path); } catch { }
                                }
                                else if (right.Path != null)
                                    c.Item().Height(80).Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle().Text("[Image]");
                                else
                                    c.Item();
                                if (!string.IsNullOrEmpty(right.Caption))
                                    c.Item().PaddingTop(4).Text(right.Caption).FontSize(7);
                            });
                        });
                    }

                    if (photosToRender.Count == 0 && filesFromFolder.Length > 0)
                    {
                        for (var i = 0; i < filesFromFolder.Length; i += 2)
                        {
                            var left = filesFromFolder[i];
                            var right = i + 1 < filesFromFolder.Length ? filesFromFolder[i + 1] : null;
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().PaddingRight(8).Column(c =>
                                {
                                    try { c.Item().MaxHeight(120).Image(left); } catch { }
                                    c.Item().PaddingTop(4).Text(Path.GetFileNameWithoutExtension(left)).FontSize(7);
                                });
                                row.RelativeItem().Column(c =>
                                {
                                    if (right != null)
                                    {
                                        try { c.Item().MaxHeight(120).Image(right); } catch { }
                                        c.Item().PaddingTop(4).Text(Path.GetFileNameWithoutExtension(right)).FontSize(7);
                                    }
                                    else
                                        c.Item();
                                });
                            });
                        }
                    }
                });

                page.Footer().AlignCenter().DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.Grey.Medium))
                    .Text(x => { x.Span("Page "); x.CurrentPageNumber(); x.Span(" of "); x.TotalPages(); });
            });
        });

        return document.GeneratePdf();
    }

    private static string EscapeHtml(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        return System.Net.WebUtility.HtmlEncode(value);
    }
}
