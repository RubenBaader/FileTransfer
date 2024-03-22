using FileTransfer.Models.Dtos;
using FileTransfer.Web.Services.Contracts;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileTransfer.Web.Services
{
    public class FileService : IFileService
    {
        private readonly HttpClient httpClient;

        public FileService(HttpClient httpClient)
        {
                this.httpClient = httpClient;
        }
        public async Task<FileMetadataDto> UploadFile(IBrowserFile browserFile)
        {
            try
            {
                var payload = new MultipartFormDataContent();

                using var memoryStream = new MemoryStream();

                await browserFile.OpenReadStream().CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                payload.Add(
                    content: new StreamContent(memoryStream), 
                    name: "file", 
                    fileName: browserFile.Name
                );


                var response = await httpClient.PostAsync("api/file", payload);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(FileMetadataDto);
                    }

                    return await response.Content.ReadFromJsonAsync<FileMetadataDto>();

                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {
                //log exception
                throw;
            }
        }

        public async Task<Stream> DownloadFile(Guid guid)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/file/{guid}/download");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    
                    return content;
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {
                //log exception
                throw;
            }
        }
        public async Task<FileMetadataDto> DeleteFile(Guid guid)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/file/{guid}/delete");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FileMetadataDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {
                //log exception
                throw;
            }
            
            
        }

        public Task<FileMetadataDto> GetFile(Guid guid)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FileMetadataDto>> GetFiles(int userId)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/File/{userId}/GetFiles");

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
    }
}
