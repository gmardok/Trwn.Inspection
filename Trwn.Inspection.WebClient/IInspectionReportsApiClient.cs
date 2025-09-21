using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trwn.Inspection.Models;
using Trwn.Inspection.WebClient.Models;

namespace Trwn.Inspection.WebClient
{
    /// <summary>
    /// Enhanced client interface for interacting with the Inspection Reports Web API with ApiResponse wrappers
    /// </summary>
    public interface IInspectionReportsApiClient
    {
        /// <summary>
        /// Gets all inspection reports
        /// </summary>
        /// <returns>API response containing list of inspection reports</returns>
        Task<ApiResponse<List<InspectionReport>>> GetInspectionReportsAsync();

        /// <summary>
        /// Gets a specific inspection report by ID
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <returns>API response containing the inspection report</returns>
        Task<ApiResponse<InspectionReport>> GetInspectionReportAsync(string id);

        /// <summary>
        /// Adds a new inspection report
        /// </summary>
        /// <param name="report">The inspection report to add</param>
        /// <returns>API response containing the created inspection report</returns>
        Task<ApiResponse<InspectionReport>> AddInspectionReportAsync(InspectionReport report);

        /// <summary>
        /// Updates an existing inspection report
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <param name="report">The updated inspection report</param>
        /// <returns>API response containing the updated inspection report</returns>
        Task<ApiResponse<InspectionReport>> UpdateInspectionReportAsync(string id, InspectionReport report);

        /// <summary>
        /// Deletes an inspection report
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <returns>API response indicating success or failure</returns>
        Task<ApiResponse<bool>> DeleteInspectionReportAsync(string id);

        /// <summary>
        /// Adds photo documentation to an inspection report
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <param name="photoDocumentation">The photo documentation to add</param>
        /// <returns>API response containing the created photo documentation</returns>
        Task<ApiResponse<PhotoDocumentation>> AddPhotoDocumentationAsync(string id, PhotoDocumentation photoDocumentation);

        /// <summary>
        /// Gets a specific photo documentation by report ID and photo code
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <param name="fotoCode">The photo code</param>
        /// <returns>API response containing the photo documentation</returns>
        Task<ApiResponse<PhotoDocumentation>> GetPhotoDocumentationAsync(string id, int fotoCode);

        /// <summary>
        /// Gets all photo documentation for a specific inspection report
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <returns>API response containing list of photo documentation</returns>
        Task<ApiResponse<List<PhotoDocumentation>>> GetAllPhotoDocumentationAsync(string id);

        /// <summary>
        /// Deletes photo documentation
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <param name="fotoCode">The photo code</param>
        /// <returns>API response indicating success or failure</returns>
        Task<ApiResponse<bool>> DeletePhotoDocumentationAsync(string id, int fotoCode);
    }
}