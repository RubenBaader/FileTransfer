namespace FileTransfer.Api.Entities
{
    public class FileBody
    {
        public Guid FileGuid { get; set; }
        public byte[] Body { get; set; }
    }
}
