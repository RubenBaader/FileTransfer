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

                //return Ok(new
                //{
                //    name = encodeFile.FileName,
                //    body = base64Data,
                //});
                return new JsonResult(base64Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost ("decode", Name = "DecodeSingleFile")]
        public async Task<IActionResult> DecodeSingleFile ([FromBody] string base64Data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(base64Data))
                {
                    return BadRequest("No base64 data provided.");
                }

                byte[] fileBytes = Convert.FromBase64String(base64Data);

                string contentType = "application/any";

                string fileName = "converted_file.txt";

                return File(fileBytes, contentType, fileName);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }
    }
}
