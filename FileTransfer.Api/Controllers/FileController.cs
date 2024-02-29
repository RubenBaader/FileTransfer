using FileTransfer.Api.Entities;
using FileTransfer.Api.Repositories.Contracts;
using FileTransfer.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FileTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileRepository fileRepository;

        public FileController(IFileRepository fileRepository)
        {
            this.fileRepository = fileRepository;
        }

        [HttpPost]
        public async Task<ActionResult<FileMetadataDto>> UploadFile(IFormFile uploadFile)
        {
            try
            {
                if (uploadFile == null || uploadFile.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                FileMetadata metadata = await this.fileRepository.AddFile(uploadFile);

                //Convert to Dto
                FileMetadataDto file = new FileMetadataDto
                {
                    Id = -1,
                    FileName = metadata.FileName,
                    FileSizeBytes = metadata.FileSizeBytes,
                    FileType = metadata.FileType,
                    UploadDateTime = metadata.UploadDateTime,
                    UserId = -1,
                };


                return Ok($"File uploaded succesfully: {file.FileName} at {file.UploadDateTime}. Type: {file.FileType}");

            }
            catch(Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{userId}/GetFiles")]
        public async Task<ActionResult<IEnumerable<FileMetadata>>> ListFiles(int userId)
        {
            try
            { 
                var files = await this.fileRepository.GetAllFileMetadata(userId);

                if(files.Count() == 0)
                {
                    return NoContent();
                }
                
                return Ok(files);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }
        
        [HttpGet]
        [Route("{guid:Guid}/download")]
        public async Task<ActionResult> DownloadFile(Guid guid)
        {
            try
            {
                var file = await fileRepository.GetFile(guid);

                if(file == null)
                {
                    return NotFound();
                }

                byte[] FileBytes = file.Body.Content;
                string FileName = file.Metadata.FileName;

                return File(FileBytes, "application/octet-stream", FileName);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
