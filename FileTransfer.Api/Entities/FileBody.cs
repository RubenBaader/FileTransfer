using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileTransfer.Api.Entities
{
    public class FileBody
    {
        [Key]
        public int UploadedFileId { get; set; }
        public byte[] Content { get; set; }

        [ForeignKey(nameof(UploadedFileId))]
        public DBFile UploadedFile { get; set; }
    }
}
