using Microsoft.EntityFrameworkCore;
using FileTransfer.Api.Entities;

namespace FileTransfer.Api.Data
{
    public class FileTransferDBContext : DbContext
    {
        public FileTransferDBContext(DbContextOptions<FileTransferDBContext> options):base(options)
        {
                
        }

        public DbSet<UploadFile> Files { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
