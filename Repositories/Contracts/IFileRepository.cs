using FileTransfer.Api.Entities;
using FileTransfer.Models.Dtos;

namespace FileTransfer.Api.Repositories.Contracts
{
    public interface IFileRepository
    {
        //upload file takes body + metadata
        //extract file metadata
        //extract file body
        public FileMetadata ExtractMetadata(IFormFile file, Guid guid);
        public Task<FileBody> ExtractFileBody(IFormFile file, Guid guid);
        public Task<FileMetadata> AddFile(IFormFile file);
        public Task<FileMetadata> GetSingleFileMetadata(int fileId);
        public Task<IEnumerable<FileMetadata>> GetAllFileMetadata(int userId);
        public Task<FileBody> GetFileBody(int fileId);
    }
}
