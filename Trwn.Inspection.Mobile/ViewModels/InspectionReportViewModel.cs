using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Trwn.Inspection.Mobile.Services;
using Trwn.Inspection.Mobile.Views;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Mobile.ViewModels
{
    public partial class InspectionReportViewModel : INotifyPropertyChanged
    {
        private readonly IPhotoPickerService _photoPickerService;

        private PersistanceService _persistanceService;

        #region Fields
        private InspectionReport _report;

        private PhotoType _currentPhotoType;

        public ObservableCollection<InspectionOrderArticleViewModel> InspectionOrderArticles => [..
            _report.InspectionOrder.Select(o => new InspectionOrderArticleViewModel
            {
                InspectionOrderArticle = o,
                RemoveCommand = new RelayCommand(() =>
                {
                    _report.InspectionOrder.Remove(o);
                    OnPropertyChanged(nameof(InspectionOrderArticles));
                })
            })];

        public ICommand AddInspectionOrderArticleCommand { get; } 

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

        public string ReportNo
        {
            get => _report.ReportNo;
            set
            {
                if (_report.ReportNo != value)
                {
                    _report.ReportNo = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Client
        {
            get => _report.Client;
            set
            {
                if (_report.Client != value)
                {
                    _report.Client = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ContractNo
        {
            get => _report.ContractNo;
            set
            {
                if (_report.ContractNo != value)
                {
                    _report.ContractNo = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ArticleName
        {
            get => _report.ArticleName;
            set
            {
                if (_report.ArticleName != value)
                {
                    _report.ArticleName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Supplier
        {
            get => _report.Supplier;
            set
            {
                if (_report.Supplier != value)
                {
                    _report.Supplier = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Factory
        {
            get => _report.Factory;
            set
            {
                if (_report.Factory != value)
                {
                    _report.Factory = value;
                    OnPropertyChanged();
                }
            }
        }

        public string InspectionPlace
        {
            get => _report.InspectionPlace;
            set
            {
                if (_report.InspectionPlace != value)
                {
                    _report.InspectionPlace = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime InspectionDate
        {
            get => _report.InspectionDate;
            set
            {
                if (_report.InspectionDate != value)
                {
                    _report.InspectionDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string QualityMark
        {
            get => _report.QualityMark;
            set
            {
                if (_report.QualityMark != value)
                {
                    _report.QualityMark = value;
                    OnPropertyChanged();
                }
            }
        }

        public string InspectionStandard
        {
            get => _report.InspectionStandard;
            set
            {
                if (_report.InspectionStandard != value)
                {
                    _report.InspectionStandard = value;
                    OnPropertyChanged();
                }
            }
        }

        public string InspectionSampling
        {
            get => _report.InspectionSampling;
            set
            {
                if (_report.InspectionSampling != value)
                {
                    _report.InspectionSampling = value;
                    OnPropertyChanged();
                }
            }
        }

        public int InspectionQuantity
        {
            get => _report.InspectionQuantity;
            set
            {
                if (_report.InspectionQuantity != value)
                {
                    _report.InspectionQuantity = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SampleSize
        {
            get => _report.SampleSize;
            set
            {
                if (_report.SampleSize != value)
                {
                    _report.SampleSize = value;
                    OnPropertyChanged();
                }
            }
        }

        public string InspectionCartonNo
        {
            get => _report.InspectionCartonNo;
            set
            {
                if (_report.InspectionCartonNo != value)
                {
                    _report.InspectionCartonNo = value;
                    OnPropertyChanged();
                }
            }
        }

        public string InspectorName
        {
            get => _report.InspectorName;
            set
            {
                if (_report.InspectorName != value)
                {
                    _report.InspectorName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FactoryRepresentative
        {
            get => _report.FactoryRepresentative;
            set
            {
                if (_report.FactoryRepresentative != value)
                {
                    _report.FactoryRepresentative = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<InspectionResultType> InspectionResultTypes { get; } =
            new ObservableCollection<InspectionResultType>((InspectionResultType[])Enum.GetValues(typeof(InspectionResultType)));

        public InspectionResultType InspectionResult
        {
            get => _report.InspectionResult;
            set
            {
                if (_report.InspectionResult != value)
                {
                    _report.InspectionResult = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<PhotoDocumentation> Defects => [.. _report.PhotoDocumentation.Where(p => p.PhotoType == _currentPhotoType)];

        public ICommand EnlargePhotoCommand { get; }

        public PhotoDocumentation CurrentDocumentation { get; set; }
        #endregion

        public InspectionReportViewModel(IPhotoPickerService photoPickerService)
        {
            _report = new InspectionReport
            {
                InspectionResult = InspectionResultType.Passes
            };

            _persistanceService = new PersistanceService();

            AddInspectionOrderArticleCommand = new RelayCommand(AddInspectionOrderArticle);
            EnlargePhotoCommand = new RelayCommand<string>(EnlargePhoto);
            _photoPickerService = photoPickerService;
            Shell.Current.Navigating += (s, e) =>
            {
                if (e.Current.Location.OriginalString.EndsWith(nameof(PhotoDetailsPage))
                    && (!string.IsNullOrEmpty(CurrentDocumentation?.PicturePath) || !string.IsNullOrEmpty(CurrentDocumentation?.Description)))
                {
                    _report.PhotoDocumentation.Add(CurrentDocumentation);
                    OnPropertyChanged(nameof(Defects));
                }
            };

            /*var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var files = Directory.GetFiles(documentsDirectory, "*.jpg");
            for (int i = 0; i < files.Length; i++)
            {
                _report.PhotoDocumentation.Add(new PhotoDocumentation
                {
                    PhotoType = PhotoType.Minor,
                    Description = $"Photo {i + 1}",
                    PicturePath = files[i],
                    Code = i + 1
                });
            }*/
        }

        public void LoadReport(string fileName)
        {
            _report = _persistanceService.Load(fileName);
            OnPropertyChanged(string.Empty);
        }

        public void NewReport()
        {
            _report = new InspectionReport
            {
                InspectionResult = InspectionResultType.Passes
            };

            _persistanceService = new PersistanceService();

            OnPropertyChanged(string.Empty);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            _persistanceService.Save(_report);
        }

        #region details methods
        private void AddInspectionOrderArticle()
        {
            _report.InspectionOrder.Add(new InspectionOrderArticle());
            OnPropertyChanged(nameof(InspectionOrderArticles));
        }
        #endregion

        #region navigation methods
        [RelayCommand]
        private void GoBack()
        {
            Shell.Current.SendBackButtonPressed();
        }

        [RelayCommand]
        private async Task NavigateToDetailsAsync()
        {
            await Shell.Current.GoToAsync(nameof(InspectionReportDetailsPage),
                new Dictionary<string, object> { { "Vm", this } });
        }

        [RelayCommand]
        private async Task NavigateToItemsAsync()
        {
            await Shell.Current.GoToAsync(nameof(InspectionReportItemsPage),
                new Dictionary<string, object> { { "Vm", this } });
        }

        [RelayCommand]
        private async Task NavigateToGeneralAsync()
        {
            await Shell.Current.GoToAsync(nameof(InspectionReportGeneralPage),
                new Dictionary<string, object> { { "Vm", this } });
        }

        [RelayCommand]
        private async Task NavigateToDefectsAsync(PhotoType photoType)
        {
            _currentPhotoType = photoType;
            OnPropertyChanged(nameof(Defects));
            await Shell.Current.GoToAsync(nameof(InspectionReportDefectsPage),
            new Dictionary<string, object> { { "Vm", this } });
        }

        [RelayCommand]
        private async Task NavigateToPhotoDetailsAsync()
        {
            CurrentDocumentation = new PhotoDocumentation
            {
                Code = Defects.Count(d => d.PhotoType == _currentPhotoType) + 1,
                PhotoType = _currentPhotoType
            };

            await Shell.Current.GoToAsync(nameof(PhotoDetailsPage),
                new Dictionary<string, object> { { "Vm", this } });
        }

        [RelayCommand]
        private async Task TakePhotoAsync()
        {
            CurrentDocumentation.PicturePath = await _photoPickerService.TakePhotoAsync();
            OnPropertyChanged(nameof(CurrentDocumentation));
        }

        private void EnlargePhoto(string photoPath)
        {
            var currentPage = Application.Current?.Windows.FirstOrDefault();
            if (currentPage?.Page != null)
            {
                currentPage.Page.ShowPopup(new FullScreenImagePopup(photoPath));
            }
        }
        #endregion
    }
}
