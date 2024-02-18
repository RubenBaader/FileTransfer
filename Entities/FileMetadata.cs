namespace FileTransfer.Api.Entities
{
    public class FileMetadata
    {
        public Guid FileGuid { get; set; }
        public string FileName { get; set; }
        public long FileSizeBytes { get; set; }
        public string FileType { get; set; }
        public DateTime UploadDateTime { get; set; }
        public int UploaderUserId { get; set; }
        //public string FilePath { get; set; }
    }
}
