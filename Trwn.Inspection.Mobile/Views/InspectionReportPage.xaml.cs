using Trwn.Inspection.Mobile.ViewModels;

namespace Trwn.Inspection.Mobile.Views;

public partial class InspectionReportPage : ContentPage
{
	public InspectionReportPage(InspectionReportViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}