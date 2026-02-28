using Microsoft.EntityFrameworkCore;
using Trwn.Inspection.Data;
using Trwn.Inspection.Infrastructure;
using Trwn.Inspection.Infrastructure.Repositories;

namespace Trwn.Inspection.Web.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IInspectionReportsService, InspectionReportsService>();
            //services.AddSingleton<IInspectionReportRepository, SimpleInspectionReportRepository>();
            //services.AddSingleton<IInspectionReportRepository, InspectionReportMongoRepository>();
            services.AddScoped<IInspectionReportRepository, InspectionReportSqlRepository>();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=TrwnInspection.db";
            services.AddDbContext<InspectionDbContext>(options =>
                options.UseSqlite(connectionString));
        }
    }
}
