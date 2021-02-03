using eCert.Daos;
using eCert.Models;
using System.Collections.Generic;
using System.Web.Mvc;
namespace eCert.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int userId = 18;
            //Get all certiificates of a user
            CertificateDAO certificateDAO = new CertificateDAO();
            List<Certificate> certificates = certificateDAO.GetCertificatesOfUser(userId);



            return View();
        }

        [HttpPost]
        public void AddCertificate(Certificate cert)
        {
            //CertificateDAO temp= new CertificateDAO();
            //temp.CreateCertificate(new Certificate() { OrganizationId = 1, UserId = 18, created_at = DateTime.Now, updated_at = DateTime.Now});
            Certificate temp = cert;
            if(cert.CertificateFile == null)
            {
                ViewBag.Message = "khong co file, nhap link";
            }
            else
            {
                uploadFile(cert.CertificateFile);
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