using Trwn.Inspection.Mobile.ViewModels;

namespace Trwn.Inspection.Mobile.Views;

public partial class InspectionReportGeneralPage : ContentPage
{
	public InspectionReportGeneralPage(InspectionReportViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}