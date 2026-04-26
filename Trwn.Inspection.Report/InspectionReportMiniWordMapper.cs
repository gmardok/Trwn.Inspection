using System.Globalization;
using MiniSoftware;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Report;

/// <summary>
/// Maps <see cref="InspectionReport"/> to a MiniWord value dictionary (see <c>Templates/InspectionReportTemplate.docx</c>).
/// Photo rows: <c>PhotoMajor</c> / <c>PhotoMinor</c> use <see cref="PhotoType"/>; each row has Description, PictureLabel (code), Count, and Photo (<see cref="MiniWordPicture"/> when <see cref="PhotoDocumentation.PicturePath"/> exists).
/// Additional lists <c>PhotoShippingMark</c>, <c>PhotoPackaging</c>, <c>PhotoPackageWithDeffects</c> are supplied for custom template tags (same row shape).
/// </summary>
public static class InspectionReportMiniWordMapper
{
    public static Dictionary<string, object> ToDictionary(InspectionReport report, string photoStoragePath)
    {
        var majorDefects = report.PhotoDocumentation.Where(p => p.PhotoType == PhotoType.Major).ToList();
        var minorDefects = report.PhotoDocumentation.Where(p => p.PhotoType == PhotoType.Minor).ToList();
        var d = new Dictionary<string, object>
        {
            ["ReportNo"] = report.ReportNo,
            ["Client"] = report.Client,
            ["ContractNo"] = report.ContractNo,
            ["ArticleName"] = report.ArticleName,
            ["Supplier"] = report.Supplier,
            ["Factory"] = report.Factory,
            ["InspectionPlace"] = report.InspectionPlace,
            ["InspectionDate"] = report.InspectionDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
            ["QualityMark"] = report.QualityMark,
            ["InspectionStandard"] = report.InspectionStandard,
            ["InspectionSampling"] = report.InspectionSampling,
            ["InspectionQuantity"] = report.InspectionQuantity,
            ["SampleSize"] = report.SampleSize,
            ["InspectionCartonNo"] = report.InspectionCartonNo,
            ["InspectorName"] = report.InspectorName,
            ["FactoryRepresentative"] = report.FactoryRepresentative,
            ["InspectorDate"] = report.InspectionDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
            ["FactoryRepDate"] = report.InspectionDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
            ["Remarks"] = string.Join(Environment.NewLine, report.Remarks),
            ["PiecesCuttingCount"] = report.PiecesCuttingCount,
            ["PiecesSewingCount"] = report.PiecesSewingCount,
            ["PiecesInBagsCount"] = report.PiecesInBagsCount,
            ["Contract"] = report.Contract,
            ["TestReport"] = report.TestReport,
            ["OtherRemarks"] = report.OtherRemarks,
            ["MeasuringEquipmentCheck"] = FormatNullablePassFail(report.MeasuringEquipmentCheck),
            ["FalseProducts"] = report.FalseProducts ? "WYSTĘPUJĄ" : "NIE WYSTĘPUJĄ",
            ["DprQuantityVerification"] = FormatBoolPassFail(report.DprQuantityVerification),
            ["ProductIdentification"] = report.ProductIdentification ? "JEST" : "BRAK",
            ["Specification"] = report.Specification ? "JEST" : "NIE",
            ["ProductEvaluationForm"] = report.ProductEvaluationForm ? "JEST" : "NIE",
            ["TrimCard"] = report.TrimCard ? "JEST" : "NIE",
            ["MarkFinal"] = Mark(report.InspectionType == InspectionType.Final),
            ["MarkDuring"] = Mark(report.InspectionType == InspectionType.DuringProduction),
            ["MarkRe"] = Mark(report.InspectionType == InspectionType.ReInspection),
            ["MarkResultPass"] = Mark(report.InspectionResult == InspectionResultType.Passes),
            ["MarkResultPending"] = Mark(report.InspectionResult == InspectionResultType.Pending),
            ["MarkResultFail"] = Mark(report.InspectionResult == InspectionResultType.Fail),
            ["InspectionOrder"] = BuildInspectionOrderRows(report),
            ["PhotoMajor"] = BuildPhotoRows(report, PhotoType.Major, photoStoragePath),
            ["PhotoMinor"] = BuildPhotoRows(report, PhotoType.Minor, photoStoragePath),
            ["PhotoShippingMark"] = BuildPhotoRows(report, PhotoType.ShippingMark, photoStoragePath),
            ["PhotoPackaging"] = BuildPhotoRows(report, PhotoType.Packaging, photoStoragePath),
            ["PhotoPackageWithDeffects"] = BuildPhotoRows(report, PhotoType.PackageWithDeffects, photoStoragePath),
            ["MajorAllowed"] = "0",//majorDefects.Sum(d => d.),
            ["MajorFound"] = "0",
            ["MinorAllowed"] = "0",
            ["MinorFound"] = "0"
        };
        return d;
    }

    private static string Mark(bool on) => on ? "X" : " ";

    private static string FormatNullablePassFail(bool? value) =>
        value switch { true => "POZYTYWNE", false => "NEGATYWNE", null => "—" };

    private static string FormatBoolPassFail(bool value) => value ? "POZYTYWNE" : "NEGATYWNE";

    private static List<Dictionary<string, object>> BuildInspectionOrderRows(InspectionReport report)
    {
        var list = new List<Dictionary<string, object>>();
        foreach (var line in report.InspectionOrder)
        {
            var oq = line.OrderQuantity;
            var row = new Dictionary<string, object>
            {
                ["LotNo"] = line.LotNo,
                ["ArticleNumber"] = line.ArticleNumber,
                ["OrderQuantity"] = line.OrderQuantity,
                ["ShipmentQuantityPcs"] = line.ShipmentQuantityPcs,
                ["ShipmentQuantityCartons"] = line.ShipmentQuantityCartons,
                ["UnitsPacked"] = line.UnitsPacked,
                ["UnitsPackedPct"] = Pct(line.UnitsPacked, oq),
                ["UnitsFinishedNotPacked"] = line.UnitsFinishedNotPacked,
                ["UnitsFinishedNotPackedPct"] = Pct(line.UnitsFinishedNotPacked, oq),
                ["UnitsNotFinished"] = line.UnitsNotFinished,
                ["UnitsNotFinishedPct"] = Pct(line.UnitsNotFinished, oq),
            };
            list.Add(row);
        }

        if (list.Count == 0)
        {
            list.Add(new Dictionary<string, object>
            {
                ["LotNo"] = "",
                ["ArticleNumber"] = "",
                ["OrderQuantity"] = "",
                ["ShipmentQuantityPcs"] = "",
                ["ShipmentQuantityCartons"] = "",
                ["UnitsPacked"] = "",
                ["ShipmentPcsPct"] = "",
                ["UnitsFinishedNotPacked"] = "",
                ["UnitsFinishedPct"] = "",
                ["UnitsNotFinished"] = "",
                ["UnitsNotFinishedPct"] = "",
            });
        }

        return list;
    }

    private static string Pct(int part, int total)
    {
        if (total <= 0) return "";
        var v = (int)Math.Round(100.0 * part / total, MidpointRounding.AwayFromZero);
        return v.ToString(CultureInfo.InvariantCulture);
    }

    private static List<Dictionary<string, object>> BuildPhotoRows(InspectionReport report, PhotoType type, string photoStoragePath)
    {
        const int photoWidth = 200;
        const int photoHeight = 150;
        var list = new List<Dictionary<string, object>>();
        foreach (var p in report.PhotoDocumentation)
        {
            if (p.PhotoType != type) continue;
            object photoValue = "";
            var fullPath = Path.Combine(photoStoragePath, p.PicturePath);
            if (!string.IsNullOrWhiteSpace(p.PicturePath) && File.Exists(fullPath))
            {
                photoValue = new MiniWordPicture
                {
                    Path = Path.GetFullPath(fullPath),
                    Width = photoWidth,
                    Height = photoHeight,
                };
            }

            list.Add(new Dictionary<string, object>
            {
                ["Description"] = p.Description,
                ["PictureLabel"] = p.Code.ToString(CultureInfo.InvariantCulture),
                ["Photo"] = photoValue,
                ["Count"] = p.Count,
            });
        }

        if (list.Count == 0)
        {
            list.Add(new Dictionary<string, object>
            {
                ["Description"] = "",
                ["PictureLabel"] = "",
                ["Photo"] = "",
                ["Count"] = "",
            });
        }

        return list;
    }
}
