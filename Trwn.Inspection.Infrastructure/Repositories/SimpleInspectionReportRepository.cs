using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure.Repositories
{
    public class SimpleInspectionReportRepository : IInspectionReportRepository
    {
        private static readonly List<InspectionReport> InspectionReports = new List<InspectionReport>();

        public Task<InspectionReport> AddInspectionReport(InspectionReport report, int authSessionId)
        {
            report.AuthSessionId = authSessionId;
            InspectionReports.Add(report);
            return Task.FromResult(report);
        }

        public Task DeleteInspectionReport(int id, int authSessionId)
        {
            var report = InspectionReports.FirstOrDefault(r => r.Id == id && r.AuthSessionId == authSessionId);
            if (report != null)
            {
                InspectionReports.Remove(report);
            }

            return Task.CompletedTask;
        }

        public Task<InspectionReport?> GetInspectionReport(int id, int authSessionId)
        {
            return Task.FromResult(InspectionReports.FirstOrDefault(r =>
                r.Id == id && r.AuthSessionId == authSessionId));
        }

        public Task<List<InspectionReport>> GetInspectionReports(int authSessionId)
        {
            return Task.FromResult(InspectionReports.Where(r => r.AuthSessionId == authSessionId).ToList());
        }

        public Task<InspectionReport?> UpdateInspectionReport(int id, InspectionReport report, int authSessionId)
        {
            var existingReport = InspectionReports.FirstOrDefault(r =>
                r.Id == id && r.AuthSessionId == authSessionId);
            if (existingReport != null)
            {
                existingReport.InspectionType = report.InspectionType;
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
                existingReport.InspectionResult = report.InspectionResult;
                existingReport.InspectorName = report.InspectorName;
                existingReport.FactoryRepresentative = report.FactoryRepresentative;
                existingReport.PhotoDocumentation = report.PhotoDocumentation;
            }

            return Task.FromResult(existingReport);
        }
    }
}
