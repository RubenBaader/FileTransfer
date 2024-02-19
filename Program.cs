
using FileTransfer.Api.Data;
using FileTransfer.Api.Repositories;
using FileTransfer.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FileTransfer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Inject DB instance
            builder.Services.AddDbContextPool<FileTransferDBContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("FileTransferConnection"))
            );

            builder.Services.AddScoped<IFileRepository, FileRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}