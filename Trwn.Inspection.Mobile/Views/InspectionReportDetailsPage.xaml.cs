using Trwn.Inspection.Mobile.ViewModels;

namespace Trwn.Inspection.Mobile.Views;

public partial class InspectionReportDetailsPage : ContentPage
{
	public InspectionReportDetailsPage(InspectionReportViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}