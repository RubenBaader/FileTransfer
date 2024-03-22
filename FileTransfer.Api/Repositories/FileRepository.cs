using FileTransfer.Api.Data;
using FileTransfer.Api.Entities;
using FileTransfer.Api.Extensions;
using FileTransfer.Api.Repositories.Contracts;
using FileTransfer.Models.Dtos;
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
            var file = new DBFile();
            file.Guid = Guid.NewGuid();
            // Placeholder userId
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
                FileType = new FileInfo(file.FileName).Extension,
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

        public async Task<IEnumerable<FileMetadataDto>> GetAllFileMetadata(int userId)
        {
            var dataList = await (from file in this.fileTransferDBContext.DBFiles
                                  join metadata in this.fileTransferDBContext.FileMetadata
                                  on file.Id equals metadata.UploadedFileId
                                  where file.UserId == userId
                                  select new FileMetadataDto
                                  {
                                      Id = file.Id,
                                      FileName = metadata.FileName,
                                      FileSizeBytes = metadata.FileSizeBytes,
                                      FileType = metadata.FileType,
                                      UploadDateTime = metadata.UploadDateTime,
                                      Guid = file.Guid,
                                  }).ToListAsync();

            if (dataList.Count() == 0)
            {
                return Enumerable.Empty<FileMetadataDto>();
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

        public async Task<FileMetadataDto> DeleteFile(Guid guid)
        {
            var file = await this.fileTransferDBContext.DBFiles
                                .Include(f => f.Metadata)
                                .Include(f => f.Body)
                                .SingleOrDefaultAsync(f => f.Guid == guid);

            var receipt = new FileMetadataDto();

            if (file != null)
            {
                this.fileTransferDBContext.FileBody.Remove(file.Body);
                this.fileTransferDBContext.FileMetadata.Remove(file.Metadata);
                this.fileTransferDBContext.DBFiles.Remove(file);

                await this.fileTransferDBContext.SaveChangesAsync();

                receipt = file.Metadata.ConvertToDto();
            }

            return receipt;
        }
    }
}
