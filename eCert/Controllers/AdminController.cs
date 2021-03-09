﻿using eCert.Models.ViewModel;
using eCert.Services;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.Mvc;

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
            return View();
        }

        public ActionResult ImportExcel()
        {
            return View();
        }
        public ActionResult ImportDiploma()
        {
            return View();
        }
        public ActionResult ListAcademicService()
        {
            return View();
        }
        public ActionResult CreateAccountAcademicService()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccountAcademicService(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                //return RedirectToAction("ListAcademicService", "Admin");
            }
            return RedirectToAction("Index", "Admin");

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
                    _adminServices.ImportCertificatesByExcel(importExcelFile.File, Server.MapPath("~/Uploads/"));
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