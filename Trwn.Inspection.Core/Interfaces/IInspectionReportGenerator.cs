using Trwn.Inspection.Models;

namespace Trwn.Inspection.Core.Interfaces;

/// <summary>
/// Generates formatted reports from <see cref="InspectionReport"/> objects.
/// </summary>
public interface IInspectionReportGenerator
{
    /// <summary>
    /// Generates a Word document report from the given inspection report.
    /// </summary>
    /// <param name="report">The inspection report to generate the document from.</param>
    /// <returns>The Word document as a byte array.</returns>
    byte[] GenerateDocxReport(InspectionReport report);
}
