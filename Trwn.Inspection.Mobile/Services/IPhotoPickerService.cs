namespace Trwn.Inspection.Mobile.Services
{
    public interface IPhotoPickerService
    {
        Task<string> TakePhotoAsync();
    }
}
