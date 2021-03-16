﻿using eCert.Models.Entity;
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
                if ((bool)Session["isUpdatedEmail"])
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
                if ((bool)Session["isUpdatedEmail"])
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
            return PartialView();
        }

        public ActionResult AddReport(int certId)
        {
            if (Session["RollNumber"] != null)
            {
                if ((bool)Session["isUpdatedEmail"])
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
            int userId = 1;
            string rollNumber = Session["RollNumber"].ToString();
            //Get all certiificates of a user
            
            
            ViewBag.Pagination = _certificateServices.GetCertificatesPagination(rollNumber, pageSize, pageNumber, keyword);
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
                    Url = Guid.NewGuid().ToString(),
                    User = new User()
                    {
                        UserId = 1
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
                        _certificateServices.UploadCertificatesFile(cert.CertificateFile, "HE9876", addCertificate.Url);
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

        
        
        public ActionResult FPTCertificateDetail(int certId)
        {
            ViewBag.Title = "FPT Education Certificate Detail";
            CertificateViewModel certViewModel = _certificateServices.GetCertificateDetail(certId);

            //Doesn't have pdf
            if(certViewModel.CertificateContents.Count == 0)
            {
                //Trí code
                string razorString = RenderRazorViewToString("~/Views/Shared/Certificate.cshtml", certViewModel);
                _certificateServices.GeneratePdfFuCert(certViewModel, razorString);
                //Trí code tiếp

               
            }
            //Có file
            certViewModel = _certificateServices.GetCertificateDetail(certId);
            return View(certViewModel);
        }
        public ActionResult PersonalCertificateDetail(int certId)
        {
            if (Session["RollNumber"] != null)
            {
                if ((bool)Session["isUpdatedEmail"])
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
                if ((bool)Session["isUpdatedEmail"])
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
        public ActionResult Test(string m)
        {
            UserDAO dao = new UserDAO();
            User x = new User()
            {
                PasswordHash = "ABC",
                PasswordSalt = "XYZ"
            };

            UserViewModel y = new UserViewModel()
            {
                AcademicEmail = "A@Gmail.com"
            };
            x = AutoMapper.Mapper.Map<UserViewModel, User>(y);
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