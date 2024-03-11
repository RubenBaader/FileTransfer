using FileTransfer.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace FileTransfer.Web.Pages
{
    public class UploadFileBase : ComponentBase
    {
        [Inject]
        public IFileService FileService { get; set; }

        protected string fileName {  get; set; }
        protected int fileCount { get; set; }
        public IBrowserFile browserFile { get; set; }

        public async void LoadFiles(InputFileChangeEventArgs e)
        {
            if (e.File != null)
            {
                fileCount = e.FileCount;
                browserFile = e.File;
                fileName = browserFile.Name;

                await FileService.UploadFile(browserFile);
            }
        }
    }
}
