using Android.App;
using Android.Content;
using Android.Provider;
using Microsoft.Maui.ApplicationModel;
using Trwn.Inspection.Mobile.Services;
using System.IO;
using System.Threading.Tasks;
using Env = Android.OS.Environment;

namespace Trwn.Inspection.Mobile.Platforms.Android.Services
{
    public class PhotoPickerService : IPhotoPickerService
    {
        private TaskCompletionSource<string?>? _photoTaskCompletionSource;
        private string? _currentPhotoPath;

        public Task<string?> TakePhotoAsync()
        {
            _photoTaskCompletionSource = new TaskCompletionSource<string?>();

            var activity = Platform.CurrentActivity ?? throw new InvalidOperationException("No current activity");

            var actionSheet = new AlertDialog.Builder(activity)
                .SetTitle("Choose Option")
                .SetItems(new[] { "Take Photo", "Choose from Gallery" }, (sender, args) =>
                {
                    if (args.Which == 0)
                    {
                        LaunchCamera();
                    }
                    else if (args.Which == 1)
                    {
                        LaunchGallery();
                    }
                })
                .SetNegativeButton("Cancel", (sender, args) =>
                {
                    _photoTaskCompletionSource?.SetResult(null);
                })
                .Create();

            actionSheet.Show();

            return _photoTaskCompletionSource.Task;
        }

        private void LaunchCamera()
        {
            var activity = Platform.CurrentActivity ?? throw new InvalidOperationException("No current activity");

            var intent = new Intent(MediaStore.ActionImageCapture);
            var photoFile = CreateImageFile();
            _currentPhotoPath = photoFile?.AbsolutePath;

            if (photoFile != null)
            {
                var photoUri = AndroidX.Core.Content.FileProvider.GetUriForFile(activity, $"{activity.PackageName}.fileprovider", photoFile);
                intent.PutExtra(MediaStore.ExtraOutput, photoUri);
            }

            activity.StartActivityForResult(intent, 1001);
        }

        private void LaunchGallery()
        {
            var activity = Platform.CurrentActivity ?? throw new InvalidOperationException("No current activity");

            var intent = new Intent(Intent.ActionPick);
            intent.SetType("image/*");

            activity.StartActivityForResult(intent, 1002);
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            if (requestCode == 1001 && resultCode == Result.Ok)
            {
                // Camera result
                _photoTaskCompletionSource?.SetResult(_currentPhotoPath);
            }
            else if (requestCode == 1002 && resultCode == Result.Ok && data?.Data != null)
            {
                // Gallery result
                var activity = Platform.CurrentActivity ?? throw new InvalidOperationException("No current activity");

                try
                {
                    var inputStream = activity.ContentResolver?.OpenInputStream(data.Data);
                    if (inputStream != null)
                    {
                        var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                        var filePath = Path.Combine(documentsPath, $"{Guid.NewGuid()}.jpg");

                        using (var outputStream = File.Create(filePath))
                        {
                            inputStream.CopyTo(outputStream);
                        }

                        _photoTaskCompletionSource?.SetResult(filePath);
                    }
                    else
                    {
                        _photoTaskCompletionSource?.SetResult(null);
                    }
                }
                catch (Exception)
                {
                    _photoTaskCompletionSource?.SetResult(null);
                }
            }
            else
            {
                _photoTaskCompletionSource?.SetResult(null);
            }
        }

        private Java.IO.File? CreateImageFile()
        {
            try
            {
                var activity = Platform.CurrentActivity;
                var storageDir = activity?.GetExternalFilesDir(Env.DirectoryPictures);
                return storageDir != null ? Java.IO.File.CreateTempFile($"photo_{Guid.NewGuid()}", ".jpg", storageDir) : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
