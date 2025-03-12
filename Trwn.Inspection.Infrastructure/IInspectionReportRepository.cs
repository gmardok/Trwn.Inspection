﻿using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure
{
    public interface IInspectionReportRepository
    {
        Task<IEnumerable<InspectionReport>> GetInspectionReports();
        Task<InspectionReport?> GetInspectionReport(Guid id);
        Task<InspectionReport> AddInspectionReport(InspectionReport report);
        Task<InspectionReport?> UpdateInspectionReport(Guid id, InspectionReport report);
        Task DeleteInspectionReport(Guid id);
        Task<FotoDocumentation?> AddInspectionFoto(Guid id, FotoDocumentation fotoDocumentation);
        Task<FotoDocumentation?> GetInspectionFoto(Guid id, Guid fotoId);
        Task DeleteInspectionFoto(Guid id, Guid fotoId);
    }
}