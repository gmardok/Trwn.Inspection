using Trwn.Inspection.Infrastructure;

namespace Trwn.Inspection.Web.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IInspectionReportsService,InspectionReportsService>();
            services.AddSingleton<IInspectionReportRepository, SimpleInspectionReportRepository>();
        }
    }
}
