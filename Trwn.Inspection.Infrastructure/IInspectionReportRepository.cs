using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure
{
    public interface IInspectionReportRepository
    {
        Task<List<InspectionReport>> GetInspectionReports();
        Task<InspectionReport?> GetInspectionReport(int id);
        Task<InspectionReport> AddInspectionReport(InspectionReport report);
        Task<InspectionReport?> UpdateInspectionReport(int id, InspectionReport report);
        Task DeleteInspectionReport(int id);
        Task<PhotoDocumentation?> AddInspectionFoto(int id, PhotoDocumentation photoDocumentation);
        Task<PhotoDocumentation?> GetInspectionFoto(int id, int fotoCode);
        Task<List<PhotoDocumentation>> GetAllInspectionFoto(int id);
        Task DeleteInspectionFoto(int id, int fotoCode);
    }
}