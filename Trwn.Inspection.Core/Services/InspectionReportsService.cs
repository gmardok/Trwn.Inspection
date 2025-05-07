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

    public Task DeleteInspectionReport(string id)
    {
        return _inspectionReportRepository.DeleteInspectionReport(id);
    }

    public Task<InspectionReport?> GetInspectionReport(string id)
    {
        return _inspectionReportRepository.GetInspectionReport(id);
    }

    public async Task<List<InspectionReport>> GetInspectionReports()
    {
        return await _inspectionReportRepository.GetInspectionReports();
    }

    public Task<InspectionReport?> UpdateInspectionReport(string id, InspectionReport report)
    {
        return _inspectionReportRepository.UpdateInspectionReport(id, report);
    }

    public Task<PhotoDocumentation?> AddInspectionFoto(string id, PhotoDocumentation photoDocumentation)
    {
        return _inspectionReportRepository.AddInspectionFoto(id, photoDocumentation);
    }

    public Task<PhotoDocumentation?> GetInspectionFoto(string id, int fotoCode)
    {
        return _inspectionReportRepository.GetInspectionFoto(id, fotoCode);
    }

    public Task DeleteInspectionFoto(string id, int fotoCode)
    {
        return _inspectionReportRepository.DeleteInspectionFoto(id, fotoCode);
    }

    public Task<List<PhotoDocumentation>> GetAllInspectionFoto(string id)
    {
        return _inspectionReportRepository.GetAllInspectionFoto(id);
    }
}