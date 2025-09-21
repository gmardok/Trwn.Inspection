using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using Trwn.Inspection.Models;
using Trwn.Inspection.WebClient.Models;

namespace Trwn.Inspection.WebClient
{
    /// <summary>
    /// Enhanced HTTP client implementation with ApiResponse wrappers for better error handling
    /// </summary>
    public class InspectionReportsApiClient : IInspectionReportsApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<InspectionReportsApiClient> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public InspectionReportsApiClient(HttpClient httpClient, ILogger<InspectionReportsApiClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse<List<InspectionReport>>> GetInspectionReportsAsync()
        {
            try
            {
                _logger.LogInformation("Getting all inspection reports");
                
                var response = await _httpClient.GetAsync("api/InspectionReports");
                
                if (response.IsSuccessStatusCode)
                {
                    var reports = await response.Content.ReadFromJsonAsync<List<InspectionReport>>(_jsonOptions);
                    return ApiResponse<List<InspectionReport>>.Success(reports ?? new List<InspectionReport>(), (int)response.StatusCode);
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return ApiResponse<List<InspectionReport>>.Failure($"Failed to get inspection reports: {errorContent}", (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inspection reports");
                return ApiResponse<List<InspectionReport>>.Failure(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<InspectionReport>> GetInspectionReportAsync(string id)
        {
            try
            {
                _logger.LogInformation("Getting inspection report with ID: {Id}", id);
                
                var response = await _httpClient.GetAsync($"api/InspectionReports/{id}");
                
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return ApiResponse<InspectionReport>.Failure("Inspection report not found", 404);
                }
                
                if (response.IsSuccessStatusCode)
                {
                    var report = await response.Content.ReadFromJsonAsync<InspectionReport>(_jsonOptions);
                    return ApiResponse<InspectionReport>.Success(report!, (int)response.StatusCode);
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return ApiResponse<InspectionReport>.Failure($"Failed to get inspection report: {errorContent}", (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inspection report with ID: {Id}", id);
                return ApiResponse<InspectionReport>.Failure(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<InspectionReport>> AddInspectionReportAsync(InspectionReport report)
        {
            try
            {
                _logger.LogInformation("Adding new inspection report");
                
                var response = await _httpClient.PostAsJsonAsync("api/InspectionReports", report, _jsonOptions);
                
                if (response.IsSuccessStatusCode)
                {
                    var createdReport = await response.Content.ReadFromJsonAsync<InspectionReport>(_jsonOptions);
                    return ApiResponse<InspectionReport>.Success(createdReport!, (int)response.StatusCode);
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return ApiResponse<InspectionReport>.Failure($"Failed to add inspection report: {errorContent}", (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding inspection report");
                return ApiResponse<InspectionReport>.Failure(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<InspectionReport>> UpdateInspectionReportAsync(string id, InspectionReport report)
        {
            try
            {
                _logger.LogInformation("Updating inspection report with ID: {Id}", id);
                
                var response = await _httpClient.PutAsJsonAsync($"api/InspectionReports/{id}", report, _jsonOptions);
                
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return ApiResponse<InspectionReport>.Failure("Inspection report not found", 404);
                }
                
                if (response.IsSuccessStatusCode)
                {
                    var updatedReport = await response.Content.ReadFromJsonAsync<InspectionReport>(_jsonOptions);
                    return ApiResponse<InspectionReport>.Success(updatedReport!, (int)response.StatusCode);
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return ApiResponse<InspectionReport>.Failure($"Failed to update inspection report: {errorContent}", (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating inspection report with ID: {Id}", id);
                return ApiResponse<InspectionReport>.Failure(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PhotoDocumentation>> AddPhotoDocumentationAsync(string id, PhotoDocumentation photoDocumentation)
        {
            try
            {
                _logger.LogInformation("Adding photo documentation to report with ID: {Id}", id);
                
                var response = await _httpClient.PostAsJsonAsync($"api/InspectionReports/{id}/foto", photoDocumentation, _jsonOptions);
                
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return ApiResponse<PhotoDocumentation>.Failure("Inspection report not found", 404);
                }
                
                if (response.IsSuccessStatusCode)
                {
                    var createdPhoto = await response.Content.ReadFromJsonAsync<PhotoDocumentation>(_jsonOptions);
                    return ApiResponse<PhotoDocumentation>.Success(createdPhoto!, (int)response.StatusCode);
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return ApiResponse<PhotoDocumentation>.Failure($"Failed to add photo documentation: {errorContent}", (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding photo documentation to report with ID: {Id}", id);
                return ApiResponse<PhotoDocumentation>.Failure(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<PhotoDocumentation>> GetPhotoDocumentationAsync(string id, int fotoCode)
        {
            try
            {
                _logger.LogInformation("Getting photo documentation for report ID: {Id}, photo code: {FotoCode}", id, fotoCode);
                
                var response = await _httpClient.GetAsync($"api/InspectionReports/{id}/foto/{fotoCode}");
                
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return ApiResponse<PhotoDocumentation>.Failure("Photo documentation not found", 404);
                }
                
                if (response.IsSuccessStatusCode)
                {
                    var photo = await response.Content.ReadFromJsonAsync<PhotoDocumentation>(_jsonOptions);
                    return ApiResponse<PhotoDocumentation>.Success(photo!, (int)response.StatusCode);
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return ApiResponse<PhotoDocumentation>.Failure($"Failed to get photo documentation: {errorContent}", (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting photo documentation for report ID: {Id}, photo code: {FotoCode}", id, fotoCode);
                return ApiResponse<PhotoDocumentation>.Failure(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<PhotoDocumentation>>> GetAllPhotoDocumentationAsync(string id)
        {
            try
            {
                _logger.LogInformation("Getting all photo documentation for report with ID: {Id}", id);
                
                var response = await _httpClient.GetAsync($"api/InspectionReports/{id}/foto");
                
                if (response.IsSuccessStatusCode)
                {
                    var photos = await response.Content.ReadFromJsonAsync<List<PhotoDocumentation>>(_jsonOptions);
                    return ApiResponse<List<PhotoDocumentation>>.Success(photos ?? new List<PhotoDocumentation>(), (int)response.StatusCode);
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return ApiResponse<List<PhotoDocumentation>>.Failure($"Failed to get photo documentation: {errorContent}", (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all photo documentation for report with ID: {Id}", id);
                return ApiResponse<List<PhotoDocumentation>>.Failure(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteInspectionReportAsync(string id)
        {
            try
            {
                _logger.LogInformation("Deleting inspection report with ID: {Id}", id);
                
                var response = await _httpClient.DeleteAsync($"api/InspectionReports/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    return ApiResponse<bool>.Success(true, (int)response.StatusCode);
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return ApiResponse<bool>.Failure($"Failed to delete inspection report: {errorContent}", (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting inspection report with ID: {Id}", id);
                return ApiResponse<bool>.Failure(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> DeletePhotoDocumentationAsync(string id, int fotoCode)
        {
            try
            {
                _logger.LogInformation("Deleting photo documentation for report ID: {Id}, photo code: {FotoCode}", id, fotoCode);
                
                var response = await _httpClient.DeleteAsync($"api/InspectionReports/{id}/foto/{fotoCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    return ApiResponse<bool>.Success(true, (int)response.StatusCode);
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return ApiResponse<bool>.Failure($"Failed to delete photo documentation: {errorContent}", (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting photo documentation for report ID: {Id}, photo code: {FotoCode}", id, fotoCode);
                return ApiResponse<bool>.Failure(ex.Message, 500);
            }
        }
    }
}