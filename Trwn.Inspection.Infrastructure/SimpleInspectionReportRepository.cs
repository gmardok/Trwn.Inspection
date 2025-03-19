using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure
{
    public class SimpleInspectionReportRepository : IInspectionReportRepository
    {
        private static readonly List<InspectionReport> InspectionReports = new List<InspectionReport>();

        public Task<InspectionReport> AddInspectionReport(InspectionReport report)
        {
            report.Id = Guid.NewGuid().ToString();
            InspectionReports.Add(report);
            return Task.FromResult(report);
        }

        public Task DeleteInspectionReport(string id)
        {
            var report = InspectionReports.FirstOrDefault(r => r.Id == id);
            if (report != null)
            {
                InspectionReports.Remove(report);
            }
            return Task.CompletedTask;
        }

        public Task<InspectionReport?> GetInspectionReport(string id)
        {
            return Task.FromResult(InspectionReports.FirstOrDefault(r => r.Id == id));
        }

        public Task<List<InspectionReport>> GetInspectionReports()
        {
            return Task.FromResult<List<InspectionReport>>(InspectionReports);
        }

        public Task<InspectionReport?> UpdateInspectionReport(string id, InspectionReport report)
        {
            var existingReport = InspectionReports.FirstOrDefault(r => r.Id == id);
            if (existingReport != null)
            {
                existingReport.InspectionType = report.InspectionType;
                existingReport.Name = report.Name;
                existingReport.Inspector = report.Inspector;
                existingReport.ReportNo = report.ReportNo;
                existingReport.Client = report.Client;
                existingReport.ContractNo = report.ContractNo;
                existingReport.ArticleName = report.ArticleName;
                existingReport.Supplier = report.Supplier;
                existingReport.Factory = report.Factory;
                existingReport.InspectionPlace = report.InspectionPlace;
                existingReport.InspectionDate = report.InspectionDate;
                existingReport.InspectionOrder = report.InspectionOrder;
                existingReport.QualityMark = report.QualityMark;
                existingReport.InspectionStandard = report.InspectionStandard;
                existingReport.InspectionSampling = report.InspectionSampling;
                existingReport.InspectionQuantity = report.InspectionQuantity;
                existingReport.SampleSize = report.SampleSize;
                existingReport.InspectionCartonNo = report.InspectionCartonNo;
                existingReport.DefectsSummary = report.DefectsSummary;
                existingReport.InspectionResult = report.InspectionResult;
                existingReport.InspectorName = report.InspectorName;
                existingReport.FactoryRepresentative = report.FactoryRepresentative;
                existingReport.InspectionDefects = report.InspectionDefects;
                existingReport.Remarks = report.Remarks;
                existingReport.ProductionStatus = report.ProductionStatus;
                existingReport.ListOfDocuments = report.ListOfDocuments;
                existingReport.FotoDocumentation = report.FotoDocumentation;
            }
            return Task.FromResult(existingReport);
        }

        public Task<FotoDocumentation?> AddInspectionFoto(string id, FotoDocumentation fotoDocumentation)
        {
            var existingReport = InspectionReports.FirstOrDefault(r => r.Id == id);
            if (existingReport != null)
            {
                fotoDocumentation.Id = Guid.NewGuid().ToString();
                existingReport.FotoDocumentation.Add(fotoDocumentation);

                return Task.FromResult<FotoDocumentation?>(fotoDocumentation);
            }
            return Task.FromResult<FotoDocumentation?>(null);
        }

        public Task DeleteInspectionFoto(string id, string fotoId)
        {
            var existingReport = InspectionReports.FirstOrDefault(r => r.Id == id);
            if (existingReport != null)
            {
                var foto = existingReport.FotoDocumentation.FirstOrDefault(f => f.Id == fotoId);
                if (foto != null)
                {
                    existingReport.FotoDocumentation.Remove(foto);
                }
            }
            return Task.CompletedTask;
        }

        public Task<FotoDocumentation?> GetInspectionFoto(string id, string fotoId)
        {
            var existingReport = InspectionReports.FirstOrDefault(r => r.Id == id);
            if (existingReport != null)
            {
                return Task.FromResult(existingReport.FotoDocumentation.FirstOrDefault(f => f.Id == fotoId));
            }
            return Task.FromResult<FotoDocumentation?>(null);
        }

        public Task<List<FotoDocumentation>> GetAllInspectionFoto(string id)
        {
            var existingReport = InspectionReports.FirstOrDefault(r => r.Id == id);
            return Task.FromResult(existingReport?.FotoDocumentation ?? new List<FotoDocumentation>());
        }
    }
}
