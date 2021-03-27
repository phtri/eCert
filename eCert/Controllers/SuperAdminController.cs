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
            return View();
        }

        public ActionResult ManageEducation()
        {
            List<EducationSystemViewModel> listEduSystem = _adminServices.GetAllEducatinSystem();
            ViewBag.ListEducationSystem = listEduSystem;
            return View();
        }
        public JsonResult GetAllEducationSystem()
        {
            List<EducationSystemViewModel> listEduSystem = _adminServices.GetAllEducatinSystem();
            return Json(listEduSystem, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListCampus(int eduSystemId)
        {
            List<CampusViewModel> listEduSystem = _adminServices.GetListCampusById(eduSystemId);
            //return listEduSystem;
            return Json(listEduSystem, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEducation()
        {
            return View();
        }

        public ActionResult ManageSignatory()
        {
            return View();
        }

    }
}