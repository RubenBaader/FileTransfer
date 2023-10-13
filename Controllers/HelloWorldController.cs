using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FileTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        private readonly string FilePath = "data.txt";

        [HttpPost (Name = "UploadWorld")]
        public async Task<IActionResult> UploadFile(IFormFile formFile)
        {
            try
            {
                if(formFile != null && formFile.Length > 0)
                {
                    using (var stream = new FileStream(FilePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    return Ok("File uploaded succesfully!");
                }
                else
                {
                    return BadRequest("No file uploaded");
                }
            }
            catch(Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet(Name = "GetHelloWorld")]
        public async Task<IActionResult> GetHelloWorld()
        {
            try
            {
                if(System.IO.File.Exists(FilePath))
                {
                    byte[] FileBytes = await System.IO.File.ReadAllBytesAsync(FilePath);
                    string FileName = Path.GetFileName(FilePath);

                    return File(FileBytes, "application/octet-stream", FileName);
                }
                else
                {
                    return NotFound("File not found");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
