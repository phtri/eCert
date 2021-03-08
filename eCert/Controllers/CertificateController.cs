using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Services;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using IronPdf;
using System.Text;
using eCert.Daos;

namespace eCert.Controllers
{
    public class CertificateController : Controller
    {
        private readonly CertificateServices _certificateServices;
        private readonly FileServices _fileServices;

        public CertificateController()
        {
            _certificateServices = new CertificateServices();
            _fileServices = new FileServices();
        }
        public ActionResult Index(string mesage, int pageSize = 5, int pageNumber = 1)
        {
            ViewBag.Title = "Home";
            return View();
        }
        public ActionResult LoadListOfCert(string mesage, int pageSize = 5, int pageNumber = 1)
        {
            int userId = 1;
            string rollNumber = "HE9876";
            //Get all certiificates of a user
            ViewBag.Pagination = _certificateServices.GetCertificatesPagination(rollNumber, pageSize, pageNumber);
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
                    CertificateName = cert.CertificateName,
                    Description = cert.Description,
                    Issuer = Constants.CertificateIssuer.PERSONAL,
                    ViewCount = 100,
                    VerifyCode = Guid.NewGuid().ToString(),
                    User = new User()
                    {
                        RollNumber = "HE9876"
                    }
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
                        _certificateServices.UploadCertificatesFile(cert.CertificateFile, "HE9876", addCertificate.VerifyCode);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        TempData["Msg"] = "Upload failed";
                        return RedirectToAction("Index");
                    }
                }
                //Get certificate contents (To add to the database)
                addCertificate.CertificateContents = _certificateServices.GetCertificateContents(cert.Content, cert.CertificateFile, "HE9876", addCertificate.VerifyCode);

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
        [HttpPost]
        public ActionResult Delete(int certId)
        {
            _certificateServices.DeleteCertificate(certId);
            TempData["Msg"] = "Delete certificate successfully";
            return RedirectToAction("Index");
        }
        public void DownloadPersonalCertificate(int certId)
        {
            string fileLocation = _certificateServices.DownloadPersonalCertificate(certId);
            
            FileInfo file = new FileInfo(fileLocation);
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
            
            //Remove temp file after download
            if (System.IO.File.Exists(fileLocation))
            {
                System.IO.File.Delete(fileLocation);
            }
        }
        //public ActionResult EditCertificate(int certId)
        //{
        //    Certificate cert = _certificateDao.GetCertificateByID(certId);
        //    return Json(cert, JsonRequestBehavior.AllowGet);
        //}

        
        //For testing purpose
        public ActionResult FPTCertificateDetail(int certId)
        {
            ViewBag.Title = "FU Education Certificate Detail";
            CertificateViewModel certViewModel = _certificateServices.GetCertificateDetail(certId);
            if(certViewModel.CertificateContents.Count == 0)
            {
                string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\cert.pdf";
                string razorString = RenderRazorViewToString("~/Views/Shared/Certificate.cshtml", certViewModel);
                var Renderer = new IronPdf.HtmlToPdf();
                Renderer.PrintOptions.CssMediaType = IronPdf.PdfPrintOptions.PdfCssMediaType.Print;
                Renderer.PrintOptions.PaperSize = IronPdf.PdfPrintOptions.PdfPaperSize.A4;
                Renderer.PrintOptions.PaperOrientation = PdfPrintOptions.PdfPaperOrientation.Landscape;
                Renderer.PrintOptions.Title = "My PDF Document Name";
                Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Screen;
                Renderer.PrintOptions.DPI = 300;
                Renderer.PrintOptions.FitToPaperWidth = true;
                Renderer.PrintOptions.JpegQuality = 100;
                Renderer.PrintOptions.GrayScale = false;
                Renderer.PrintOptions.FitToPaperWidth = true;
                Renderer.PrintOptions.InputEncoding = Encoding.UTF8;
                Renderer.PrintOptions.Zoom = 100;
                Renderer.PrintOptions.MarginTop = 0;  //millimeters
                Renderer.PrintOptions.MarginLeft = 0;  //millimeters
                Renderer.PrintOptions.MarginRight = 0;  //millimeters
                Renderer.PrintOptions.MarginBottom = 0;  //millimeters
                Renderer.PrintOptions.CreatePdfFormsFromHtml = true;


                var PDF = Renderer.RenderHtmlAsPdf(razorString);
                
                PDF.SaveAs(savePath);
            }
            return View();
        }
        public ActionResult PersonalCertificateDetail(int certId)
        {
            ViewBag.Title = "Personal Certificate Detail";
            CertificateViewModel certViewModel = _certificateServices.GetCertificateDetail(certId);
            return View(certViewModel);
        }
        
        public ActionResult EditCertificate(int certId)
        {
            CertificateViewModel certViewModel = _certificateServices.GetCertificateDetail(certId);
            return View(certViewModel);
        }
        [HttpPost]
        public ActionResult EditCertificate(CertificateViewModel model)
        {
            //_certificateServices.UpdateCertificate(model);
            return View();
        }
        public ActionResult Test(string m)
        {
            UserDAO dao = new UserDAO();
            dao.GetUserByAcademicEmail(m);
            return View("~/Views/Shared/Certificate.cshtml");
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
    }
}