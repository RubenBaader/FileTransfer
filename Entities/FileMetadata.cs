using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileTransfer.Api.Entities
{
    public class FileMetadata
    {
        [Key]
        public int UploadedFileId { get; set; }
        public string FileName { get; set; }
        public long FileSizeBytes { get; set; }
        public string FileType { get; set; }
        public DateTime UploadDateTime { get; set; }

        [ForeignKey(nameof(UploadedFileId))]
        public UploadedFile UploadedFile { get; set; }
    }
}
