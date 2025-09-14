using Trwn.Inspection.Mobile.ViewModels;

namespace Trwn.Inspection.Mobile.Views;

public partial class InspectionReportItemsPage : ContentPage
{
	public InspectionReportItemsPage(InspectionReportViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}