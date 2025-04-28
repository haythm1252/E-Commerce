using E_Commerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace E_Commerce.Web.Helpers
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string targetFolder);
        //Task<List<string>> UploadFilesAsync(List<IFormFile> files, string targetFolder);
        bool DeleteFile(string FileName);
    }
}
