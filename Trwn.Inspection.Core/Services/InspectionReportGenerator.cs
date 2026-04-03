using Trwn.Inspection.Core.Interfaces;
using Trwn.Inspection.Models;
using Trwn.Inspection.Report;

namespace Trwn.Inspection.Core.Services;

public sealed class InspectionReportGenerator : IInspectionReportGenerator
{
    private readonly InspectionReportDocumentGenerator _documentGenerator;

    public InspectionReportGenerator()
    {
        _documentGenerator = new InspectionReportDocumentGenerator();
    }

    public byte[] GenerateDocxReport(InspectionReport report)
    {
        return _documentGenerator.Generate(report);
    }
}
