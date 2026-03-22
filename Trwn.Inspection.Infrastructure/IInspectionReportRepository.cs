using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure
{
    public interface IInspectionReportRepository
    {
        Task<List<InspectionReport>> GetInspectionReports(int authSessionId);

        Task<InspectionReport?> GetInspectionReport(int id, int authSessionId);

        Task<InspectionReport> AddInspectionReport(InspectionReport report, int authSessionId);

        Task<InspectionReport?> UpdateInspectionReport(int id, InspectionReport report, int authSessionId);

        Task DeleteInspectionReport(int id, int authSessionId);
    }
}