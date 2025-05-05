using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Mobile.ViewModels
{
    public partial class InspectionOrderArticleViewModel : ObservableObject
    {
        public InspectionOrderArticle InspectionOrderArticle;

        public InspectionOrderArticleViewModel()
        {
            InspectionOrderArticle = new InspectionOrderArticle();
            RemoveCommand = new RelayCommand(Remove);
        }

        public string LotNoDisplay => LotNo == 0 ? "New lot" : LotNo.ToString();

        public int LotNo
        {
            get => InspectionOrderArticle.LotNo;
            set
            {
                if (InspectionOrderArticle.LotNo != value)
                {
                    InspectionOrderArticle.LotNo = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LotNoDisplay));
                }
            }
        }

        public string ArticleNumber
        {
            get => InspectionOrderArticle.ArticleNumber;
            set
            {
                if (InspectionOrderArticle.ArticleNumber != value)
                {
                    InspectionOrderArticle.ArticleNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public int OrderQuantity
        {
            get => InspectionOrderArticle.OrderQuantity;
            set
            {
                if (InspectionOrderArticle.OrderQuantity != value)
                {
                    InspectionOrderArticle.OrderQuantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UnitsPackedPercentage));
                    OnPropertyChanged(nameof(UnitsFinishedNotPackedPercentage));
                    OnPropertyChanged(nameof(UnitsNotFinishedPercentage));
                }
            }
        }

        public int ShipmentQuantityPcs
        {
            get => InspectionOrderArticle.ShipmentQuantityPcs;
            set
            {
                if (InspectionOrderArticle.ShipmentQuantityPcs != value)
                {
                    InspectionOrderArticle.ShipmentQuantityPcs = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UnitsPackedPercentage));
                    OnPropertyChanged(nameof(UnitsFinishedNotPackedPercentage));
                    OnPropertyChanged(nameof(UnitsNotFinishedPercentage));
                }
            }
        }

        public int ShipmentQuantityCartons
        {
            get => InspectionOrderArticle.ShipmentQuantityCartons;
            set
            {
                if (InspectionOrderArticle.ShipmentQuantityCartons != value)
                {
                    InspectionOrderArticle.ShipmentQuantityCartons = value;
                    OnPropertyChanged();
                }
            }
        }

        public int UnitsPacked
        {
            get => InspectionOrderArticle.UnitsPacked;
            set
            {
                if (InspectionOrderArticle.UnitsPacked != value)
                {
                    InspectionOrderArticle.UnitsPacked = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UnitsPackedPercentage));
                }
            }
        }

        public int UnitsFinishedNotPacked
        {
            get => InspectionOrderArticle.UnitsFinishedNotPacked;
            set
            {
                if (InspectionOrderArticle.UnitsFinishedNotPacked != value)
                {
                    InspectionOrderArticle.UnitsFinishedNotPacked = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UnitsFinishedNotPackedPercentage));
                }
            }
        }

        public int UnitsNotFinished
        {
            get => InspectionOrderArticle.UnitsNotFinished;
            set
            {
                if (InspectionOrderArticle.UnitsNotFinished != value)
                {
                    InspectionOrderArticle.UnitsNotFinished = value;
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
