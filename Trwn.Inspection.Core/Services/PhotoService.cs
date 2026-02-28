using Microsoft.Extensions.Options;
using Trwn.Inspection.Configuration;
using Trwn.Inspection.Infrastructure;

namespace Trwn.Inspection.Core
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoDocumentationRepository _repository;
        private readonly string _photoStoragePath;

        public PhotoService(
            IPhotoDocumentationRepository repository,
            IOptions<AppSettings> appSettings)
        {
            _repository = repository;
            _photoStoragePath = appSettings.Value.PhotoStoragePath ?? "Photos";
        }

        public async Task<bool> AddOrUpdatePhoto(int id, Stream fileStream, string fileExtension)
        {
            var photo = await _repository.GetPhotoDocumentation(id);
            if (photo == null)
                return false;

            var directory = Path.Combine(_photoStoragePath, photo.InspectionReportId.ToString());
            Directory.CreateDirectory(directory);

            var fileName = $"{photo.Id}{fileExtension}";
            var filePath = Path.Combine(directory, fileName);
            var relativePath = Path.Combine(photo.InspectionReportId.ToString(), fileName);

            await using (var targetStream = File.Create(filePath))
            {
                await fileStream.CopyToAsync(targetStream);
            }

            await _repository.UpdatePicturePath(id, relativePath);
            return true;
        }

        public async Task<(Stream? FileStream, string? ContentType)> GetPhoto(int id)
        {
            var photo = await _repository.GetPhotoDocumentation(id);
            if (photo == null || string.IsNullOrEmpty(photo.PicturePath))
                return (null, null);

            var filePath = Path.Combine(_photoStoragePath, photo.PicturePath);
            if (!File.Exists(filePath))
                return (null, null);

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            var contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".bmp" => "image/bmp",
                _ => "application/octet-stream"
            };

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return (stream, contentType);
        }
    }
}
