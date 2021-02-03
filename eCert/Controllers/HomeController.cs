using eCert.Daos;
using System;

using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCert.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DataProvider provider = new DataProvider();
            DataTable table = provider.GET_LIST_OBJECT("select * from [dbo].[Organizations]", new object[] { });
            List<string> listString = new List<string>();

            foreach (DataRow dataRow in table.Rows)
            {
                listString.Add(dataRow["OrganizationId"].ToString() + dataRow["OrganizationName"].ToString() + dataRow["LogoImage"].ToString());
            }

            return View();
        }

        [HttpPost]
        public void UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    //string storePath = "C:\\Users\\BachHV\\Desktop\\CertificateFile";
                    string folderPath = Server.MapPath("~/UploadedFiles");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string _FileName = Path.GetFileName(file.FileName);
                    //string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    string _path = Path.Combine(folderPath, _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "File Uploaded Successfully!!";
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
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