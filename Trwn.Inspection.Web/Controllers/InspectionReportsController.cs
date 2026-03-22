using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trwn.Inspection.Configuration;
using Trwn.Inspection.Core.Interfaces;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionReportsController : ControllerBase
    {
        private readonly IInspectionReportsService _inspectionReportsService;
        private readonly IInspectionReportGenerator _reportGenerator;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;

        public InspectionReportsController(
            IInspectionReportsService inspectionReportsService,
            IInspectionReportGenerator reportGenerator,
            IWebHostEnvironment env,
            IOptions<AppSettings> appSettings)
        {
            _inspectionReportsService = inspectionReportsService;
            _reportGenerator = reportGenerator;
            _env = env;
            _appSettings = appSettings.Value;
        }
        // GET: api/InspectionReports
        [HttpGet]
        public async Task<IActionResult> GetInspectionReports()
        {
            var result = await _inspectionReportsService.GetInspectionReports();
            return Ok(result);
        }

        // GET: api/InspectionReports/5/pdf
        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GetInspectionReportPdf(int id)
        {
            var report = await _inspectionReportsService.GetInspectionReport(id);
            if (report == null)
            {
                return NotFound();
            }
            var photoPath = Path.Combine(_env.ContentRootPath, _appSettings.PhotoStoragePath ?? "Photos");
            var pdfBytes = _reportGenerator.GeneratePdfReport(report, photoPath);
            var fileName = $"InspectionReport_{report.ReportNo?.Replace("/", "-") ?? id.ToString()}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        // GET: api/InspectionReports/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInspectionReport(int id)
        {
            var report = await _inspectionReportsService.GetInspectionReport(id);
            if (report == null)
            {
                return NotFound();
            }
            return Ok(report);
        }

        // POST: api/InspectionReports
        [HttpPost]
        public async Task<IActionResult> AddInspectionReport(InspectionReport report)
        {
            var newReport = await _inspectionReportsService.AddInspectionReport(report);
            return CreatedAtAction(nameof(GetInspectionReport), new { id = newReport.Id }, newReport);
        }

        // PUT: api/InspectionReports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInspectionReport(int id, InspectionReport report)
        {
            var updatedReport = _inspectionReportsService.UpdateInspectionReport(id, report);
            if (updatedReport == null)
            {
                return NotFound();
            }

            return Ok(updatedReport);
        }

        // DELETE: api/InspectionReports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspectionReport(int id)
        {
            await _inspectionReportsService.DeleteInspectionReport(id);

            return NoContent();
        }
    }
}
