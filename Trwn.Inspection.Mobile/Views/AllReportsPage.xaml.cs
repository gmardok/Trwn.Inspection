using Trwn.Inspection.Mobile.ViewModels;

namespace Trwn.Inspection.Mobile.Views;

public partial class AllReportsPage : ContentPage
{
	public AllReportsPage(AllReportViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}