using Trwn.Inspection.Models;

public interface IInspectionReportsService
{
    Task<IEnumerable<InspectionReport>> GetInspectionReports();
    Task<InspectionReport?> GetInspectionReport(Guid id);
    Task<InspectionReport> AddInspectionReport(InspectionReport report);
    Task<InspectionReport?> UpdateInspectionReport(Guid id, InspectionReport report);
    Task DeleteInspectionReport(Guid id);
}