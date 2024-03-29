﻿using FileTransfer.Api.Entities;
using FileTransfer.Api.Extensions;
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
        public async Task<ActionResult<FileMetadataDto>> UploadFile()
        {
            try
            {

                var uploadFile = Request.Form.Files.FirstOrDefault();
                
                if (uploadFile == null || uploadFile.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }
                
                FileMetadata metadata = await this.fileRepository.AddFile(uploadFile);

                //Convert to Dto
                FileMetadataDto file = metadata.ConvertToDto();


                return Ok(file);

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
                //var dtos = files.ConvertToDto();

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

        [HttpDelete]
        [Route("{guid:Guid}/delete")]
        public async Task<ActionResult> DeleteFile (Guid guid)
        {
            try
            {
                var metadata = await this.fileRepository.DeleteFile(guid);

                if (metadata == null || metadata.FileName == null)
                {
                    return NotFound();
                }

                return Ok(metadata);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
