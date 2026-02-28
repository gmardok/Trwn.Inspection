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

    public Task DeleteInspectionReport(int id)
    {
        return _inspectionReportRepository.DeleteInspectionReport(id);
    }

    public Task<InspectionReport?> GetInspectionReport(int id)
    {
        return _inspectionReportRepository.GetInspectionReport(id);
    }

    public async Task<List<InspectionReport>> GetInspectionReports()
    {
        return await _inspectionReportRepository.GetInspectionReports();
    }

    public Task<InspectionReport?> UpdateInspectionReport(int id, InspectionReport report)
    {
        return _inspectionReportRepository.UpdateInspectionReport(id, report);
    }
}