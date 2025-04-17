using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Mobile.ViewModels
{
    public class InspectionOrderArticleViewModel : ObservableObject
    {
        private InspectionOrderArticle _inspectionOrderArticle;

        public InspectionOrderArticleViewModel()
        {
            _inspectionOrderArticle = new InspectionOrderArticle();
            RemoveCommand = new RelayCommand(Remove);
        }

        public int LotNo
        {
            get => _inspectionOrderArticle.LotNo;
            set
            {
                if (_inspectionOrderArticle.LotNo != value)
                {
                    _inspectionOrderArticle.LotNo = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ArticleNumber
        {
            get => _inspectionOrderArticle.ArticleNumber;
            set
            {
                if (_inspectionOrderArticle.ArticleNumber != value)
                {
                    _inspectionOrderArticle.ArticleNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public int OrderQuantity
        {
            get => _inspectionOrderArticle.OrderQuantity;
            set
            {
                if (_inspectionOrderArticle.OrderQuantity != value)
                {
                    _inspectionOrderArticle.OrderQuantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UnitsPackedPercentage));
                    OnPropertyChanged(nameof(UnitsFinishedNotPackedPercentage));
                    OnPropertyChanged(nameof(UnitsNotFinishedPercentage));
                }
            }
        }

        public int ShipmentQuantityPcs
        {
            get => _inspectionOrderArticle.ShipmentQuantityPcs;
            set
            {
                if (_inspectionOrderArticle.ShipmentQuantityPcs != value)
                {
                    _inspectionOrderArticle.ShipmentQuantityPcs = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UnitsPackedPercentage));
                    OnPropertyChanged(nameof(UnitsFinishedNotPackedPercentage));
                    OnPropertyChanged(nameof(UnitsNotFinishedPercentage));
                }
            }
        }

        public int ShipmentQuantityCartons
        {
            get => _inspectionOrderArticle.ShipmentQuantityCartons;
            set
            {
                if (_inspectionOrderArticle.ShipmentQuantityCartons != value)
                {
                    _inspectionOrderArticle.ShipmentQuantityCartons = value;
                    OnPropertyChanged();
                }
            }
        }

        public int UnitsPacked
        {
            get => _inspectionOrderArticle.UnitsPacked;
            set
            {
                if (_inspectionOrderArticle.UnitsPacked != value)
                {
                    _inspectionOrderArticle.UnitsPacked = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UnitsPackedPercentage));
                }
            }
        }

        public int UnitsFinishedNotPacked
        {
            get => _inspectionOrderArticle.UnitsFinishedNotPacked;
            set
            {
                if (_inspectionOrderArticle.UnitsFinishedNotPacked != value)
                {
                    _inspectionOrderArticle.UnitsFinishedNotPacked = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UnitsFinishedNotPackedPercentage));
                }
            }
        }

        public int UnitsNotFinished
        {
            get => _inspectionOrderArticle.UnitsNotFinished;
            set
            {
                if (_inspectionOrderArticle.UnitsNotFinished != value)
                {
                    _inspectionOrderArticle.UnitsNotFinished = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UnitsNotFinishedPercentage));
                }
            }
        }

        public double UnitsPackedPercentage => ShipmentQuantityPcs != 0 ? (double)UnitsPacked / ShipmentQuantityPcs * 100 : 0;
        public double UnitsFinishedNotPackedPercentage => ShipmentQuantityPcs != 0 ? (double)UnitsFinishedNotPacked / ShipmentQuantityPcs * 100 : 0;
        public double UnitsNotFinishedPercentage => ShipmentQuantityPcs != 0 ? (double)UnitsNotFinished / ShipmentQuantityPcs * 100 : 0;

        public ICommand RemoveCommand { get; set; }

        private void Remove()
        {
            // This method will be set by the parent view model
        }
    }
}
