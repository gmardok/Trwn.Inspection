using Microsoft.AspNetCore.Mvc;
using Trwn.Inspection.Core;

namespace Trwn.Inspection.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        /// <summary>
        /// Upload or update a photo for PhotoDocumentation.
        /// </summary>
        /// <param name="id">PhotoDocumentation.Id</param>
        /// <param name="picture">The photo file to upload</param>
        [HttpPost("{id}")]
        public async Task<IActionResult> AddOrUpdate(int id, IFormFile picture)
        {
            if (picture == null || picture.Length == 0)
                return BadRequest("No file uploaded.");

            var extension = Path.GetExtension(picture.FileName);
            if (string.IsNullOrEmpty(extension))
                return BadRequest("File must have an extension.");

            await using var stream = picture.OpenReadStream();
            var success = await _photoService.AddOrUpdatePhoto(id, stream, extension);

            if (!success)
                return NotFound("Photo documentation record not found.");

            return Ok();
        }

        /// <summary>
        /// Get a photo by PhotoDocumentation.Id.
        /// </summary>
        /// <param name="id">PhotoDocumentation.Id</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var (fileStream, contentType) = await _photoService.GetPhoto(id);

            if (fileStream == null || contentType == null)
                return NotFound("Photo not found.");

            return File(fileStream, contentType);
        }
    }
}
