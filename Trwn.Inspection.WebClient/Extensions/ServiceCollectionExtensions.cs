using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Trwn.Inspection.WebClient.Extensions
{
    /// <summary>
    /// Extension methods for configuring the Inspection Reports Web API client
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Inspection Reports Web API client to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="baseUrl">The base URL of the Web API</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddInspectionReportsClient(this IServiceCollection services, string baseUrl)
        {
            return AddInspectionReportsClient(services, options => options.BaseUrl = baseUrl);
        }

        /// <summary>
        /// Adds the Inspection Reports Web API client to the service collection with configuration
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configureOptions">Action to configure client options</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddInspectionReportsClient(this IServiceCollection services, Action<InspectionReportsClientOptions> configureOptions)
        {
            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            // Configure options
            services.Configure(configureOptions);

            // Register HttpClient with configuration for basic client
            services.AddHttpClient<IInspectionReportsClient, InspectionReportsClient>((serviceProvider, httpClient) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<InspectionReportsClientOptions>>().Value;
                ConfigureHttpClient(httpClient, options);
            });

            // Register HttpClient with configuration for enhanced API client
            services.AddHttpClient<IInspectionReportsApiClient, InspectionReportsApiClient>((serviceProvider, httpClient) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<InspectionReportsClientOptions>>().Value;
                ConfigureHttpClient(httpClient, options);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                // Allowing Untrusted SSL Certificates
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) => true;

                return handler;
            });

            return services;
        }

        /// <summary>
        /// Adds the Inspection Reports Web API client to the service collection using an existing HttpClient name
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="httpClientName">The name of the existing HttpClient configuration</param>
        /// <param name="baseUrl">The base URL of the Web API</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddInspectionReportsClient(this IServiceCollection services, string httpClientName, string baseUrl)
        {
            services.AddHttpClient(httpClientName, client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

            // Register basic client
            services.AddTransient<IInspectionReportsClient>(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(httpClientName);
                var logger = serviceProvider.GetRequiredService<ILogger<InspectionReportsClient>>();
                return new InspectionReportsClient(httpClient, logger);
            });

            // Register enhanced API client
            services.AddTransient<IInspectionReportsApiClient>(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(httpClientName);
                var logger = serviceProvider.GetRequiredService<ILogger<InspectionReportsApiClient>>();
                return new InspectionReportsApiClient(httpClient, logger);
            });

            return services;
        }

        private static void ConfigureHttpClient(HttpClient httpClient, InspectionReportsClientOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.BaseUrl))
                throw new InvalidOperationException("BaseUrl must be configured for InspectionReportsClient");

            httpClient.BaseAddress = new Uri(options.BaseUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);

            // Add API key header if provided
            if (!string.IsNullOrWhiteSpace(options.ApiKey))
            {
                httpClient.DefaultRequestHeaders.Add("X-API-Key", options.ApiKey);
            }

            // Add additional headers
            foreach (var header in options.Headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }
}