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
        public Task<IEnumerable<FileMetadata>> GetAllFileMetadata(int userId);
        public Task<FileMetadata> GetSingleFileMetadata(Guid guid);
        public Task<FileBody> GetFileBody(Guid guid);
    }
}
