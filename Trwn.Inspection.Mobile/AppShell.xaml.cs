using Trwn.Inspection.Mobile.Views;

namespace Trwn.Inspection.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PhotoDetailsPage), typeof(PhotoDetailsPage));
            Routing.RegisterRoute(nameof(InspectionReportDetailsPage), typeof(InspectionReportDetailsPage));
            Routing.RegisterRoute(nameof(InspectionReportGeneralPage), typeof(InspectionReportGeneralPage));
            Routing.RegisterRoute(nameof(InspectionReportItemsPage), typeof(InspectionReportItemsPage));
            Routing.RegisterRoute(nameof(InspectionReportDefectsPage), typeof(InspectionReportDefectsPage));
        }
    }
}
