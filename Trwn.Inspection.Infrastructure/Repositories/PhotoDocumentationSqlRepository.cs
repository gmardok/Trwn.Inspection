using Microsoft.EntityFrameworkCore;
using Trwn.Inspection.Data;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure.Repositories
{
    public class PhotoDocumentationSqlRepository : IPhotoDocumentationRepository
    {
        private readonly InspectionDbContext _context;

        public PhotoDocumentationSqlRepository(InspectionDbContext context)
        {
            _context = context;
        }

        public async Task<PhotoDocumentation?> GetPhotoDocumentation(int id)
        {
            return await _context.PhotoDocumentations
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PhotoDocumentation?> GetPhotoDocumentationForSession(int id, int authSessionId)
        {
            return await _context.PhotoDocumentations
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    _context.InspectionReports.Any(r =>
                        r.Id == p.InspectionReportId && r.AuthSessionId == authSessionId));
        }

        public async Task UpdatePicturePath(int id, string picturePath)
        {
            var photo = await _context.PhotoDocumentations.FindAsync(id);
            if (photo != null)
            {
                photo.PicturePath = picturePath;
                await _context.SaveChangesAsync();
            }
        }
    }
}
