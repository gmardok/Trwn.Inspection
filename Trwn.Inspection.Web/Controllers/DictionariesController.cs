using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trwn.Inspection.Data;

namespace Trwn.Inspection.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {
        private readonly InspectionDbContext _context;
        private readonly IAuthSessionContext _authSessionContext;

        public DictionariesController(InspectionDbContext context, IAuthSessionContext authSessionContext)
        {
            _context = context;
            _authSessionContext = authSessionContext;
        }

        /// <summary>
        /// Returns a list of distinct values from PhotoDocumentation.Description.
        /// </summary>
        [HttpGet("defects")]
        public async Task<ActionResult<IEnumerable<string>>> GetDefects()
        {
            var sessionId = _authSessionContext.GetSessionId();
            if (sessionId is null)
            {
                return Unauthorized();
            }

            var defects = await _context.PhotoDocumentations
                .Where(p => _context.InspectionReports.Any(r =>
                    r.Id == p.InspectionReportId && r.AuthSessionId == sessionId))
                .Select(p => p.Description)
                .Where(d => !string.IsNullOrEmpty(d))
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();

            return Ok(defects);
        }
    }
}
