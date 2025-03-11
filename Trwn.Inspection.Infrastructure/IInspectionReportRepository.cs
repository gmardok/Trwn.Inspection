using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure
{
    public interface IInspectionReportRepository
    {
        Task<IEnumerable<InspectionReport>> GetInspectionReports();
        Task<InspectionReport?> GetInspectionReport(Guid id);
        Task<InspectionReport> AddInspectionReport(InspectionReport report);
        Task<InspectionReport?> UpdateInspectionReport(Guid id, InspectionReport report);
        Task DeleteInspectionReport(Guid id);
    }
}