﻿using FileTransfer.Api.Data;
using FileTransfer.Api.Entities;
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

        public async Task<FileMetadata> AddFile(IFormFile file)
        {
            Guid fileGuid = Guid.NewGuid();

            var metadataResult = await fileTransferDBContext.FileMetadata.AddAsync(
                ExtractMetadata(file, fileGuid)
            );
            var bodyResult = await fileTransferDBContext.FileBody.AddAsync(
                await ExtractFileBody(file, fileGuid)
            );

            await fileTransferDBContext.SaveChangesAsync();
            return metadataResult.Entity;

        }
        public FileMetadata ExtractMetadata(IFormFile file, Guid guid)
        {
            FileMetadata metadata = new FileMetadata
            {
                FileGuid = guid,
                FileName = file.FileName,
                FileSizeBytes = file.Length,
                FileType = file.ContentType,
                UploadDateTime = DateTime.Now,
                UserId = 1,
            };

            return metadata;
        }
        public async Task<FileBody> ExtractFileBody(IFormFile file, Guid guid)
        {
            FileBody fileBody = new FileBody();
            fileBody.FileGuid = guid;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                fileBody.Body = ms.ToArray();
            }

            return fileBody;
        }


        public async Task<IEnumerable<FileMetadata>> GetAllFileMetadata(int userId)
        {
            var dataList = await this.fileTransferDBContext.FileMetadata
                                        .Where(f => f.UserId == userId).ToListAsync();
            
            return dataList;
        }

        public async Task<FileMetadata> GetSingleFileMetadata(string guidString)
        {
            var data = await this.fileTransferDBContext.FileMetadata
                        .SingleOrDefaultAsync(f => f.FileGuid.ToString() == guidString);

            return data;
        }
        public async Task<FileBody> GetFileBody(string guidString)
        {
            var data = await this.fileTransferDBContext.FileBody
                        .SingleOrDefaultAsync(b => b.FileGuid.ToString() == guidString);

            return data;
        }

    }
}
