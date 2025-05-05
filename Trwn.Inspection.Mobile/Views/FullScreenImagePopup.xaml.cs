using CommunityToolkit.Maui.Views;

namespace Trwn.Inspection.Mobile.Views
{
    public partial class FullScreenImagePopup : Popup
    {
        public FullScreenImagePopup(string imageSource)
        {
            InitializeComponent();
            BindingContext = new FullScreenImagePopupViewModel(imageSource, this);
        }
    }

    public class FullScreenImagePopupViewModel
    {
        public string ImageSource { get; }
        public Command CloseCommand { get; }

        public FullScreenImagePopupViewModel(string imageSource, Popup popup)
        {
            ImageSource = imageSource;
            CloseCommand = new Command(() => popup.Close());
        }
    }
}