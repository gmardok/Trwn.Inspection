using Trwn.Inspection.Infrastructure;
using Trwn.Inspection.Models;

public class InspectionReportsService : IInspectionReportsService
{
    private readonly IInspectionReportRepository _inspectionReportRepository;
    private readonly IAuthSessionContext _authSessionContext;

    public InspectionReportsService(
        IInspectionReportRepository inspectionReportRepository,
        IAuthSessionContext authSessionContext)
    {
        _inspectionReportRepository = inspectionReportRepository;
        _authSessionContext = authSessionContext;
    }

    private int SessionId =>
        _authSessionContext.GetSessionId()
        ?? throw new InvalidOperationException("Auth session is not available.");

    public Task<InspectionReport> AddInspectionReport(InspectionReport report)
    {
        return _inspectionReportRepository.AddInspectionReport(report, SessionId);
    }

    public Task DeleteInspectionReport(int id)
    {
        return _inspectionReportRepository.DeleteInspectionReport(id, SessionId);
    }

    public Task<InspectionReport?> GetInspectionReport(int id)
    {
        return _inspectionReportRepository.GetInspectionReport(id, SessionId);
    }

    public async Task<List<InspectionReport>> GetInspectionReports()
    {
        return await _inspectionReportRepository.GetInspectionReports(SessionId);
    }

    public Task<InspectionReport?> UpdateInspectionReport(int id, InspectionReport report)
    {
        return _inspectionReportRepository.UpdateInspectionReport(id, report, SessionId);
    }
}
