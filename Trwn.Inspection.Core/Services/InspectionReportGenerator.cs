using Microsoft.Extensions.Options;
using Trwn.Inspection.Configuration;
using Trwn.Inspection.Core.Interfaces;
using Trwn.Inspection.Models;
using Trwn.Inspection.Report;

namespace Trwn.Inspection.Core.Services;

public sealed class InspectionReportGenerator : IInspectionReportGenerator
{
    private readonly InspectionReportDocumentGenerator _documentGenerator;

    public InspectionReportGenerator(IOptions<AppSettings> appSettings)
    {
        _documentGenerator = new InspectionReportDocumentGenerator(photoStoragePath: appSettings.Value.PhotoStoragePath);
    }

    public byte[] GenerateDocxReport(InspectionReport report)
    {
        return _documentGenerator.Generate(report);
    }
}
