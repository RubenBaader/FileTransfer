using FileTransfer.Models.Dtos;
using FileTransfer.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IO;

namespace FileTransfer.Web.Pages
{
    public class DisplayFilesBase : ComponentBase
    {
        [Inject]
        public IFileService FileService { get; set; }
        [Inject]
        public IJSRuntime JS { get; set; }
        public IEnumerable<FileMetadataDto> Files { get; set; }
        public string? ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                //get files for user - here hardcoded to test id
                Files = await FileService.GetFiles(1);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task DownloadFile (Guid guid, string name)
        {
            var fileStream = await FileService.DownloadFile(guid);
            var fileName = name;

            using var streamRef = new DotNetStreamReference(stream: fileStream);

            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
        protected async Task DeleteFile (int id)
        {

        }
    }
}
