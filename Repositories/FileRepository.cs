using FileTransfer.Api.Data;
using FileTransfer.Api.Entities;
using FileTransfer.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FileTransfer.Api.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly FileTransferDBContext fileTransferDBContext;

        public FileRepository(FileTransferDBContext fileTransferDBContext)
        {
            this.fileTransferDBContext = fileTransferDBContext;
        }

        public async Task<FileMetadata> AddFile(IFormFile formFile)
        {
            DBFile file = new();
            file.Guid = Guid.NewGuid();
            // Placeholder userId = 1
            file.UserId = 1;
            file.Metadata = ExtractMetadata(formFile);
            file.Body = await ExtractFileBody(formFile);

            var receipt = await fileTransferDBContext.DBFiles.AddAsync(file);
            await fileTransferDBContext.SaveChangesAsync();

            return receipt.Entity.Metadata;

        }
        public FileMetadata ExtractMetadata(IFormFile file)
        {
            FileMetadata metadata = new FileMetadata
            {
                FileName = file.FileName,
                FileSizeBytes = file.Length,
                FileType = file.ContentType,
                UploadDateTime = DateTime.Now,
            };

            return metadata;
        }
        public async Task<FileBody> ExtractFileBody(IFormFile file)
        {
            FileBody fileBody = new FileBody();
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                fileBody.Content = ms.ToArray();
            }

            return fileBody;
        }

        public async Task<IEnumerable<FileMetadata>> GetAllFileMetadata(int userId)
        {
            var dataList = await this.fileTransferDBContext.DBFiles
                                .Include(f => f.Metadata)
                                .Where(f => f.UserId == userId)
                                .Select(f => f.Metadata).ToListAsync();
            
            if (dataList.Count() == 0)
            {
                return Enumerable.Empty<FileMetadata>();
            }

            return dataList;
        }

        public async Task<FileMetadata> GetSingleFileMetadata(Guid guid)
        {
            var data = await this.fileTransferDBContext.DBFiles
                                .Include(f => f.Metadata)
                                .SingleOrDefaultAsync(f => f.Guid == guid);

            return data.Metadata;
        }
        public async Task<DBFile> GetFile(Guid guid)
        {
            var data = await this.fileTransferDBContext.DBFiles
                                .Include(f => f.Metadata)
                                .Include(f => f.Body)
                                .SingleOrDefaultAsync(f => f.Guid == guid);

            return data;
        }

    }
}
