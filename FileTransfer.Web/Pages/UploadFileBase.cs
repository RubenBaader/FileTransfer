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
        protected IBrowserFile browserFile { get; set; }

        protected void LoadFiles(InputFileChangeEventArgs e)
        {
            if (e.FileCount > 0)
            {
                fileCount = e.FileCount;
                browserFile = e.File;
                fileName = browserFile.Name;

                FileService.UploadFile(e.File as IFormFile);
            }
        }
    }
}
