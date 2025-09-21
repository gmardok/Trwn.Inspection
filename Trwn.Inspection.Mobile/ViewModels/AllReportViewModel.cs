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
        
        private string _selectedReport;
        public string SelectedReport
        {
            get => _selectedReport;
            set
            {
                if (_selectedReport != value)
                {
                    _selectedReport = value;
                    //OnPropertyChanged(nameof(SelectedReport));
                    OpenReportCommand.NotifyCanExecuteChanged();
                    UploadReportCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public AllReportViewModel(InspectionReportViewModel inspectionReportViewModel, PersistanceService persistanceService)
        {
            _persistanceService = persistanceService;
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

        [RelayCommand(CanExecute = nameof(CanExecuteReportCommands))]
        private async Task OpenReportAsync()
        {
            _inspectionReportViewModel.LoadReport(SelectedReport);

            await Shell.Current.GoToAsync("///InspectionReportPage");
        }
        
        [RelayCommand(CanExecute = nameof(CanExecuteReportCommands))]
        private async Task UploadReportAsync()
        {
            var report = _persistanceService.Load(SelectedReport);
            await _persistanceService.UploadReportAsync(report);
        }

        private bool CanExecuteReportCommands()
        {
            return !string.IsNullOrEmpty(SelectedReport);
        }
    }
}
