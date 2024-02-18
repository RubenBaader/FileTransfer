using FileTransfer.Api.Data;
using FileTransfer.Api.Entities;
using FileTransfer.Api.Repositories.Contracts;

namespace FileTransfer.Api.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly FileTransferDBContext fileTransferDBContext;

        public FileRepository(FileTransferDBContext fileTransferDBContext)
        {
            this.fileTransferDBContext = fileTransferDBContext;
        }

        public async Task<FileMetadata> AddFile(IFormFile file)
        {
            Guid fileGuid = Guid.NewGuid();
            FileMetadata metadata = new FileMetadata
            {
                FileGuid = fileGuid,
                FileName = file.FileName,
                FileSizeBytes = file.Length,
                FileType = file.ContentType,
                UploadDateTime = DateTime.Now,
                UploaderUserId = 1,
            };

            var result = await fileTransferDBContext.Files.AddAsync(metadata);
            await fileTransferDBContext.SaveChangesAsync();
            return result.Entity;

            //FileBody fileBody = new FileBody
            //{
            //    FileGuid = fileGuid,
            //    Body = file.
            //};
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
