using eCert.Daos;
using eCert.Models.Entity;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace eCert.Services
{
    public class CertificateContentServices
    {
        private readonly CertificateContentDAO _certificateContentDAO;

        public CertificateContentServices()
        {
            _certificateContentDAO = new CertificateContentDAO(); 
        }

        public int CreateACertificateContent(CertificateContents certificateContent)
        {
            return _certificateContentDAO.CreateACertificateContent(certificateContent);
        }
        public string GetFileExtension(HttpPostedFileBase file)
        {
            if(Path.GetExtension(file.FileName).Substring(1) == "pdf")
            {
                return Constants.CertificateFormat.PDF;
            }else if(Path.GetExtension(file.FileName).Substring(1) == "png")
            {
                return Constants.CertificateFormat.PNG;
            }else if(Path.GetExtension(file.FileName).Substring(1) == "jpg")
            {
                return Constants.CertificateFormat.JPG;
            }else if(Path.GetExtension(file.FileName).Substring(1) == "jpeg")
            {
                return Constants.CertificateFormat.JPEG;
            }
            return null;
        }

    }
}