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
            if (Int32.Parse(Session["RoleId"].ToString()) == Utilities.Constants.Role.FPT_UNIVERSITY_ACADEMIC)
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
            return PartialView();
        }

        public ActionResult DetailReport()
        {
            if (Int32.Parse(Session["RoleId"].ToString()) == Utilities.Constants.Role.FPT_UNIVERSITY_ACADEMIC)
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