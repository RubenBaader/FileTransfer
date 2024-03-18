namespace FileTransfer.Models.Dtos
{
    public class FileMetadataDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public long FileSizeBytes { get; set; }
        public string FileType { get; set; }
        public DateTime UploadDateTime { get; set; }
        public Guid Guid { get; set; }
        

    }

}
