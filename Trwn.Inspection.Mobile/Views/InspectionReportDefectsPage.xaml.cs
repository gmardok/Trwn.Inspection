using Trwn.Inspection.Mobile.ViewModels;

namespace Trwn.Inspection.Mobile.Views;

public partial class InspectionReportDefectsPage : ContentPage
{
	public InspectionReportDefectsPage(InspectionReportViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}