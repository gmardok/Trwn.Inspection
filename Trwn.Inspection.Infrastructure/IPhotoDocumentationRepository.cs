using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure
{
    public interface IPhotoDocumentationRepository
    {
        Task<PhotoDocumentation?> GetPhotoDocumentation(int id);
        Task UpdatePicturePath(int id, string picturePath);
    }
}
