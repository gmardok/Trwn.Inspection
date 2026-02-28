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

        // POST: api/InspectionReports/5/Foto
        [HttpPost("{id}/foto")]
        public async Task<IActionResult> AddPhotoDocumentation(int id, PhotoDocumentation photoDocumentation )
        {
            var newFoto = await _inspectionReportsService.AddInspectionFoto(id, photoDocumentation);
            if (newFoto == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetPhotoDocumentation), new { id, fotoId = newFoto.Code }, newFoto);
        }

        // GET: api/InspectionReports/5/Foto/5
        [HttpGet("{id}/foto/{fotoCode}")]
        public async Task<IActionResult> GetPhotoDocumentation(int id, int fotoCode)
        {
            var foto = await _inspectionReportsService.GetInspectionFoto(id, fotoCode);
            if (foto == null)
            {
                return NotFound();
            }
            return Ok(foto);
        }

        // GET: api/InspectionReports/5/Foto
        [HttpGet("{id}/foto")]
        public async Task<IActionResult> GetAllPhotoDocumentation(int id)
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
        public async Task<IActionResult> DeleteInspectionReport(int id)
        {
            await _inspectionReportsService.DeleteInspectionReport(id);

            return NoContent();
        }

        // DELETE: api/InspectionReports/5/Foto/5
        [HttpDelete("{id}/foto/{fotoCode}")]
        public async Task<IActionResult> DeletePhotoDocumentation(int id, int fotoCode)
        {
            await _inspectionReportsService.DeleteInspectionFoto(id, fotoCode);

            return NoContent();
        }
    }
}
