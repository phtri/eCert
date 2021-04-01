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
        private readonly SuperAdminServices _superAdminServices;
        private readonly UserServices _userServices;
        private readonly FileServices _fileServices;

        public SuperAdminController()
        {
            _superAdminServices = new SuperAdminServices();
            _userServices = new UserServices();
            _fileServices = new FileServices();
        }
        // GET: SuperAdmin
        public ActionResult Index()
        {
            string currentRoleName = "";
            if (Session["RoleName"] != null)
            {
                currentRoleName = Session["RoleName"].ToString();
            }
            if (currentRoleName == Utilities.Constants.Role.SUPER_ADMIN)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
        public ActionResult LoadAllAdmin(int pageSize = 8, int pageNumber = 1)
        {
            //get list user academic service
            ViewBag.Pagination = _superAdminServices.GetAdminPagination(pageSize, pageNumber);
            return PartialView();
        }
        public ActionResult LoadAllAcademicService(int pageSize = 8, int pageNumber = 1)
        {
            //get list user academic service
            ViewBag.Pagination = _superAdminServices.GetAcaServicePagination(pageSize, pageNumber);
            return PartialView();
        }
        public void DeleteAdmin(int userId, int campusId, int roleId)
        {
            _userServices.DeleteAdmin(userId, campusId, roleId);
            TempData["Msg"] = "Delete user successfully";
        }
        public ActionResult ManageEducation()
        {
            string currentRoleName = "";
            if (Session["RoleName"] != null)
            {
                currentRoleName = Session["RoleName"].ToString();
            }
            if (currentRoleName == Utilities.Constants.Role.SUPER_ADMIN)
            {
                List<EducationSystemViewModel> listEduSystem = _superAdminServices.GetAllEducatinSystem();
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
            List<EducationSystemViewModel> listEduSystem = _superAdminServices.GetAllEducatinSystem();
            return Json(listEduSystem, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetListCampus(int eduSystemId)
        {
            List<CampusViewModel> listCampus = _superAdminServices.GetListCampusById(eduSystemId);
            //return listEduSystem;
            return Json(listCampus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEducation()
        {
            string currentRoleName = "";
            if (Session["RoleName"] != null)
            {
                currentRoleName = Session["RoleName"].ToString();
            }
            if (currentRoleName == Constants.Role.SUPER_ADMIN)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
            
        }
        [HttpPost]
        public ActionResult AddEducationSystem(EducationSystemViewModel educationSystemViewModel)
        {
            //Check logo image file exists
            if (educationSystemViewModel.LogoImageFile == null)
            {
                //Thông báo lỗi
                return RedirectToAction("AddEducation");
            }
            else
            {
                //Check logo image file format
                Result logoResult = _fileServices.ValidateUploadedFile(educationSystemViewModel.LogoImageFile, new string[] { "png", "jpg", "jpeg"}, 5);
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
                        _superAdminServices.UploadEducationSystemLogoImage(educationSystemViewModel);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        TempData["Msg"] = "Upload failed";
                        return RedirectToAction("Index");
                    }
                    //Add to database education system & campus
                    _superAdminServices.AddEducationSystem(educationSystemViewModel);
                }
            }
            return RedirectToAction("AddEducation");
        }
        public ActionResult ImportCertificateSuperadmin()
        {
            string currentRoleName = "";
            if (Session["RoleName"] != null)
            {
                currentRoleName = Session["RoleName"].ToString();
            }
            if (currentRoleName == Utilities.Constants.Role.SUPER_ADMIN)
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
            string currentRoleName = "";
            if (Session["RoleName"] != null)
            {
                currentRoleName = Session["RoleName"].ToString();
            }
            if (currentRoleName == Constants.Role.SUPER_ADMIN)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }

        public ActionResult AddSignature()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEducationSystemSignature(SignatureViewModel signatureViewModel)
        {
            //Check logo image file exists
            if (signatureViewModel.SignatureImageFile == null)
            {
                //Thông báo lỗi
                return RedirectToAction("AddEducation");
            }
            else
            {
                //Check logo image file format
                Result logoResult = _fileServices.ValidateUploadedFile(signatureViewModel.SignatureImageFile, new string[] { "png", "jpg", "jpeg" }, 5);
                if (logoResult.IsSuccess == false)
                {
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    //Try to upload file
                    try
                    {
                        _superAdminServices.UploadEducationSystemSingatureImage(signatureViewModel);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        TempData["Msg"] = "Upload failed";
                        return RedirectToAction("Index");
                    }
                    //Add to database education system & campus
                    _superAdminServices.AddSignature(signatureViewModel);
                }
            }
            return RedirectToAction("AddEducation");
        }

        public ActionResult ManageAccount()
        {
            string currentRoleName = "";
            if (Session["RoleName"] != null)
            {
                currentRoleName = Session["RoleName"].ToString();
            }
            if (currentRoleName == Constants.Role.SUPER_ADMIN)
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