using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure
{
    public interface IInspectionReportRepository
    {
        Task<List<InspectionReport>> GetInspectionReports();
        Task<InspectionReport?> GetInspectionReport(string id);
        Task<InspectionReport> AddInspectionReport(InspectionReport report);
        Task<InspectionReport?> UpdateInspectionReport(string id, InspectionReport report);
        Task DeleteInspectionReport(string id);
        Task<PhotoDocumentation?> AddInspectionFoto(string id, PhotoDocumentation photoDocumentation);
        Task<PhotoDocumentation?> GetInspectionFoto(string id, int fotoCode);
        Task<List<PhotoDocumentation>> GetAllInspectionFoto(string id);
        Task DeleteInspectionFoto(string id, int fotoCode);
    }
}