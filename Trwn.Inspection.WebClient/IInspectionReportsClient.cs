using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.WebClient
{
    /// <summary>
    /// Client interface for interacting with the Inspection Reports Web API
    /// </summary>
    public interface IInspectionReportsClient
    {
        /// <summary>
        /// Gets all inspection reports
        /// </summary>
        /// <returns>List of inspection reports</returns>
        Task<List<InspectionReport>> GetInspectionReportsAsync();

        /// <summary>
        /// Gets a specific inspection report by ID
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <returns>The inspection report or null if not found</returns>
        Task<InspectionReport?> GetInspectionReportAsync(string id);

        /// <summary>
        /// Adds a new inspection report
        /// </summary>
        /// <param name="report">The inspection report to add</param>
        /// <returns>The created inspection report</returns>
        Task<InspectionReport> AddInspectionReportAsync(InspectionReport report);

        /// <summary>
        /// Updates an existing inspection report
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <param name="report">The updated inspection report</param>
        /// <returns>The updated inspection report or null if not found</returns>
        Task<InspectionReport?> UpdateInspectionReportAsync(string id, InspectionReport report);

        /// <summary>
        /// Deletes an inspection report
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <returns>Task representing the delete operation</returns>
        Task DeleteInspectionReportAsync(string id);

        /// <summary>
        /// Adds photo documentation to an inspection report
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <param name="photoDocumentation">The photo documentation to add</param>
        /// <returns>The created photo documentation or null if report not found</returns>
        Task<PhotoDocumentation?> AddPhotoDocumentationAsync(string id, PhotoDocumentation photoDocumentation);

        /// <summary>
        /// Gets a specific photo documentation by report ID and photo code
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <param name="fotoCode">The photo code</param>
        /// <returns>The photo documentation or null if not found</returns>
        Task<PhotoDocumentation?> GetPhotoDocumentationAsync(string id, int fotoCode);

        /// <summary>
        /// Gets all photo documentation for a specific inspection report
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <returns>List of photo documentation</returns>
        Task<List<PhotoDocumentation>> GetAllPhotoDocumentationAsync(string id);

        /// <summary>
        /// Deletes photo documentation
        /// </summary>
        /// <param name="id">The report ID</param>
        /// <param name="fotoCode">The photo code</param>
        /// <returns>Task representing the delete operation</returns>
        Task DeletePhotoDocumentationAsync(string id, int fotoCode);
    }
}