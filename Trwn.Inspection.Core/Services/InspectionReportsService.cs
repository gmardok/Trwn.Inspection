using Trwn.Inspection.Core;
using Trwn.Inspection.Infrastructure;
using Trwn.Inspection.Models;

public class InspectionReportsService : IInspectionReportsService
{
    private readonly IInspectionReportRepository _inspectionReportRepository;
    private readonly IUserContext _userContext;

    public InspectionReportsService(
        IInspectionReportRepository inspectionReportRepository,
        IUserContext userContext)
    {
        _inspectionReportRepository = inspectionReportRepository;
        _userContext = userContext;
    }

    private int UserId =>
        _userContext.GetUserId()
        ?? throw new InvalidOperationException("User identity is not available.");

    public Task<InspectionReport> AddInspectionReport(InspectionReport report)
    {
        return _inspectionReportRepository.AddInspectionReport(report, UserId);
    }

    public Task DeleteInspectionReport(int id)
    {
        return _inspectionReportRepository.DeleteInspectionReport(id);
    }

    public Task<InspectionReport?> GetInspectionReport(int id)
    {
        return _inspectionReportRepository.GetInspectionReport(id);
    }

    public Task<List<InspectionReport>> GetInspectionReports()
    {
        return _inspectionReportRepository.GetInspectionReports();
    }

    public Task<InspectionReport?> UpdateInspectionReport(int id, InspectionReport report)
    {
        return _inspectionReportRepository.UpdateInspectionReport(id, report, UserId);
    }
}
