using MiniSoftware;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Report;

/// <summary>
/// Generates a Word inspection report from <see cref="InspectionReport"/> using the MiniWord template.
/// </summary>
public class InspectionReportDocumentGenerator
{
    private readonly byte[] _templateBytes;

    /// <param name="templatePath">Optional path to the .docx template. Defaults to <c>Templates/InspectionReportTemplate.docx</c> next to the assembly.</param>
    public InspectionReportDocumentGenerator(string? templatePath = null)
    {
        var path = templatePath ?? Path.Combine(AppContext.BaseDirectory, "Templates", "InspectionReportTemplate.docx");
        if (!File.Exists(path))
            throw new FileNotFoundException("Inspection report template not found.", path);
        _templateBytes = File.ReadAllBytes(path);
    }

    public void Write(Stream output, InspectionReport report)
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(report);
        var value = InspectionReportMiniWordMapper.ToDictionary(report);
        MiniWord.SaveAsByTemplate(output, _templateBytes, value);
    }

    public byte[] Generate(InspectionReport report)
    {
        using var ms = new MemoryStream();
        Write(ms, report);
        return ms.ToArray();
    }
}
