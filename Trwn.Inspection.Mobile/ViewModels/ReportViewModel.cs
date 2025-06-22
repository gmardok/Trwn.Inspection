using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trwn.Inspection.Mobile.Services;
using Trwn.Inspection.Mobile.Views;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Mobile.ViewModels
{
    public partial class ReportViewModel : ObservableObject, IQueryAttributable
    {
        private InspectionReport _report;

        public ReportViewModel()
        {
            _report = new InspectionReport
            {
                InspectionResult = InspectionResultType.Passes
            };

            /*var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var files = Directory.GetFiles(documentsDirectory, "*.jpg");
            for (int i = 0; i < files.Length; i++)
            {
                _report.PhotoDocumentation.Add(new PhotoDocumentation
                {
                    PhotoType = PhotoType.Major,
                    Description = $"Photo {i + 1}",
                    PicturePath = files[i],
                    Code = i + 1
                });
            }*/

            AddInspectionOrderArticleCommand = new RelayCommand(AddInspectionOrderArticle);
            SaveCommand = new AsyncRelayCommand(() => new PersistanceService().Save(_report));

            EnlargePhotoCommand = new RelayCommand<string>(EnlargePhoto);
        }

        public ObservableCollection<PhotoDocumentation> MajorDefects => [.. _report.PhotoDocumentation.Where(p => p.PhotoType == PhotoType.Major)];
        public ObservableCollection<PhotoDocumentation> MinorDefects => [.. _report.PhotoDocumentation.Where(p => p.PhotoType == PhotoType.Minor)];
        public ObservableCollection<PhotoDocumentation> ShippingMarks => [.. _report.PhotoDocumentation.Where(p => p.PhotoType == PhotoType.ShippingMark)];
        public ObservableCollection<PhotoDocumentation> Packagings => [.. _report.PhotoDocumentation.Where(p => p.PhotoType == PhotoType.Packaging)];
        public ObservableCollection<PhotoDocumentation> PackagesWithDeffects => [.. _report.PhotoDocumentation.Where(p => p.PhotoType == PhotoType.PackageWithDeffects)];

        public ICommand EnlargePhotoCommand { get; }

        private void EnlargePhoto(string photoPath)
        {
            var currentPage = Application.Current?.Windows.FirstOrDefault();
            if (currentPage?.Page != null)
            {
                // todo: Use a custom popup for displaying full-screen images
                // currentPage.Page.ShowPopup(new FullScreenImagePopup(photoPath));
            }
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand AddInspectionOrderArticleCommand { get; private set; }

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

        public string Name
        {
            get => _report.Name;
            set
            {
                if (_report.Name != value)
                {
                    _report.Name = value;
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var photoType = (PhotoType)query["photoType"];
            var code = _report.PhotoDocumentation.Count > 0 ? _report.PhotoDocumentation.Max(p => p.Code) + 1 : 1;
            _report.PhotoDocumentation.Add(new PhotoDocumentation
            {
                PhotoType = photoType,
                Description = (string)query["description"],
                PicturePath = (string)query["path"],
                Code = code
            });
            switch (photoType)
            {
                case PhotoType.Major:
                    OnPropertyChanged(nameof(MajorDefects));
                    break;
                case PhotoType.Minor:
                    OnPropertyChanged(nameof(MinorDefects));
                    break;
                case PhotoType.ShippingMark:
                    OnPropertyChanged(nameof(ShippingMarks));
                    break;
                case PhotoType.Packaging:
                    OnPropertyChanged(nameof(Packagings));
                    break;
                case PhotoType.PackageWithDeffects:
                    OnPropertyChanged(nameof(PackagesWithDeffects));
                    break;
            }
        }

        private void AddInspectionOrderArticle()
        {
            _report.InspectionOrder.Add(new InspectionOrderArticle());
            OnPropertyChanged(nameof(InspectionOrderArticles));
        }

        [RelayCommand]
        private async Task NavigateToPhotoDetailsAsync(PhotoType photoType)
        {
            try
            {
                await Shell.Current.GoToAsync($"PhotoDetailsPage?photoType={(int)photoType}", true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex.Message}");
            }            
        }
    }
}
