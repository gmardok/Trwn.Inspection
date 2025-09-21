using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.WebClient
{
    /// <summary>
    /// HTTP client implementation for interacting with the Inspection Reports Web API
    /// </summary>
    public class InspectionReportsClient : IInspectionReportsClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<InspectionReportsClient> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public InspectionReportsClient(HttpClient httpClient, ILogger<InspectionReportsClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<InspectionReport>> GetInspectionReportsAsync()
        {
            try
            {
                _logger.LogInformation("Getting all inspection reports");
                
                var response = await _httpClient.GetAsync("api/InspectionReports");
                response.EnsureSuccessStatusCode();
                
                var reports = await response.Content.ReadFromJsonAsync<List<InspectionReport>>(_jsonOptions);
                return reports ?? new List<InspectionReport>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inspection reports");
                throw;
            }
        }

        public async Task<InspectionReport?> GetInspectionReportAsync(string id)
        {
            try
            {
                _logger.LogInformation("Getting inspection report with ID: {Id}", id);
                
                var response = await _httpClient.GetAsync($"api/InspectionReports/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<InspectionReport>(_jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inspection report with ID: {Id}", id);
                throw;
            }
        }

        public async Task<InspectionReport> AddInspectionReportAsync(InspectionReport report)
        {
            try
            {
                _logger.LogInformation("Adding new inspection report");
                
                var response = await _httpClient.PostAsJsonAsync("api/InspectionReports", report, _jsonOptions);
                response.EnsureSuccessStatusCode();
                
                var createdReport = await response.Content.ReadFromJsonAsync<InspectionReport>(_jsonOptions);
                return createdReport ?? throw new InvalidOperationException("Failed to deserialize created report");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding inspection report");
                throw;
            }
        }

        public async Task<InspectionReport?> UpdateInspectionReportAsync(string id, InspectionReport report)
        {
            try
            {
                _logger.LogInformation("Updating inspection report with ID: {Id}", id);
                
                var response = await _httpClient.PutAsJsonAsync($"api/InspectionReports/{id}", report, _jsonOptions);
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<InspectionReport>(_jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating inspection report with ID: {Id}", id);
                throw;
            }
        }

        public async Task<PhotoDocumentation?> AddPhotoDocumentationAsync(string id, PhotoDocumentation photoDocumentation)
        {
            try
            {
                _logger.LogInformation("Adding photo documentation to report with ID: {Id}", id);
                
                var response = await _httpClient.PostAsJsonAsync($"api/InspectionReports/{id}/foto", photoDocumentation, _jsonOptions);
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<PhotoDocumentation>(_jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding photo documentation to report with ID: {Id}", id);
                throw;
            }
        }

        public async Task<PhotoDocumentation?> GetPhotoDocumentationAsync(string id, int fotoCode)
        {
            try
            {
                _logger.LogInformation("Getting photo documentation for report ID: {Id}, photo code: {FotoCode}", id, fotoCode);
                
                var response = await _httpClient.GetAsync($"api/InspectionReports/{id}/foto/{fotoCode}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<PhotoDocumentation>(_jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting photo documentation for report ID: {Id}, photo code: {FotoCode}", id, fotoCode);
                throw;
            }
        }

        public async Task<List<PhotoDocumentation>> GetAllPhotoDocumentationAsync(string id)
        {
            try
            {
                _logger.LogInformation("Getting all photo documentation for report with ID: {Id}", id);
                
                var response = await _httpClient.GetAsync($"api/InspectionReports/{id}/foto");
                response.EnsureSuccessStatusCode();
                
                var photos = await response.Content.ReadFromJsonAsync<List<PhotoDocumentation>>(_jsonOptions);
                return photos ?? new List<PhotoDocumentation>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all photo documentation for report with ID: {Id}", id);
                throw;
            }
        }

        public async Task DeleteInspectionReportAsync(string id)
        {
            try
            {
                _logger.LogInformation("Deleting inspection report with ID: {Id}", id);
                
                var response = await _httpClient.DeleteAsync($"api/InspectionReports/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting inspection report with ID: {Id}", id);
                throw;
            }
        }

        public async Task DeletePhotoDocumentationAsync(string id, int fotoCode)
        {
            try
            {
                _logger.LogInformation("Deleting photo documentation for report ID: {Id}, photo code: {FotoCode}", id, fotoCode);
                
                var response = await _httpClient.DeleteAsync($"api/InspectionReports/{id}/foto/{fotoCode}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting photo documentation for report ID: {Id}, photo code: {FotoCode}", id, fotoCode);
                throw;
            }
        }
    }
}