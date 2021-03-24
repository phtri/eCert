using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.Mvc;
using static eCert.Utilities.Constants;

namespace eCert.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminServices _adminServices;
        private readonly UserServices _userServices;
        public AdminController()
        {
            _adminServices = new AdminServices();
            _userServices = new UserServices();
        }

        // GET: Admin
        public ActionResult Index()
        {
            int currentRole = 0;
            if(Session["RoleId"] != null)
            {
                currentRole = Int32.Parse(Session["RoleId"].ToString());
            }
            if (currentRole  == Utilities.Constants.Role.ADMIN | currentRole == Utilities.Constants.Role.SUPER_ADMIN)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
           
        }

        public ActionResult ImportExcel()
        {

            int currentRole = 0;
            if (Session["RoleId"] != null)
            {
                currentRole = Int32.Parse(Session["RoleId"].ToString());
            }
            if (currentRole == Utilities.Constants.Role.ADMIN)
            {
               
                return View();
            }else if(currentRole == Utilities.Constants.Role.SUPER_ADMIN)
            {
                List<EducationSystemViewModel> listEduSystem = _adminServices.GetAllEducatinSystem();
                ViewBag.ListEducationSystem = listEduSystem;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
        public ActionResult ImportDiploma()
        {
            int currentRole = 0;
            if (Session["RoleId"] != null)
            {
                currentRole = Int32.Parse(Session["RoleId"].ToString());
            }
            if (currentRole == Utilities.Constants.Role.ADMIN | currentRole == Utilities.Constants.Role.SUPER_ADMIN)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
        public ActionResult ListAcademicService()
        {
            int currentRole = 0;
            if (Session["RoleId"] != null)
            {
                currentRole = Int32.Parse(Session["RoleId"].ToString());
            }
            if (currentRole == Utilities.Constants.Role.ADMIN | currentRole == Utilities.Constants.Role.SUPER_ADMIN)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }

        public ActionResult LoadListOfAcademicService(int pageSize = 5, int pageNumber = 1)
        {
            //get list user academic service
            ViewBag.Pagination = _adminServices.GetAcademicServicePagination(pageSize, pageNumber);
            return PartialView();
        }

        public void DeleteAcademicService(int userId)
        {
            _userServices.DeleteUser(userId);
            TempData["Msg"] = "Delete user successfully";
        }
        public ActionResult CreateAccountAcademicService()
        {
            int currentRole = 0;
            if (Session["RoleId"] != null)
            {
                currentRole = Int32.Parse(Session["RoleId"].ToString());
            }
            if (currentRole == Utilities.Constants.Role.ADMIN | currentRole == Utilities.Constants.Role.SUPER_ADMIN)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
      
        [HttpPost]
        public ActionResult CreateAccountAcademicService(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                //check exist email in DB
                UserViewModel user = _userServices.GetUserByAcademicEmail(userViewModel.AcademicEmail);
                //case email existed in DB
                if(user != null)
                {
                    ModelState.AddModelError("ErrorMessage", "Invalid. This email has been existed.");
                    return View();
                }
                else
                {
                    User addAcademicService = new User()
                    {
                        PhoneNumber = userViewModel.PhoneNumber,
                        AcademicEmail = userViewModel.AcademicEmail
                    };
                    _adminServices.AddAcademicSerivce(addAcademicService);

                    //send email
                    return RedirectToAction("ListAcademicService", "Admin");
                }
                
            }
            else
            {
                return View();
            }

        }
        

        [HttpPost]
        public ActionResult ImportExcel(ImportExcel importExcelFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int countResult = 0;
                    countResult = _adminServices.ImportCertificatesByExcel(importExcelFile.File, Server.MapPath("~/Uploads/"), TypeImportExcel.IMPORT_CERT, importExcelFile.CampusId);
                    ViewBag.MessageSuccess = countResult + " rows are imported succesfully";
                    
                }
            }
            catch
            {
                ViewBag.MessageError = "File is not valid";
            }
           
            return View();
        }

        [HttpPost]
        public ActionResult ImportDiploma(ImportExcel importExcelFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _adminServices.ImportCertificatesByExcel(importExcelFile.File, Server.MapPath("~/Uploads/"), TypeImportExcel.IMPORT_DIPLOMA, importExcelFile.CampusId);
                }
            }
            catch
            {
                ViewBag.MessageError = "File is not valid";
            }

            return View();
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

        public void DownloadTemplateCertificate()
        {
            DownloadTemplate(TypeImportExcel.IMPORT_CERT);
        }
        public void DownloadTemplateDiploma()
        {
            DownloadTemplate(TypeImportExcel.IMPORT_DIPLOMA);
        }

        public void DownloadTemplate(int type)
        {
            string fileLocation = String.Empty;
            if (type == TypeImportExcel.IMPORT_CERT)
            {
                fileLocation = SaveCertificateLocation.BaseTemplateFileCert;
            }else if(type == TypeImportExcel.IMPORT_DIPLOMA)
            {
                fileLocation = SaveCertificateLocation.BaseTemplateFileDiploma;
            }
            FileInfo file = new FileInfo(fileLocation);
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();
        }

    }
}