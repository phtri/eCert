using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Services;
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
        public AdminController()
        {
            _adminServices = new AdminServices();
        }

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["RollNumber"] != null)
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
            if (Session["RollNumber"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
        public ActionResult ImportDiploma()
        {
            if (Session["RollNumber"] != null)
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
            if (Session["RollNumber"] != null)
            {
                //get list user academic service
                ViewBag.Pagination = _adminServices.GetAcademicServicePagination(5, 1);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
        public ActionResult CreateAccountAcademicService()
        {
            if (Session["RollNumber"] != null)
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
                User addAcademicService = new User()
                {
                    PhoneNumber = userViewModel.PhoneNumber,
                    AcademicEmail = userViewModel.AcademicEmail
                };
                _adminServices.AddAcademicSerivce(addAcademicService);

                //send email
                return RedirectToAction("ListAcademicService", "Admin");
            }
            else
            {
                return View();
            }

        }
        public ActionResult LoadListOfAcademicService(int pageSize = 5, int pageNumber = 1)
        {
            //Get all certiificates of a user
            //ViewBag.Pagination = _adminServices.GetAcademicServicePagination(pageSize, pageNumber);
            return PartialView();
        }

        [HttpPost]
        public ActionResult ImportExcel(ImportExcel importExcelFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _adminServices.ImportCertificatesByExcel(importExcelFile.File, Server.MapPath("~/Uploads/"), TypeImportExcel.IMPORT_CERT);
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
                    _adminServices.ImportCertificatesByExcel(importExcelFile.File, Server.MapPath("~/Uploads/"), TypeImportExcel.IMPORT_DIPLOMA);
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

    }
}