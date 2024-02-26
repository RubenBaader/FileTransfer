namespace FileTransfer.Api.Entities
{
    public class UploadedFile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Guid Guid { get; set; }
        public FileBody Body { get; set; }
        public FileMetadata Metadata { get; set; }
    }
}
