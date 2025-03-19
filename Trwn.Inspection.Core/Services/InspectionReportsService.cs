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

    public Task<FotoDocumentation?> AddInspectionFoto(string id, FotoDocumentation fotoDocumentation)
    {
        return _inspectionReportRepository.AddInspectionFoto(id, fotoDocumentation);
    }

    public Task<FotoDocumentation?> GetInspectionFoto(string id, string fotoId)
    {
        return _inspectionReportRepository.GetInspectionFoto(id, fotoId);
    }

    public Task DeleteInspectionFoto(string id, string fotoId)
    {
        return _inspectionReportRepository.DeleteInspectionFoto(id, fotoId);
    }

    public Task<List<FotoDocumentation>> GetAllInspectionFoto(string id)
    {
        return _inspectionReportRepository.GetAllInspectionFoto(id);
    }
}