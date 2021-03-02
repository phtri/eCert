using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Services;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
namespace eCert.Controllers
{
    public class HomeController : Controller
    {
        private readonly CertificateServices _certificateServices;
        private readonly FileServices _fileServices;
        public HomeController()
        {
            _certificateServices = new CertificateServices();
            _fileServices = new FileServices();
        }
        public ActionResult Index(string mesage, int pageSize = 5, int pageNumber = 1)
        {
            int userId = 1;
            //Get all certiificates of a user
            ViewBag.Pagination = _certificateServices.GetCertificatesPagination(userId, pageSize, pageNumber);
            ViewBag.message = mesage;
            return View();
        }
        public ActionResult LoadListOfCert(string mesage, int pageSize = 5, int pageNumber = 1)
        {
            int userId = 1;
            //Get all certiificates of a user
            ViewBag.Pagination = _certificateServices.GetCertificatesPagination(userId, pageSize, pageNumber);
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddCertificate(CertificateViewModel cert)
        {
            try
            {
                Result certificateInforValidate = _certificateServices.ValidateCertificateInfor(cert);
                if (certificateInforValidate.IsSuccess == false)
                {
                    TempData["Msg"] = certificateInforValidate.Message;
                    return RedirectToAction("Index");
                }
                Certificate addCertificate = new Certificate()
                {
                    OrganizationId = 1,
                    UserId = 1,
                    CertificateName = cert.CertificateName,
                    Description = cert.Description,
                    Issuer = Constants.CertificateIssuer.PERSONAL,
                    ViewCount = 100,
                    VerifyCode = Guid.NewGuid().ToString(),
                    
                };
                //Check certificate file
                if (cert.CertificateFile != null && cert.CertificateFile[0] != null)
                {
                    Result certificateFileValidate = _certificateServices.ValidateCertificateFiles(cert.CertificateFile);
                    if (certificateFileValidate.IsSuccess == false)
                    {
                        TempData["Msg"] = certificateInforValidate.Message;
                        return RedirectToAction("Index");
                    }
                    //Try to upload file
                    try
                    {
                        _certificateServices.UploadCertificatesFile(cert.CertificateFile, "HE9999", addCertificate.VerifyCode);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        TempData["Msg"] = "Upload failed";
                        return RedirectToAction("Index");
                    }
                }
                //Get certificate contents (To add to the database)
                addCertificate.CertificateContents = _certificateServices.GetCertificateContents(cert.Content, cert.CertificateFile, "HE9999", addCertificate.VerifyCode);

                //Add certificate & certificate contents to database
                _certificateServices.AddCertificate(addCertificate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["Msg"] = "Failed, something went wrong";
            }
            TempData["Msg"] = "Add successfully";
            return RedirectToAction("Index");
        }
      
        public ActionResult Delete(int certId)
        {
            _certificateServices.DeleteCertificate(certId);
            TempData["Msg"] = "Delete certificate successfully";
            return RedirectToAction("Index");
        }
        public void DownloadCertificate(int certificateId)
        {
            //string fileName = _certificateDao.GetCertificateContent(certificateId);


            //FileInfo file = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/UploadedFiles/" + fileName);
            //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            //Response.Clear();
            //Response.ClearHeaders();
            //Response.ClearContent();
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            //Response.AddHeader("Content-Length", file.Length.ToString());
            //Response.ContentType = "text/plain";
            //Response.Flush();
            //Response.TransmitFile(file.FullName);
            //Response.End();
        }

        //public ActionResult EditCertificate(int certId)
        //{
        //    Certificate cert = _certificateDao.GetCertificateByID(certId);
        //    return Json(cert, JsonRequestBehavior.AllowGet);
        //}

        //For testing purpose
        public ActionResult Test()
        {

            return View("~/Views/Shared/Certificate.cshtml");
        }

        public ActionResult FPTCertificateDetail(int certId)
        {
            
            return View();
        }

        public ActionResult PersonalCertificateDetail(int certId)
        {
            CertificateViewModel certViewModel = _certificateServices.GetCertificateById(certId);

            return View(certViewModel);
        }

        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult EditCertificate(int certId)
        {
            CertificateViewModel certViewModel = _certificateServices.GetCertificateById(certId);
            return View(certViewModel);
        }

        [HttpPost]
        public ActionResult EditCertificate(CertificateViewModel model)
        {
            //_certificateServices.UpdateCertificate(model);
            return View();

        }

       
    }
}

