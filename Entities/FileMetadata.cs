namespace FileTransfer.Api.Entities
{
    public class FileMetadata
    {
        public int Id { get; set; }
        public Guid FileGuid { get; set; }
        public string FileName { get; set; }
        public long FileSizeBytes { get; set; }
        public string FileType { get; set; }
        public DateTime UploadDateTime { get; set; }
        public int UserId { get; set; }
    }
}
