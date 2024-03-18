using FileTransfer.Models.Dtos;
using FileTransfer.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace FileTransfer.Web.Pages
{
    public class DisplayFilesBase : ComponentBase
    {
        [Inject]
        public IFileService FileService { get; set; }
        public IEnumerable<FileMetadataDto> Files { get; set; }
        public string ErrorMessage { get; set; }

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

        protected async Task DownloadFile (Guid guid)
        {
            await FileService.DownloadFile(guid);
        }
        protected async Task DeleteFile (int id)
        {

        }
    }
}
