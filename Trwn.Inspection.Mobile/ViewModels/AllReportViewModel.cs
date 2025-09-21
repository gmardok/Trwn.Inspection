using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Trwn.Inspection.Mobile.Services;

namespace Trwn.Inspection.Mobile.ViewModels
{
    public partial class AllReportViewModel : INotifyPropertyChanged
    {
        private readonly PersistanceService _persistanceService;

        private readonly InspectionReportViewModel _inspectionReportViewModel;

        public ObservableCollection<string> LocalReports { get; set; }

        public AllReportViewModel(InspectionReportViewModel inspectionReportViewModel)
        {
            _persistanceService = new PersistanceService();
            LocalReports = new ObservableCollection<string>( _persistanceService.GetLocalReports());
            _inspectionReportViewModel = inspectionReportViewModel;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [RelayCommand]
        private async Task NewReportAsync()
        {
            _inspectionReportViewModel.NewReport();

            await Shell.Current.GoToAsync("///InspectionReportPage");
        }
    }
}
