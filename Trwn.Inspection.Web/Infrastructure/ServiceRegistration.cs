using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trwn.Inspection.Configuration;
using Trwn.Inspection.Core;
using Trwn.Inspection.Core.Interfaces;
using Trwn.Inspection.Core.Services;
using Trwn.Inspection.Data;
using Trwn.Inspection.Infrastructure;
using Trwn.Inspection.Infrastructure.Auth;
using Trwn.Inspection.Infrastructure.Repositories;

namespace Trwn.Inspection.Web.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthSessionContext, HttpAuthSessionContext>();

            services.Configure<AuthSettings>(configuration.GetSection("Auth"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();
            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<AuthSettings>>((options, authOptions) =>
                    JwtBearerConfiguration.ConfigureJwtBearer(options, authOptions.Value));
            services.AddAuthorization();

            services.AddScoped<IEmailDomainPolicy, EmailDomainPolicy>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<ILoginCodeEmailSender, SmtpLoginCodeEmailSender>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IInspectionReportsService, InspectionReportsService>();
            services.AddScoped<IInspectionReportGenerator, InspectionReportGenerator>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IPhotoDocumentationRepository, PhotoDocumentationSqlRepository>();
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
