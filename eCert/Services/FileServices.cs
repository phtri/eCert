using eCert.Utilities;
using System;
using System.IO;
using System.Web;

namespace eCert.Services
{
    public class FileServices
    {
        public void UploadMultipleFile(HttpPostedFileBase[] files) { 
            //Iterating through multiple file collection   
            string uploadedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/UploadedFiles";
            if (!Directory.Exists(uploadedPath))
            {
                Directory.CreateDirectory(uploadedPath);
            }
            foreach (HttpPostedFileBase file in files)
            {
                //Checking file is available to save.  
                if (file != null)
                {
                    var InputFileName = Path.GetFileName(file.FileName);
                    var ServerSavePath = Path.Combine(uploadedPath, InputFileName);
                    //Save file to server folder  
                    file.SaveAs(ServerSavePath);
                }
            }
        }
        
        public Result ValidateUploadedFile(HttpPostedFileBase logo, string[] supportedTypes, int sizeLimit)
        {
            int totalSize = 0;
            string fileExt = Path.GetExtension(logo.FileName).Substring(1).ToLower();
            totalSize += logo.ContentLength;
            if (Array.IndexOf(supportedTypes, fileExt) < 0)
            {
                return new Result()
                {
                    IsSuccess = false,
                    Message = "File Extension Is InValid - Only Upload PNG/JPG/JPEG file"
                };
            }
            //Total files size > 5mb
            else if (totalSize > (sizeLimit * 1024 * 1024))
            {
                return new Result()
                {
                    IsSuccess = false,
                    Message = "Total size of files can not exceed " + sizeLimit + "Mb"
                };
            }
            return new Result()
            {
                IsSuccess = true
            };
        }
    }
}