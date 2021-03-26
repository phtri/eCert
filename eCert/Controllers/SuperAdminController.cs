using eCert.Models.ViewModel;
using eCert.Services;
using eCert.Utilities;
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
            List<CampusViewModel> listCampus = _adminServices.GetListCampusById(eduSystemId);
            //return listEduSystem;
            return Json(listCampus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEducation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddEducationSystem(EducationSystemViewModel educationSystemViewModel)
        {
            //Check logo image file exists
            if(educationSystemViewModel.LogoImageFile == null)
            {
                //Thông báo lỗi
                return RedirectToAction("AddEducation");
            }
            else
            {
                //Check logo image file format
                Result logoResult = _adminServices.ValidateEducationSystemLogoImage(educationSystemViewModel.LogoImageFile);
                if (logoResult.IsSuccess == false)
                {
                    //TempData["Msg"] = logoResult.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    //Try to upload file
                    try
                    {
                        _adminServices.UploadEducationSystemLogoImage(educationSystemViewModel);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        TempData["Msg"] = "Upload failed";
                        return RedirectToAction("Index");
                    }
                    //Add to database education system & campus
                }
            }
            return RedirectToAction("AddEducation");
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