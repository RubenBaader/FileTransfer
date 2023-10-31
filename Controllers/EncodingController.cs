using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncodingController : ControllerBase
    {
        // maximum size for uploaded file: 10 mb
        private readonly long maxFileSize = 10 * 1024 * 1024;

        [HttpPost ("encode", Name ="EncodeSingleFile")]
        public async Task<IActionResult> EncodeSingleFile(IFormFile encodeFile)
        {
            try
            {
                // Validate file existence and size
                if (encodeFile == null || encodeFile.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }
                if (encodeFile.Length > maxFileSize)
                {
                    return BadRequest("File too large");
                }

                // Copy and encode file data; do not save
                using var memoryStream = new MemoryStream();

                await encodeFile.CopyToAsync (memoryStream);
                byte[] fileBytes = memoryStream.ToArray ();

                string base64Data = Convert.ToBase64String (fileBytes);

                //return Ok(base64Data);
                return new JsonResult(base64Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
