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

        
    }
}