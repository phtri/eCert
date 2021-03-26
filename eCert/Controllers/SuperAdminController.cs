using eCert.Models.ViewModel;
using eCert.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCert.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly AdminServices _adminServices;
        public SuperAdminController()
        {
            _adminServices = new AdminServices();
        }
        // GET: SuperAdmin
        public ActionResult Index()
        {
            int currentRole = 0;
            if (Session["RoleId"] != null)
            {
                currentRole = Int32.Parse(Session["RoleId"].ToString());
            }
            if (currentRole == Utilities.Constants.Role.SUPER_ADMIN)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }

        public ActionResult ManageEducation()
        {
            int currentRole = 0;
            if (Session["RoleId"] != null)
            {
                currentRole = Int32.Parse(Session["RoleId"].ToString());
            }
            if (currentRole == Utilities.Constants.Role.SUPER_ADMIN)
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
        public JsonResult GetAllEducationSystem()
        {
            List<EducationSystemViewModel> listEduSystem = _adminServices.GetAllEducatinSystem();
            return Json(listEduSystem, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListCampus(int eduSystemId)
        {
            List<CampusViewModel> listCampus = _adminServices.GetListCampusById(eduSystemId);
            //return listEduSystem;
            return Json(listCampus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEducation()
        {
            int currentRole = 0;
            if (Session["RoleId"] != null)
            {
                currentRole = Int32.Parse(Session["RoleId"].ToString());
            }
            if (currentRole == Utilities.Constants.Role.SUPER_ADMIN)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
            
        }
        public ActionResult ImportCertificateSuperadmin()
        {
            int currentRole = 0;
            if (Session["RoleId"] != null)
            {
                currentRole = Int32.Parse(Session["RoleId"].ToString());
            }
            if (currentRole == Utilities.Constants.Role.SUPER_ADMIN)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
        public ActionResult ImportDiplomaSuperadmin()
        {
            int currentRole = 0;
            if (Session["RoleId"] != null)
            {
                currentRole = Int32.Parse(Session["RoleId"].ToString());
            }
            if (currentRole == Utilities.Constants.Role.SUPER_ADMIN)
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