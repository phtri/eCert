using eCert.Models.ViewModel;
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
        private readonly UserServices _userServices;
        
        public AcademicServiceController()
        {
            _certificateServices = new CertificateServices();
            _userServices = new UserServices();
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
            string academicEmail = Session["AcademicEmail"].ToString();
            UserViewModel userViewModel = _userServices.GetUserByAcademicEmail(academicEmail);

            ViewBag.Pagination = _certificateServices.GetReportByUserIdPagination(userViewModel.UserId ,pageSize, pageNumber);
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
        public ActionResult DetailReport(int reportId)
        {
            string currentRoleName = "";
            if (Session["RoleName"] != null)
            {
                currentRoleName = Session["RoleName"].ToString();
            }
            if (currentRoleName == Utilities.Constants.Role.FPT_UNIVERSITY_ACADEMIC)
            {
                ReportViewModel report = _certificateServices.GetReportByReportId(reportId);
                UserViewModel userViewModel = _userServices.GetUserByUserId(report.UserId);
                ViewBag.User = userViewModel;
                return View(report);
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }

        public JsonResult GetStatusByReportId(int reportId)
        {
            ReportViewModel report = _certificateServices.GetReportByReportId(reportId);
            return Json(report.Status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DetailReport(ReportViewModel reportViewModel)
        {
            ReportViewModel report = _certificateServices.GetReportByReportId(reportViewModel.ReportId);
            UserViewModel userViewModel = _userServices.GetUserByUserId(report.UserId);
            ViewBag.User = userViewModel;
            report.Status = reportViewModel.Status;
            _certificateServices.UpdateReport(report);
            TempData["Msg"] = "Update status report successfully.";
            return View(report);
        }


    }
}