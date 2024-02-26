using Microsoft.EntityFrameworkCore;
using FileTransfer.Api.Entities;

namespace FileTransfer.Api.Data
{
    public class FileTransferDBContext : DbContext
    {
        public FileTransferDBContext(DbContextOptions<FileTransferDBContext> options):base(options)
        {
                
        }

        public DbSet<FileMetadata> FileMetadata { get; set; }
        public DbSet<FileBody> FileBody { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
