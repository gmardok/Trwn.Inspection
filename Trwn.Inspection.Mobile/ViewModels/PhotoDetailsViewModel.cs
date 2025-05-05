using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Media;
using Trwn.Inspection.Mobile.Services;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Mobile.ViewModels
{
    public partial class PhotoDetailsViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private PhotoType photoType;

        [ObservableProperty]
        private string photoDescription;

        [ObservableProperty]
        private string photoPath;

        public IAsyncRelayCommand TakePhotoCommand { get; }
        public IRelayCommand NavigateBackCommand { get; }
        public IRelayCommand SavePhotoDetailsCommand { get; }

        private readonly IPhotoPickerService _photoPickerService;

        public PhotoDetailsViewModel(IPhotoPickerService photoPickerService)
        {
            _photoPickerService = photoPickerService;
            TakePhotoCommand = new AsyncRelayCommand(TakePhotoAsync);
            NavigateBackCommand = new RelayCommand(NavigateBack);
            SavePhotoDetailsCommand = new RelayCommand(SavePhotoDetails);
        }

        private async Task TakePhotoAsync()
        {
            PhotoPath = await _photoPickerService.TakePhotoAsync();
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
            var pars = new ShellNavigationQueryParameters(new Dictionary<string, object>
                {
                    { "photoType", (int)PhotoType },
                    { "description", PhotoDescription},
                    { "path", PhotoPath }
                });
            Shell.Current.GoToAsync("..", true, pars);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            PhotoType = (PhotoType)Convert.ToInt32(query["photoType"]);
        }
    }
}
