using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trwn.Inspection.Data;

namespace Trwn.Inspection.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {
        private readonly InspectionDbContext _context;

        public DictionariesController(InspectionDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of distinct values from PhotoDocumentation.Description.
        /// </summary>
        [HttpGet("defects")]
        public async Task<ActionResult<IEnumerable<string>>> GetDefects()
        {
            var defects = await _context.PhotoDocumentations
                .Select(p => p.Description)
                .Where(d => !string.IsNullOrEmpty(d))
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();

            return Ok(defects);
        }
    }
}
