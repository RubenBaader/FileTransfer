using FileTransfer.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FileTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string storagePath;

        public FileController()
        {
            this.storagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(storagePath))
            { 
                Directory.CreateDirectory(storagePath);
            }
        }

        [HttpPost]
        public async Task<ActionResult<FileDto>> UploadFile(IFormFile uploadFile)
        {
            try
            {
                if (uploadFile == null || uploadFile.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                FileDto file = new FileDto {
                    FileName = uploadFile.FileName,
                    FileSizeBytes = uploadFile.Length,
                    FileType = uploadFile.ContentType,
                    UploadDateTime = DateTime.Now,
                    UploaderUserId = "Mom",
                    FilePath = uploadFile.FileName,
                    Checksum = "hunter2",
                    Permissions = "Mom and Dad"
                    };

                //string fileName = uploadFile.FileName;
                string uploadPath = Path.Combine(storagePath, file.FilePath);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(stream);
                }

                return Ok($"File uploaded succesfully: {file.FileName} at {file.UploadDateTime}. Type: {file.FileType}");

            }
            catch(Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<FileDto>> ListFiles()
        {
            try
            { 
                var files = new List<string>();

                // list all file names in folder, return empty array if no files exist
                if(Directory.Exists(storagePath) )
                {
                    string[] FileNames = Directory.GetFiles(storagePath)
                        .Select(Path.GetFileName)
                        .ToArray();

                    return Ok(FileNames);
                }
                else
                {
                    return Ok(Array.Empty<string>());
                }
            }
            catch(Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        
        [HttpGet]
        [Route("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            string FilePath = Path.Combine(storagePath, fileName);
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
