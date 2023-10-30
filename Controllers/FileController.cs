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
        private readonly string UploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");


        [HttpPost ("upload", Name = "UploadSingleFile")]
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

        [HttpGet ("list", Name = "ListFiles")]
        public IActionResult ListFiles()
        {
            try
            { 
                // list all file names in folder, return empty array if no files exist
                if(Directory.Exists(UploadFolder) )
                {
                    string?[] FileNames = Directory.GetFiles(UploadFolder)
                        .Select(Path.GetFileName)
                        .ToArray();

                    return Ok(FileNames);
                }
                else
                {
                    // Upload folder will not always exist on first load
                    return Ok(Array.Empty<string>());
                }
            }
            catch(Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        
        [HttpGet("download/{fileName}", Name = "DownloadSingleFile")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
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
