namespace FileTransfer.Api.Entities
{
    public class FileBody
    {
        public int Id { get; set; }
        public Guid FileGuid { get; set; }
        public byte[] Body { get; set; }
    }
}
