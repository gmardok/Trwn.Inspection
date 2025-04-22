using Trwn.Inspection.Mobile.Views;

namespace Trwn.Inspection.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("Report", typeof(ReportPage));
            Routing.RegisterRoute("Report/PhotoDetailsPage", typeof(PhotoDetailsPage));
        }
    }
}
