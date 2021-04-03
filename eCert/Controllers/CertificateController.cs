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
using System.Web;
using static eCert.Utilities.Constants;

namespace eCert.Controllers
{
    public class CertificateController : Controller
    {
        private readonly CertificateServices _certificateServices;
        private readonly FileServices _fileServices;
        private readonly UserServices _userServices;

        public CertificateController()
        {
            _certificateServices = new CertificateServices();
            _fileServices = new FileServices();
            _userServices = new UserServices();
        }
        public ActionResult Index(string mesage, int pageSize = 5, int pageNumber = 1)
        {
            if (Session["RollNumber"] != null)
            {
                if (!String.IsNullOrEmpty(Session["isUpdatedEmail"].ToString()) && (bool)Session["isUpdatedEmail"])
                {
                    ViewBag.Title = "Certificate";
                    return View();
                }
                else
                {
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                }
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
        public ActionResult ListReport()
        {
            if (Session["RollNumber"] != null)
            {
                if (!String.IsNullOrEmpty(Session["isUpdatedEmail"].ToString()) && (bool)Session["isUpdatedEmail"])
                {
                    return View();
                }
                else
                {
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                }      
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }

        public ActionResult LoadListOfReport(int pageSize = 5, int pageNumber = 1)
        {
            UserViewModel userViewModel = _userServices.GetUserByRollNumber(Session["RollNumber"].ToString());
            ViewBag.Pagination = _certificateServices.GetReportPagination(userViewModel.UserId, pageSize, pageNumber);
            if (ViewBag.Pagination.PagingData.Count == 0)
            {
                ViewBag.OverflowHidden = "overflow-hidden";
            }
            else
            {
                ViewBag.OverflowHidden = String.Empty;
            }
            return PartialView();
        }

        public ActionResult AddReport(int certId)
        {
            if (Session["RollNumber"] != null)
            {
                if (!String.IsNullOrEmpty(Session["isUpdatedEmail"].ToString()) && (bool)Session["isUpdatedEmail"])
                {
                    CertificateViewModel certViewModel = _certificateServices.GetCertificateDetail(certId);
                    ReportViewModel reportViewModel = new ReportViewModel()
                    {
                        CertificateName = certViewModel.CertificateName,
                        CertificateId = certViewModel.CertificateId
                    };
                    return View(reportViewModel);
                }
                else
                {
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                }
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
        [HttpPost]
        public ActionResult AddReport(ReportViewModel reportViewModel)
        {
            try
            {
                string rollNumber = Session["RollNumber"].ToString();
                _certificateServices.AddReport(reportViewModel, rollNumber);
                //send email to DVSV

                ViewBag.isSent = true;
                ViewBag.Message = "Sent report successfully.";
            }catch(Exception e)
            {
                ViewBag.isSent = false;
                ViewBag.Message = "Sent report error.";
            }
            //TempData["Message"] = "Add REPORT successfully";
            return View();
        }
        public ActionResult LoadListOfCert(string mesage, int pageSize = 5, int pageNumber = 1, string keyword = "")
        {
            
            string rollNumber = Session["RollNumber"].ToString();
            //Get all certiificates of a user
            ViewBag.Pagination = _certificateServices.GetCertificatesPagination(rollNumber, pageSize, pageNumber, keyword);
            if (ViewBag.Pagination.PagingData.Count == 0)
            {
                ViewBag.OverflowHidden = "overflow-hidden";
            }
            else
            {
                ViewBag.OverflowHidden = String.Empty;
            }
            if (ViewBag.Pagination.PagingData.Count != 0)
            {
                return PartialView();
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult AddCertificate(CertificateViewModel certViewModel)
        {
            try
            {
                Result certificateInforValidate = _certificateServices.ValidateCertificateInfor(certViewModel);
                if (certificateInforValidate.IsSuccess == false)
                {
                    TempData["Msg"] = certificateInforValidate.Message;
                    return RedirectToAction("Index");
                }
                certViewModel.RollNumber = Session["RollNumber"].ToString();
                certViewModel.Url = Guid.NewGuid().ToString();
                
                //Check certificate file
                if (certViewModel.CertificateFile != null && certViewModel.CertificateFile[0] != null)
                {
                    Result certificateFileValidate = _certificateServices.ValidateCertificateFiles(certViewModel.CertificateFile);
                    if (certificateFileValidate.IsSuccess == false)
                    {
                        TempData["Msg"] = certificateFileValidate.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //Try to upload file
                        try
                        {
                            _certificateServices.UploadCertificatesFile(certViewModel);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            TempData["Msg"] = "Upload failed";
                            return RedirectToAction("Index");
                        }
                    }
                }
                //Get certificate links
                _certificateServices.AddCertificateLinks(certViewModel);
                //Add certificate & certificate contents to database
                _certificateServices.AddCertificate(certViewModel, CertificateIssuer.PERSONAL);
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
            return RedirectToAction("Index", "Certificate");
        }
        public void DownloadPersonalCertificate(int certId)
        {
            string rollNumber = Session["RollNumber"].ToString();
            string fileLocation = _certificateServices.DownloadPersonalCertificate(certId, rollNumber);
            FileInfo file = new FileInfo(fileLocation);
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "application/x-zip-compressed";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();
            //Remove temp file after download
            if (System.IO.File.Exists(fileLocation))
            {
                System.IO.File.Delete(fileLocation);
            }
        }
        public void DownloadFptCertificate(string url, string type)
        {
            string fileLocation = _certificateServices.DownloadFPTCertificate(url, type);
            FileInfo file = new FileInfo(fileLocation);
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            if(type == CertificateFormat.PDF)
            {
                Response.ContentType = "application/pdf";
            }else if(type == CertificateFormat.PNG)
            {
                Response.ContentType = "image/png";
            }
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();
        }
        public ActionResult DownloadSearchedCertificate(string keyword)
        {
            string rollNumber = Session["RollNumber"].ToString();
            List<CertificateViewModel> certificates = _certificateServices.GetAllCertificatesByKeyword(rollNumber, keyword);
            if(certificates.Count == 0 || keyword == null)
            {
                return RedirectToAction("Index");
            }
            //Fpt certificates
            List<CertificateViewModel> fptCertificates = certificates.Where(x => x.IssuerType == CertificateIssuer.FPT).ToList();
            if(fptCertificates != null && fptCertificates.Count > 0)
            {
                //Generate file for FPT Certificate
                foreach (CertificateViewModel certificateViewModel in fptCertificates)
                {
                    if(certificateViewModel.CertificateContents.Count == 0)
                    {
                        string razorString = RenderRazorViewToString("~/Views/Shared/Certificate.cshtml", certificateViewModel);
                        _certificateServices.GeneratePdfFuCert(certificateViewModel, razorString);
                    }
                }
            }
            string fileLocation = _certificateServices.DownloadSearchedCertificate(rollNumber, keyword);
            FileInfo file = new FileInfo(fileLocation);
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "application/x-zip-compressed";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();

            return View();
            
        }
        public ActionResult FPTCertificateDetail(string url)
        {
            ViewBag.Title = "FPT Education Certificate Detail";
            CertificateViewModel certViewModel = _certificateServices.GetCertificateByUrl(url);

            //Doesn't have pdf -> Generate 
            if(certViewModel.CertificateContents.Count == 0)
            {
                string razorString = RenderRazorViewToString("~/Views/Shared/Certificate.cshtml", certViewModel);
                _certificateServices.GeneratePdfFuCert(certViewModel, razorString);
            }
            //Có file
            certViewModel = _certificateServices.GetCertificateByUrl(url);
            return View(certViewModel);
        }
        public ActionResult PersonalCertificateDetail(int certId)
        {
            if (Session["RollNumber"] != null)
            {
                if (!String.IsNullOrEmpty(Session["isUpdatedEmail"].ToString()) && (bool)Session["isUpdatedEmail"])
                {
                    ViewBag.Title = "Personal Certificate Detail";
                    CertificateViewModel certViewModel = _certificateServices.GetCertificateDetail(certId);
                    return View(certViewModel);
                }
                else
                {
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                }
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
           
        }
        
        public ActionResult EditCertificate(int certId)
        {
            if (Session["RollNumber"] != null)
            {
                if (!String.IsNullOrEmpty(Session["isUpdatedEmail"].ToString()) && (bool)Session["isUpdatedEmail"])
                {
                    CertificateViewModel certViewModel = _certificateServices.GetCertificateDetail(certId);
                    return View(certViewModel);
                }
                else
                {
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                }
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
            
        }
        [HttpPost]
        public ActionResult EditCertificate(CertificateViewModel model)
        {
            //_certificateServices.UpdateCertificate(model);
            return View();
        }
        public void Test(string m)
        {
            
        }
        public string RenderRazorViewToString(string viewName, object model)
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

        public ActionResult C()
        {
            return View("~/Views/Shared/Certificate.cshtml");
        }
    }
}