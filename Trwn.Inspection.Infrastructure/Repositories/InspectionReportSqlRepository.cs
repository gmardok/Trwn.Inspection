using Microsoft.EntityFrameworkCore;
using Trwn.Inspection.Data;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure.Repositories
{
    public class InspectionReportSqlRepository : IInspectionReportRepository
    {
        private readonly InspectionDbContext _context;

        public InspectionReportSqlRepository(InspectionDbContext context)
        {
            _context = context;
        }

        public async Task<List<InspectionReport>> GetInspectionReports(int authSessionId)
        {
            return await _context.InspectionReports
                .Include(r => r.InspectionOrder)
                .Include(r => r.PhotoDocumentation)
                //.Where(r => r.AuthSessionId == authSessionId)
                .OrderBy(r => r.Id)
                .ToListAsync();
        }

        public async Task<InspectionReport?> GetInspectionReport(int id, int authSessionId)
        {
            return await _context.InspectionReports
                .Include(r => r.InspectionOrder)
                .Include(r => r.PhotoDocumentation)
                .FirstOrDefaultAsync(r => r.Id == id);// && r.AuthSessionId == authSessionId);
        }

        public async Task<InspectionReport> AddInspectionReport(InspectionReport report, int authSessionId)
        {
            report.Id = 0;
            report.AuthSessionId = authSessionId;
            report.AuthSession = null;
            _context.InspectionReports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<InspectionReport?> UpdateInspectionReport(int id, InspectionReport report, int authSessionId)
        {
            var existing = await _context.InspectionReports
                .Include(r => r.InspectionOrder)
                .Include(r => r.PhotoDocumentation)
                .FirstOrDefaultAsync(r => r.Id == id);// && r.AuthSessionId == authSessionId);

            if (existing == null)
            {
                return null;
            }

            existing.Name = report.Name;
            existing.InspectionType = report.InspectionType;
            existing.ReportNo = report.ReportNo;
            existing.Client = report.Client;
            existing.ContractNo = report.ContractNo;
            existing.ArticleName = report.ArticleName;
            existing.Supplier = report.Supplier;
            existing.Factory = report.Factory;
            existing.InspectionPlace = report.InspectionPlace;
            existing.InspectionDate = report.InspectionDate;
            existing.QualityMark = report.QualityMark;
            existing.InspectionStandard = report.InspectionStandard;
            existing.InspectionSampling = report.InspectionSampling;
            existing.InspectionQuantity = report.InspectionQuantity;
            existing.SampleSize = report.SampleSize;
            existing.InspectionCartonNo = report.InspectionCartonNo;
            existing.InspectionResult = report.InspectionResult;
            existing.InspectorName = report.InspectorName;
            existing.FactoryRepresentative = report.FactoryRepresentative;

            existing.InspectionOrder.Clear();
            foreach (var item in report.InspectionOrder)
            {
                existing.InspectionOrder.Add(new InspectionOrderArticle
                {
                    LotNo = item.LotNo,
                    ArticleNumber = item.ArticleNumber,
                    OrderQuantity = item.OrderQuantity,
                    ShipmentQuantityPcs = item.ShipmentQuantityPcs,
                    ShipmentQuantityCartons = item.ShipmentQuantityCartons,
                    UnitsPacked = item.UnitsPacked,
                    UnitsFinishedNotPacked = item.UnitsFinishedNotPacked,
                    UnitsNotFinished = item.UnitsNotFinished,
                });
            }

            existing.PhotoDocumentation.Clear();
            foreach (var item in report.PhotoDocumentation)
            {
                existing.PhotoDocumentation.Add(new PhotoDocumentation
                {
                    InspectionReportId = existing.Id,
                    PhotoType = item.PhotoType,
                    Code = item.Code,
                    Description = item.Description,
                    PicturePath = item.PicturePath,
                    Count = item.Count,
                });
            }

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteInspectionReport(int id, int authSessionId)
        {
            var report = await _context.InspectionReports
                .FirstOrDefaultAsync(r => r.Id == id);// && r.AuthSessionId == authSessionId);
            if (report != null)
            {
                _context.InspectionReports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }
    }
}
