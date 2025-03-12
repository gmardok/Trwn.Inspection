using Trwn.Inspection.Infrastructure;
using Trwn.Inspection.Models;

public class InspectionReportsService: IInspectionReportsService
{
    private readonly IInspectionReportRepository _inspectionReportRepository;

    public InspectionReportsService(IInspectionReportRepository inspectionReportRepository)
    {
        _inspectionReportRepository = inspectionReportRepository;
    }

    public Task<InspectionReport> AddInspectionReport(InspectionReport report)
    {
        return _inspectionReportRepository.AddInspectionReport(report);
    }

    public Task DeleteInspectionReport(Guid id)
    {
        return _inspectionReportRepository.DeleteInspectionReport(id);
    }

    public Task<InspectionReport?> GetInspectionReport(Guid id)
    {
        return _inspectionReportRepository.GetInspectionReport(id);
    }

    public Task<IEnumerable<InspectionReport>> GetInspectionReports()
    {
        return _inspectionReportRepository.GetInspectionReports();
    }

    public Task<InspectionReport?> UpdateInspectionReport(Guid id, InspectionReport report)
    {
        return _inspectionReportRepository.UpdateInspectionReport(id, report);
    }

    public Task<FotoDocumentation?> AddInspectionFoto(Guid id, FotoDocumentation fotoDocumentation)
    {
        return _inspectionReportRepository.AddInspectionFoto(id, fotoDocumentation);
    }

    public Task<FotoDocumentation?> GetInspectionFoto(Guid id, Guid fotoId)
    {
        return _inspectionReportRepository.GetInspectionFoto(id, fotoId);
    }

    public Task DeleteInspectionFoto(Guid id, Guid fotoId)
    {
        return _inspectionReportRepository.DeleteInspectionFoto(id, fotoId);
    }
}