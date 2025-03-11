using Microsoft.AspNetCore.Mvc;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionReportsController : ControllerBase
    {
        private readonly IInspectionReportsService _inspectionReportsService;

        public InspectionReportsController(IInspectionReportsService inspectionReportsService)
        {
            _inspectionReportsService = inspectionReportsService;
        }
        // GET: api/InspectionReports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InspectionReport>>> GetInspectionReports()
        {
            return await Task.FromResult(Ok(_inspectionReportsService.GetInspectionReports()));
        }

        // GET: api/InspectionReports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InspectionReport>> GetInspectionReport(Guid id)
        {
            var report = _inspectionReportsService.GetInspectionReport(id);
            if (report == null)
            {
                return await Task.FromResult(NotFound());
            }
            return await Task.FromResult(Ok(report));
        }

        // POST: api/InspectionReports
        [HttpPost]
        public async Task<ActionResult<InspectionReport>> PostInspectionReport(InspectionReport report)
        {
            var newReport = _inspectionReportsService.AddInspectionReport(report);
            return await Task.FromResult(CreatedAtAction(nameof(GetInspectionReport), new { id = newReport.Id }, newReport));
        }

        // PUT: api/InspectionReports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInspectionReport(Guid id, InspectionReport report)
        {
            var updatedReport = _inspectionReportsService.UpdateInspectionReport(id, report);
            if (updatedReport == null)
            {
                return await Task.FromResult(NotFound());
            }

            return await Task.FromResult(NoContent());
        }

        // DELETE: api/InspectionReports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspectionReport(Guid id)
        {
            await _inspectionReportsService.DeleteInspectionReport(id);

            return await Task.FromResult(NoContent());
        }
    }
}
