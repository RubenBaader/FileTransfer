
using FileTransfer.Models.Dtos;
using Microsoft.AspNetCore.Http;

namespace FileTransfer.Web.Services.Contracts
{
    public interface IFileService
    {
        public Task DownloadFile(Guid guid);
        public Task UploadFile(IFormFile formfile);
        public Task DeleteFile(Guid guid);
        public Task<IEnumerable<FileMetadataDto>> GetFiles(int userId);
        public Task<FileMetadataDto> GetFile(Guid guid);
    }
}
