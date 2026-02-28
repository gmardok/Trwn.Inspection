namespace Trwn.Inspection.Core
{
    public interface IPhotoService
    {
        Task<bool> AddOrUpdatePhoto(int id, Stream fileStream, string fileExtension);
        Task<(Stream? FileStream, string? ContentType)> GetPhoto(int id);
    }
}
