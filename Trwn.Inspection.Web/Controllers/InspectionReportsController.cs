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
        public async Task<IActionResult> GetInspectionReports()
        {
            var result = await _inspectionReportsService.GetInspectionReports();
            return Ok(result);
        }

        // GET: api/InspectionReports/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInspectionReport(string id)
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
            report.Id = Guid.NewGuid().ToString();
            var newReport = await _inspectionReportsService.AddInspectionReport(report);
            return CreatedAtAction(nameof(GetInspectionReport), new { id = newReport.Id }, newReport);
        }

        // PUT: api/InspectionReports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInspectionReport(string id, InspectionReport report)
        {
            var updatedReport = _inspectionReportsService.UpdateInspectionReport(id, report);
            if (updatedReport == null)
            {
                return NotFound();
            }

            return Ok(updatedReport);
        }

        // POST: api/InspectionReports/5/Foto
        [HttpPost("{id}/foto")]
        public async Task<IActionResult> AddFotoDocumentation(string id, FotoDocumentation fotoDocumentation )
        {
            fotoDocumentation.Id = Guid.NewGuid().ToString();
            var newFoto = await _inspectionReportsService.AddInspectionFoto(id, fotoDocumentation);
            if (newFoto == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetFotoDocumentation), new { id = id, fotoId = newFoto.Id }, newFoto);
        }

        // GET: api/InspectionReports/5/Foto/5
        [HttpGet("{id}/foto/{fotoId}")]
        public async Task<IActionResult> GetFotoDocumentation(string id, string fotoId)
        {
            var foto = await _inspectionReportsService.GetInspectionFoto(id, fotoId);
            if (foto == null)
            {
                return NotFound();
            }
            return Ok(foto);
        }

        // GET: api/InspectionReports/5/Foto
        [HttpGet("{id}/foto")]
        public async Task<IActionResult> GetAllFotoDocumentation(string id)
        {
            var photos = await _inspectionReportsService.GetAllInspectionFoto(id);
            if (photos == null)
            {
                return NotFound();
            }
            return Ok(photos);
        }

        // DELETE: api/InspectionReports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspectionReport(string id)
        {
            await _inspectionReportsService.DeleteInspectionReport(id);

            return NoContent();
        }

        // DELETE: api/InspectionReports/5/Foto/5
        [HttpDelete("{id}/foto/{fotoId}")]
        public async Task<IActionResult> DeleteFotoDocumentation(string id, string fotoId)
        {
            await _inspectionReportsService.DeleteInspectionFoto(id, fotoId);

            return NoContent();
        }
    }
}
