using FileTransfer.Api.Entities;

namespace FileTransfer.Api.Repositories.Contracts
{
    public interface IFileRepository
    {
        //upload file takes body + metadata
        //public Task<FileMetadata> AddFileMetadata(FileMetadata fileMetadata);
        //public Task<FileBody> AddFileBody(FileBody fileBody);
        public Task<FileMetadata> AddFile(IFormFile file);
        public Task<FileMetadata> GetSingleFileMetadata(int fileId);
        public Task<FileMetadata> GetAllFileMetadata(int userId);
        public Task<FileBody> GetFileBody(int fileId);
    }
}
