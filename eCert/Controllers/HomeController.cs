using eCert.Daos;
using eCert.Models;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
namespace eCert.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int pageSize = 5, int pageNumber = 1)
        {

            int userId = 18;
            //Get all certiificates of a user
            CertificateDAO certificateDAO = new CertificateDAO();

            ViewBag.Pagination = certificateDAO.GetCertificatesPagination(userId, pageSize, pageNumber);

            return View();
        }

        [HttpPost]
        public ActionResult AddCertificate(Certificate cert)
        {
            CertificateDAO certificateDAO = new CertificateDAO();
            if(cert.CertificateName == null)
            {
                //ViewBag.message = "chua co ten";
                return View("~/Views/Home/Index.cshtml");
            }

            //case cert is link
            if (cert.CertificateFile == null)
            {
                return View("~/Views/Home/Index.cshtml");
                //certificateDAO.CreateACertificate(new Certificate() { OrganizationId = 1, UserId = 18, CertificateName = cert.CertificateName, Description = cert.Description, Content = cert.Content ,created_at = DateTime.Now, updated_at = DateTime.Now });

            }
            //case cert is file
            else
            {
                //bool result = validateUploadFile(cert.CertificateFile);
                //uploadFile(cert.CertificateFile);
                //certificateDAO.CreateACertificate(new Certificate() { OrganizationId = 1, UserId = 18, CertificateName = cert.CertificateName, Description = cert.Description, Content = Path.GetFileName(cert.CertificateFile.FileName), created_at = DateTime.Now, updated_at = DateTime.Now });
                return View("~/Views/Home/Index.cshtml");
            }
        }
      
        private bool validateUploadFile(HttpPostedFileBase file)
        {
            int limitFileSize = 20;
            
            try
            {
                string[] supportedTypes =  { "pdf", "jpg", "jpeg", "png" };
                string fileExt = Path.GetExtension(file.FileName).Substring(1);
               
                if (Array.IndexOf(supportedTypes, fileExt) < 0)
                {
                    //errorMessage = "File Extension Is InValid - Only Upload PDF/PNG/JPG/JPEG File";
                    return false;
                }
                else if (file.ContentLength > (limitFileSize * 1024 * 1024))
                {
                    //errorMessage = "File size Should Be UpTo " + limitFileSize + "KB";
                    return false;
                }
                else
                {
                    //errorMessage = "File Is Successfully Uploaded";
                    return true;
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Upload Container Should Not Be Empty or Contact Admin";
                return false;
            }
        }
        private void uploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string folderPath = Server.MapPath("~/UploadedFiles");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(folderPath, _FileName);
                    file.SaveAs(_path);
                }
            }
            catch
            {

            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}

//DataProvider provider = new DataProvider();
//DataTable table = provider.GET_LIST_OBJECT("select * from [dbo].[Organizations]", new object[] { });
//List<string> listString = new List<string>();

//foreach (DataRow dataRow in table.Rows)
//{
//    listString.Add(dataRow["OrganizationId"].ToString() + dataRow["OrganizationName"].ToString() + dataRow["LogoImage"].ToString());
//}

//CertificateDAO dao = new CertificateDAO();
//dao.CreateCertificate(new Certificate() { CertificateName = "ABC", Content = "ABC", Description = "ABC", FileName = "ABC" , Format = "ABC", Hashing = "ABC", created_at = DateTime.Now, updated_at = DateTime.Now, UserId = 18, OrganizationId = 1, Type = "ABC", VerifyCode = "ABC" });