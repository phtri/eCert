using eCert.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCert.Controllers
{
    public class AcademicServiceController : Controller
    {
        private readonly CertificateServices _certificateServices;
        public AcademicServiceController()
        {
            _certificateServices = new CertificateServices();
        }
        // GET: AcademicService
        public ActionResult Index()
        {
            if (Session["RoleName"].ToString() == Utilities.Constants.Role.FPT_UNIVERSITY_ACADEMIC)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }

        public ActionResult LoadAllReport(int pageSize = 8, int pageNumber = 1)
        {
            ViewBag.Pagination = _certificateServices.GetAllReportPagination(pageSize, pageNumber);
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

        public ActionResult DetailReport()
        {
            if (Session["RoleName"].ToString() == Utilities.Constants.Role.FPT_UNIVERSITY_ACADEMIC)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
    }
}