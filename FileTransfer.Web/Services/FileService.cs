using FileTransfer.Models.Dtos;
using FileTransfer.Web.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace FileTransfer.Web.Services
{
    public class FileService : IFileService
    {
        private readonly HttpClient httpClient;

        public FileService(HttpClient httpClient)
        {
                this.httpClient = httpClient;
        }

        public Task DeleteFile(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task DownloadFile(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<FileMetadataDto> GetFile(Guid guid)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FileMetadataDto>> GetFiles(int userId)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/file/{userId}/GetFiles");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<FileMetadataDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<FileMetadataDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} message: {message}");
                }
            }
            catch (Exception)
            {
                //log exception
                throw;
            }
        }

        public Task UploadFile(IFormFile formfile)
        {
            throw new NotImplementedException();
        }
    }
}
