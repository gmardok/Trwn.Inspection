using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
            InspectionOrderArticles = new ObservableCollection<InspectionOrderArticleViewModel>();
            AddInspectionOrderArticleCommand = new RelayCommand(AddInspectionOrderArticle);
            SaveCommand = new AsyncRelayCommand(() => new PersistanceService().Save(_report));
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand AddInspectionOrderArticleCommand { get; private set; }

        public ObservableCollection<InspectionOrderArticleViewModel> InspectionOrderArticles { get; }

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

        public string Inspector
        {
            get => _report.Inspector;
            set
            {
                if (_report.Inspector != value)
                {
                    _report.Inspector = value;
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

        private void AddInspectionOrderArticle()
        {
            var newArticle = new InspectionOrderArticleViewModel();
            newArticle.RemoveCommand = new RelayCommand(() => InspectionOrderArticles.Remove(newArticle));
            InspectionOrderArticles.Add(newArticle);
        }
    }
}
