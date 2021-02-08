using eCert.Daos;
using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Services;
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
        private string errorMessage = "";
        private readonly CertificateDAO _certificateDao;
        private readonly CertificateServices _certificateServices;
        public HomeController()
        {
            _certificateDao = new CertificateDAO();
            _certificateServices = new CertificateServices();
        }
        public ActionResult Index(string mesage, int pageSize = 5, int pageNumber = 1)
        {

            int userId = 4;
            //Get all certiificates of a user
            ViewBag.Pagination = _certificateServices.GetCertificatesPagination(userId, pageSize, pageNumber);
            ViewBag.message = mesage;

            return View();
        }
        [HttpPost]
        public ActionResult Delete(int certId)
        {
            _certificateDao.DeleteCertificate(certId);
            TempData["Msg"] = "Delete certificate successfully";

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult AddCertificate(CertificateViewModel cert)
        {
            try
            {
                
                if (cert.CertificateName == null)
                {
                    TempData["Msg"] = "The certificate name is required.";
                    return RedirectToAction("Index");

                }
                if (cert.Description == null)
                {
                    TempData["Msg"] = "The description is required.";
                    return RedirectToAction("Index");

                }
                //case cert is link
                if (cert.CertificateFile == null)
                {
                    if (cert.Content == null)
                    {
                        TempData["Msg"] = "The certificate link is required.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //Add certificate (with link) to database
                        _certificateDao.CreateACertificate(new Certificate() { OrganizationId = 1, UserId = 18, CertificateName = cert.CertificateName, Description = cert.Description, created_at = DateTime.Now, updated_at = DateTime.Now, Issuer = Constants.CertificateType.PERSONAL, Format = Constants.CertificateFormat.LINK, ViewCount = 100, VerifyCode = "XYZ" });
                    }

                    //Certificate file
                }
                else
                {
                    bool result = validateUploadFile(cert.CertificateFile);
                    //If check file success
                    if (result)
                    {
                        try
                        {
                            uploadFile(cert.CertificateFile);
                            //_certificateDao.CreateACertificate(new Certificate() { OrganizationId = 1, UserId = 18, CertificateName = cert.CertificateName, Description = cert.Description, Content = Path.GetFileName(cert.CertificateFile.FileName), created_at = DateTime.Now, updated_at = DateTime.Now, Format = Path.GetExtension(cert.CertificateFile.FileName).Substring(1).ToUpper(), Type = Constants.CertificateType.PERSONAL });
                        }
                        catch
                        {
                            TempData["Msg"] = "Upload file failed";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Msg"] = errorMessage;
                        return RedirectToAction("Index");
                    }
                }
                TempData["Msg"] = "Added Successfully.";
                return RedirectToAction("Index");
            }catch(System.Data.SqlClient.SqlException e)
            {
                TempData["Msg"] = "Something went wrong.";
                return RedirectToAction("Index");
            }
            
        }

        public void DownloadCertificate(int certificateId)
        {
            string fileName = _certificateDao.GetCertificateContent(certificateId);


            FileInfo file = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/UploadedFiles/" + fileName);
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "text/plain";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();

        }

        private bool validateUploadFile(HttpPostedFileBase file)
        {
            int limitFileSize = 20;

            try
            {
                string[] supportedTypes = { "pdf", "jpg", "jpeg", "png" };
                string fileExt = Path.GetExtension(file.FileName).Substring(1);

                if (Array.IndexOf(supportedTypes, fileExt) < 0)
                {
                    errorMessage = "File Extension Is InValid - Only Upload PDF/PNG/JPG/JPEG File";
                    return false;
                }
                else if (file.ContentLength > (limitFileSize * 1024 * 1024))
                {
                    errorMessage = "File size Should Be UpTo " + limitFileSize + "MB";
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
                errorMessage = "Something went wrong";
                return false;
            }
        }
        private void uploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    //Hard code: Add to desktop
                    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/UploadedFiles";
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
                throw new Exception();
            }
        }


        //public ActionResult EditCertificate(int certId)
        //{
        //    Certificate cert = _certificateDao.GetCertificateByID(certId);
        //    return Json(cert, JsonRequestBehavior.AllowGet);
        //}

        //For testing purpose
        public ActionResult Test()
        {
            _certificateDao.Test();
            return RedirectToAction("Index");
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