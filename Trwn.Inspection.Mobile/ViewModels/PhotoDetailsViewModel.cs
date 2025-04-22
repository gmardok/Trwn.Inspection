using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Media;

namespace Trwn.Inspection.Mobile.ViewModels
{
    public partial class PhotoDetailsViewModel : ObservableObject
    {
        public ObservableCollection<string> PhotoTypes { get; } = new ObservableCollection<string>
        {
            "Type 1",
            "Type 2",
            "Type 3",
            "Type 4",
            "Type 5"
        };

        [ObservableProperty]
        private string selectedPhotoType;

        [ObservableProperty]
        private string photoDescription;

        public IAsyncRelayCommand TakePhotoCommand { get; }
        public IAsyncRelayCommand SelectPhotoCommand { get; }
        public IRelayCommand NavigateBackCommand { get; }
        public IRelayCommand SavePhotoDetailsCommand { get; }

        public PhotoDetailsViewModel()
        {
            TakePhotoCommand = new AsyncRelayCommand(TakePhotoAsync);
            SelectPhotoCommand = new AsyncRelayCommand(SelectPhotoAsync);
            NavigateBackCommand = new RelayCommand(NavigateBack);
            SavePhotoDetailsCommand = new RelayCommand(SavePhotoDetails);
        }

        [ObservableProperty]
        private string photoPath;

        private async Task TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                    using var stream = await photo.OpenReadAsync();
                    using var newStream = File.OpenWrite(newFile);
                    await stream.CopyToAsync(newStream);

                    PhotoPath = newFile; // Update the photo path
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., user cancels the operation)
            }
        }

        private async Task SelectPhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                {
                    var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                    using var stream = await photo.OpenReadAsync();
                    using var newStream = File.OpenWrite(newFile);
                    await stream.CopyToAsync(newStream);

                    PhotoPath = newFile; // Update the photo path
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., user cancels the operation)
            }
        }

        private async void NavigateBack()
        {
            //var stack = Shell.Current.Navigation.NavigationStack;
            try
            {
                await Shell.Current.GoToAsync("..", true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void SavePhotoDetails()
        {
            // Logic to save photo details (e.g., send to backend or update model)
            Shell.Current.GoToAsync("//ean /ReportPage");
        }
    }
}
