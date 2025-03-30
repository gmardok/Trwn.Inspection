using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trwn.Inspection.Mobile.Services;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Mobile.ViewModels
{
    public class ReportViewModel : ObservableObject
    {
        private InspectionReport _report;

        public ReportViewModel()
        {
            _report = new InspectionReport();
            SaveCommand = new AsyncRelayCommand(() => new PersistanceService().Save(_report));
        }

        public ICommand SaveCommand { get; private set; }

        public InspectionType ReportType
        {
            get => _report.InspectionType;
            set
            {
                if (_report.InspectionType != value)
                {
                    _report.InspectionType = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
