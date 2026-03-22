using Trwn.Inspection.Models;

namespace Trwn.Inspection.Core.Interfaces;

/// <summary>
/// Generates formatted reports from <see cref="InspectionReport"/> objects.
/// </summary>
public interface IInspectionReportGenerator
{
    /// <summary>
    /// Generates a plain text report from the given inspection report.
    /// </summary>
    /// <param name="report">The inspection report to generate the report from.</param>
    /// <returns>A formatted plain text report.</returns>
    string GenerateTextReport(InspectionReport report);

    /// <summary>
    /// Generates an HTML report from the given inspection report.
    /// </summary>
    /// <param name="report">The inspection report to generate the report from.</param>
    /// <returns>A formatted HTML report.</returns>
    string GenerateHtmlReport(InspectionReport report);

    /// <summary>
    /// Generates a PDF report from the given inspection report.
    /// </summary>
    /// <param name="report">The inspection report to generate the report from.</param>
    /// <param name="photoStoragePath">Optional base path for photo files (e.g. path to Photos folder). If null, images are not embedded.</param>
    /// <returns>The PDF document as a byte array.</returns>
    byte[] GeneratePdfReport(InspectionReport report, string? photoStoragePath = null);
}
