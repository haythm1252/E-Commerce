using Microsoft.AspNetCore.Http;  
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Application.Common;


namespace E_Commerce.Web.Helpers
{
    public class FileUploadService : IFileUploadService
    {
        private readonly string _basePath;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _basePath = environment.WebRootPath;
        }
        public async Task<string> UploadFileAsync(IFormFile file, string targetFolder)
        {
            if (file == null || file.Length == 0)
                return null;

            try
            {
                //generate the uniqe fillname guid+extention
                string FileExt = Path.GetExtension(file.FileName).ToLower();
                string FileName = Guid.NewGuid().ToString() + FileExt;

                //check if the folder exist or not and if not create it 
                string FolderPath = Path.Combine(_basePath, targetFolder);
                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);

                //combine the folder path and file name
                string FilePath = Path.Combine(FolderPath, FileName);

                //add the file in the server
                using(var stream = new FileStream(FilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // return to save in DB
                string relativePath = Path.Combine(targetFolder, FileName).Replace("\\", "/");
                return relativePath;

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error uploading file: {ex.Message}", ex);
            }
        }
        public bool DeleteFile(string FileName)
        {
            try
            {

                var FullPath = Path.Combine(_basePath, FileName);
                if (File.Exists(FullPath))
                {
                    File.Delete(FullPath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error Deleting file: {ex.Message}", ex);
            }
        }
    }
}
