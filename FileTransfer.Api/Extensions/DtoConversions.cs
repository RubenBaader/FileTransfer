using FileTransfer.Api.Entities;
using FileTransfer.Models.Dtos;

namespace FileTransfer.Api.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<FileMetadataDto> ConvertToDto(this IEnumerable<FileMetadata> fileMetadatas)
        {
            return (from fileMetadata in fileMetadatas
                    select new FileMetadataDto
                    {
                        Id = fileMetadata.UploadedFileId,
                        FileName = fileMetadata.FileName,
                        FileSizeBytes = fileMetadata.FileSizeBytes,
                        FileType = fileMetadata.FileType,
                        UploadDateTime = fileMetadata.UploadDateTime,
                        Guid = fileMetadata.UploadedFile.Guid,
                    }).ToList();
        }

        public static FileMetadataDto ConvertToDto (this FileMetadata fileMetadata)
        {
            return new FileMetadataDto
            {
                Id = fileMetadata.UploadedFileId,
                FileName = fileMetadata.FileName,
                FileSizeBytes = fileMetadata.FileSizeBytes,
                FileType = fileMetadata.FileType,
                UploadDateTime = fileMetadata.UploadDateTime,
                Guid = fileMetadata.UploadedFile.Guid,
            };
        }
    }
}
