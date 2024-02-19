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

            FileBody fileBody = new FileBody();
            fileBody.FileGuid = fileGuid;
            using (var ms = new  MemoryStream())
            {
                await file.CopyToAsync(ms);
                fileBody.Body = ms.ToArray();
            }


            var metadataResult = await fileTransferDBContext.FileMetadata.AddAsync(metadata);
            var bodyResult = await fileTransferDBContext.FileBody.AddAsync(fileBody);

            await fileTransferDBContext.SaveChangesAsync();
            return metadataResult.Entity;

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
