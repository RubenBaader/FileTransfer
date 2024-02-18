using FileTransfer.Api.Entities;
using FileTransfer.Api.Repositories.Contracts;

namespace FileTransfer.Api.Repositories
{
    public class FileRepository : IFileRepository
    {
        public Task<FileMetadata> AddFile(FileMetadata fileMetadata, FileBody filebody)
        {
            throw new NotImplementedException();
        }
        public Task<FileMetadata> GetSingleFileMetadata(int fileId)
        {
            throw new NotImplementedException();
        }
        public Task<FileMetadata> GetAllFileMetadata(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<FileBody> GetFileBody(int fileId)
        {
            throw new NotImplementedException();
        }

        
    }
}
