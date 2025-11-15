using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfluencerMarketplace.API.Controllers
{
    /// <summary>
    /// File upload endpoints for images and documents
    /// </summary>
    [ApiController]
    [Route("api/upload")]
    [Authorize]
    [Produces("application/json")]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;

        public FileUploadController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        /// <summary>
        /// Upload an image file
        /// </summary>
        /// <param name="file">Image file to upload</param>
        /// <returns>File upload result with URL</returns>
        /// <response code="200">File uploaded successfully</response>
        /// <response code="400">Invalid file or file validation failed</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("image")]
        [ProducesResponseType(typeof(FileUploadResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<FileUploadResult>> UploadImage(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest(new { message = "No file provided" });
            }

            using var stream = file.OpenReadStream();
            var result = await _fileUploadService.UploadImageAsync(stream, file.FileName, file.Length);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Upload a document file
        /// </summary>
        /// <param name="file">Document file to upload</param>
        /// <returns>File upload result with URL</returns>
        /// <response code="200">File uploaded successfully</response>
        /// <response code="400">Invalid file or file validation failed</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("document")]
        [ProducesResponseType(typeof(FileUploadResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<FileUploadResult>> UploadDocument(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest(new { message = "No file provided" });
            }

            using var stream = file.OpenReadStream();
            var result = await _fileUploadService.UploadDocumentAsync(stream, file.FileName, file.Length);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="filename">Name of the file to delete</param>
        /// <returns>Success message</returns>
        /// <response code="200">File deleted successfully</response>
        /// <response code="400">File not found or deletion failed</response>
        /// <response code="401">Unauthorized</response>
        [HttpDelete("{filename}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> DeleteFile(string filename)
        {
            var result = await _fileUploadService.DeleteFileAsync(filename);
            
            if (!result)
            {
                return BadRequest(new { message = "File not found or could not be deleted" });
            }

            return Ok(new { message = "File deleted successfully" });
        }
    }
}

