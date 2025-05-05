using Trwn.Inspection.Mobile.ViewModels;

namespace Trwn.Inspection.Mobile.Views;

public partial class PhotoDetailsPage : ContentPage
{
	public PhotoDetailsPage(PhotoDetailsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}