using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trwn.Inspection.Mobile.Services;
using UIKit;

namespace Trwn.Inspection.Mobile.Platforms.iOS.Services
{
    public class PhotoPickerService : IPhotoPickerService
    {
        private TaskCompletionSource<string> _photoTaskCompletionSource;

        public Task<string> TakePhotoAsync()
        {
            _photoTaskCompletionSource = new TaskCompletionSource<string>();

            var window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window?.RootViewController;

            var actionSheet = UIAlertController.Create("Choose Option", null, UIAlertControllerStyle.ActionSheet);

            actionSheet.AddAction(UIAlertAction.Create("Take Photo", UIAlertActionStyle.Default, _ =>
            {
                ShowPickerAsync(UIImagePickerControllerSourceType.Camera);
            }));

            actionSheet.AddAction(UIAlertAction.Create("Choose from Library", UIAlertActionStyle.Default, _ =>
            {
                ShowPickerAsync(UIImagePickerControllerSourceType.PhotoLibrary);
            }));

            actionSheet.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, _ =>
            {
                _photoTaskCompletionSource.SetResult(null);
            }));

            viewController?.PresentViewController(actionSheet, true, null);

            return _photoTaskCompletionSource.Task;
        }

        private Task<string> ShowPickerAsync(UIImagePickerControllerSourceType sourceType)
        {
            var picker = new UIImagePickerController
            {
                SourceType = sourceType,
                MediaTypes = new[] { "public.image" }
            };

            picker.FinishedPickingMedia += OnFinishedPickingMedia;
            picker.Canceled += OnPickerCancelled;

            var window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window?.RootViewController;
            viewController?.PresentViewController(picker, true, null);

            return _photoTaskCompletionSource.Task;
        }

        private void OnFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            var picker = sender as UIImagePickerController;
            picker?.DismissViewController(true, null);

            var image = e.OriginalImage;
            if (image != null)
            {
                var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var filePath = Path.Combine(documentsDirectory, $"{Guid.NewGuid()}.jpg");

                using (var data = image.AsJPEG())
                {
                    File.WriteAllBytes(filePath, data.ToArray());
                    _photoTaskCompletionSource.SetResult(filePath);
                }
            }
            else
            {
                _photoTaskCompletionSource.SetResult(null);
            }
        }

        private void OnPickerCancelled(object sender, EventArgs e)
        {
            var picker = sender as UIImagePickerController;
            picker?.DismissViewController(true, null);

            _photoTaskCompletionSource.SetResult(null);
        }
    }
}
