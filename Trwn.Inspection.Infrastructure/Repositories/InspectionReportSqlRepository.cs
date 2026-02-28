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

        public async Task<List<InspectionReport>> GetInspectionReports()
        {
            return await _context.InspectionReports
                .Include(r => r.InspectionOrder)
                .Include(r => r.PhotoDocumentation)
                .OrderBy(r => r.Id)
                .ToListAsync();
        }

        public async Task<InspectionReport?> GetInspectionReport(int id)
        {
            return await _context.InspectionReports
                .Include(r => r.InspectionOrder)
                .Include(r => r.PhotoDocumentation)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<InspectionReport> AddInspectionReport(InspectionReport report)
        {
            report.Id = 0;
            _context.InspectionReports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<InspectionReport?> UpdateInspectionReport(int id, InspectionReport report)
        {
            var existing = await _context.InspectionReports
                .Include(r => r.InspectionOrder)
                .Include(r => r.PhotoDocumentation)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (existing == null)
                return null;

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
                    UnitsNotFinished = item.UnitsNotFinished
                });
            }

            existing.PhotoDocumentation.Clear();
            foreach (var item in report.PhotoDocumentation)
            {
                existing.PhotoDocumentation.Add(new PhotoDocumentation
                {
                    PhotoType = item.PhotoType,
                    Code = item.Code,
                    Description = item.Description,
                    PicturePath = item.PicturePath,
                    Count = item.Count
                });
            }

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteInspectionReport(int id)
        {
            var report = await _context.InspectionReports.FindAsync(id);
            if (report != null)
            {
                _context.InspectionReports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PhotoDocumentation?> AddInspectionFoto(int id, PhotoDocumentation photoDocumentation)
        {
            var report = await _context.InspectionReports
                .Include(r => r.PhotoDocumentation)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (report == null)
                return null;

            photoDocumentation.Id = 0;
            report.PhotoDocumentation.Add(photoDocumentation);
            await _context.SaveChangesAsync();
            return photoDocumentation;
        }

        public async Task<PhotoDocumentation?> GetInspectionFoto(int id, int fotoCode)
        {
            return await _context.PhotoDocumentations
                .Where(p => p.Code == fotoCode && EF.Property<int>(p, "InspectionReportId") == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PhotoDocumentation>> GetAllInspectionFoto(int id)
        {
            return await _context.PhotoDocumentations
                .Where(p => EF.Property<int>(p, "InspectionReportId") == id)
                .ToListAsync();
        }

        public async Task DeleteInspectionFoto(int id, int fotoCode)
        {
            var photo = await _context.PhotoDocumentations
                .FirstOrDefaultAsync(p => p.Code == fotoCode && EF.Property<int>(p, "InspectionReportId") == id);

            if (photo != null)
            {
                _context.PhotoDocumentations.Remove(photo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
