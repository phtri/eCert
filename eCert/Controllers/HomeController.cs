using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Services;
using eCert.Utilities;
using IronPdf;
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
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    Issuer = Constants.CertificateIssuer.PERSONAL,
                    ViewCount = 100,
                    VerifyCode = "XYZ"
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
                        _certificateServices.UploadCertificatesFile(cert.CertificateFile, "HE6666");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        TempData["Msg"] = "Upload failed";
                        return RedirectToAction("Index");
                    }
                }
                //Get certificate contents (To add to the database)
                List<CertificateContents> contents = _certificateServices.GetCertificateContents(cert.Content, cert.CertificateFile);
                //Add certificate & certificate contents to database
                _certificateServices.AddCertificate(addCertificate, contents);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["Msg"] = "Failed, something went wrong";
            }
            TempData["Msg"] = "Add successfully";
            return RedirectToAction("Index");
        }
        public ActionResult GeneratePdf()
        {
            Certificate testCertificate = new Certificate()
            {
                CertificateName = "TestgeneratePDFahihi",
                DateOfIssue = DateTime.Now,
                DateOfExpiry = DateTime.Now,
                ViewCount = 99,
                Issuer = "FPT University",
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
                OrganizationId = 1,
                UserId = 1
            };

            //Generate the information of certificate to PDF format
            string razorString = RenderRazorViewToString("~/Views/Home/CertificatePdfTemplate.cshtml", testCertificate);

            

            _certificateServices.GeneratePdfForCertificate(testCertificate.CertificateName, "HE12345", razorString);
            
            CertificateContents content = new CertificateContents()
            {
                Content = "Ahihi",
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };
            _certificateServices.AddCertificate(testCertificate, new List<CertificateContents>() { content });

            return RedirectToAction("Index");
        }
        [HttpPost]
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
        //public ActionResult EditCertificate(int certId)
        //{
        //    Certificate cert = _certificateDao.GetCertificateByID(certId);
        //    return Json(cert, JsonRequestBehavior.AllowGet);
        //}

        //For testing purpose
        public ActionResult Test()
        {
            Certificate addCertificate = new Certificate()
            {
                OrganizationId = 1,
                UserId = 1,
                CertificateName = "TEST SQL TRANSACTION 2",
                Description = "THIS IS A LONG DESCRIPTION 2",
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
                Issuer = Constants.CertificateIssuer.PERSONAL,
                ViewCount = 100,
                VerifyCode = "XYZ",
                DateOfIssue = DateTime.Now,
                DateOfExpiry = DateTime.Now
            };
            List<CertificateContents> list = new List<CertificateContents>()
            {
                new CertificateContents(){Content = "Test mung 4 tet 1", created_at = DateTime.Now, updated_at = DateTime.Now, Format = Constants.CertificateFormat.PDF},
                new CertificateContents(){Content = "Test mung 4 tet 2", created_at = DateTime.Now, updated_at = DateTime.Now, Format = Constants.CertificateFormat.PNG},
                new CertificateContents(){Content = "Test mung 4 tet 3", created_at = DateTime.Now, updated_at = DateTime.Now, Format = Constants.CertificateFormat.LINK},
                new CertificateContents(){Content = "Test mung 4 tet 4", created_at = DateTime.Now, updated_at = DateTime.Now, Format = Constants.CertificateFormat.JPEG}
            };
            _certificateServices.AddCertificate(addCertificate, list);
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int certificateId)
        {
            Certificate cert = _certificateServices.GetDetail(certificateId);
            return View(cert);
        }

    }
}

