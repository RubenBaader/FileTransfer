using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FileTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        //private string? UploadedFileName { get; set; }
        private readonly string UploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        //private string UploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", UploadedFileName);


        [HttpPost (Name = "UploadSingleFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if(file != null && file.Length > 0)
                {
                    if (!Directory.Exists(UploadFolder) )
                        Directory.CreateDirectory(UploadFolder);

                    string UploadedFileName = file.FileName;
                    string UploadPath = Path.Combine(UploadFolder, UploadedFileName);
                    using (var stream = new FileStream(UploadPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return Ok($"File uploaded succesfully: {UploadedFileName}");
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


        [HttpGet(Name = "DownloadSingleFile")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            //string UploadedFileName = "hello";
            string FilePath = Path.Combine(UploadFolder, fileName);
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
                    return NotFound($"File not found: {FilePath}");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
