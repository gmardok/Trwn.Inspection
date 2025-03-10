using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionReportsController : ControllerBase
    {
        private static readonly List<InspectionReport> InspectionReports = new List<InspectionReport>();

        // GET: api/InspectionReports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InspectionReport>>> GetInspectionReports()
        {
            return await Task.FromResult(Ok(InspectionReports));
        }

        // GET: api/InspectionReports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InspectionReport>> GetInspectionReport(Guid id)
        {
            var report = InspectionReports.FirstOrDefault(r => r.Id == id);
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
            report.Id = Guid.NewGuid(); // Generate a new GUID for the ID
            InspectionReports.Add(report);
            return await Task.FromResult(CreatedAtAction(nameof(GetInspectionReport), new { id = report.Id }, report));
        }

        // PUT: api/InspectionReports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInspectionReport(Guid id, InspectionReport report)
        {
            var existingReport = InspectionReports.FirstOrDefault(r => r.Id == id);
            if (existingReport == null)
            {
                return await Task.FromResult(NotFound());
            }

            // Update properties
            existingReport.InspectionType = report.InspectionType;
            existingReport.Name = report.Name;
            existingReport.Inspector = report.Inspector;
            existingReport.ReportNo = report.ReportNo;
            existingReport.Client = report.Client;
            existingReport.ContractNo = report.ContractNo;
            existingReport.ArticleName = report.ArticleName;
            existingReport.Supplier = report.Supplier;
            existingReport.Factory = report.Factory;
            existingReport.InspectionPlace = report.InspectionPlace;
            existingReport.InspectionDate = report.InspectionDate;
            existingReport.InspectionOrder = report.InspectionOrder;
            existingReport.QualityMark = report.QualityMark;
            existingReport.InspectionStandard = report.InspectionStandard;
            existingReport.InspectionSampling = report.InspectionSampling;
            existingReport.InspectionQuantity = report.InspectionQuantity;
            existingReport.SampleSize = report.SampleSize;
            existingReport.InspectionCartonNo = report.InspectionCartonNo;
            existingReport.DefectsSummary = report.DefectsSummary;
            existingReport.InspectionResult = report.InspectionResult;
            existingReport.InspectorName = report.InspectorName;
            existingReport.FactoryRepresentative = report.FactoryRepresentative;
            existingReport.InspectionDefects = report.InspectionDefects;
            existingReport.Remarks = report.Remarks;
            existingReport.ProductionStatus = report.ProductionStatus;
            existingReport.ListOfDocuments = report.ListOfDocuments;
            existingReport.FotoDocumentation = report.FotoDocumentation;

            return await Task.FromResult(NoContent());
        }

        // DELETE: api/InspectionReports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspectionReport(Guid id)
        {
            var report = InspectionReports.FirstOrDefault(r => r.Id == id);
            if (report == null)
            {
                return await Task.FromResult(NotFound());
            }

            InspectionReports.Remove(report);
            return await Task.FromResult(NoContent());
        }
    }
}
