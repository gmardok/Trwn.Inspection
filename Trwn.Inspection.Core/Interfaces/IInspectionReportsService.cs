using Trwn.Inspection.Models;

public interface IInspectionReportsService
{
    Task<List<InspectionReport>> GetInspectionReports();
    Task<InspectionReport?> GetInspectionReport(string id);
    Task<InspectionReport> AddInspectionReport(InspectionReport report);
    Task<InspectionReport?> UpdateInspectionReport(string id, InspectionReport report);
    Task DeleteInspectionReport(string id);
    Task<FotoDocumentation?> AddInspectionFoto(string id, FotoDocumentation fotoDocumentation);
    Task<FotoDocumentation?> GetInspectionFoto(string id, string fotoId);
    Task<List<FotoDocumentation>> GetAllInspectionFoto(string id);
    Task DeleteInspectionFoto(string id, string fotoId);
}