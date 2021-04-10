﻿using eCert.Models.ViewModel;
using eCert.Services;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static eCert.Utilities.Constants;

namespace eCert.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly SuperAdminServices _superAdminServices;
        private readonly UserServices _userServices;
        private readonly FileServices _fileServices;
        private readonly AdminServices _adminServices;
        public SuperAdminController()
        {
            _superAdminServices = new SuperAdminServices();
            _userServices = new UserServices();
            _fileServices = new FileServices();
            _adminServices = new AdminServices();

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
        public ActionResult AddAcaService()
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

        public ActionResult AddAdmin()
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
        public ActionResult ManageCampus()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAcaService(UserAcaServiceViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                string academicEmail = userViewModel.AcademicEmail;
                //check exist email in DB
                UserViewModel user = _userServices.GetUserByAcademicEmail(academicEmail);
                if (user != null && user.Role.RoleName != Utilities.Constants.Role.FPT_UNIVERSITY_ACADEMIC)
                {
                    ModelState.AddModelError("ErrorMessage", "Invalid. This email has been existed.");
                    return View();
                }
                //check if choosen campus already has academic service
                UserViewModel userByCampusId = _userServices.GetAcaServiceByCampusId(userViewModel.CampusId);
                //case email existed in DB
                if (userByCampusId != null)
                {
                    ModelState.AddModelError("ErrorMessage", "Invalid. There is already a academic service of this campus.");
                    return View();
                }
                else
                {
                    UserViewModel addAcademicService = new UserViewModel()
                    {
                        UserId = (user != null) ? user.UserId : -1,
                        PhoneNumber = userViewModel.PhoneNumber,
                        AcademicEmail = academicEmail
                    };
                    _superAdminServices.AddAcademicSerivce(addAcademicService, userViewModel.CampusId);

                    //send email
                    
                    TempData["Msg"] = "Create academic service user successfully";
                    TempData["Tab"] = 2;
                    return View();
                }
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult AddAdmin(UserAcaServiceViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                string academicEmail = userViewModel.AcademicEmail;
                //check exist email in DB
                UserViewModel user = _userServices.GetUserByAcademicEmail(academicEmail);
                if(user != null && user.Role.RoleName != Utilities.Constants.Role.ADMIN)
                {
                    ModelState.AddModelError("ErrorMessage", "Invalid. This email has been existed.");
                    return View();
                }
                //check if choosen campus already has academic service
                UserViewModel userByCampusId = _userServices.GetAdminByCampusId(userViewModel.CampusId);
                //case email existed in DB
                if (userByCampusId != null)
                {
                    ModelState.AddModelError("ErrorMessage", "This campus has already had an admin");
                    return View();
                }
                else
                {
                    UserViewModel addAcademicService = new UserViewModel()
                    {
                        UserId = (user != null) ? user.UserId : -1,
                        PhoneNumber = userViewModel.PhoneNumber,
                        AcademicEmail = academicEmail
                    };
                    _superAdminServices.AddAdminSerivce(addAcademicService, userViewModel.CampusId);

                    //send email

                    TempData["Msg"] = "Create admin user successfully";
                    TempData["Tab"] = 1;
                    return View();
                }
            }
            else
            {
                return View();
            }

        }
        public ActionResult LoadAllAdmin(int pageSize = 8, int pageNumber = 1)
        {
            //get list user academic service
            ViewBag.Pagination = _superAdminServices.GetAdminPagination(pageSize, pageNumber);
            if(ViewBag.Pagination.PagingData.Count == 0)
            {
                ViewBag.OverflowHidden = "overflow-hidden";
            }
            else
            {
                ViewBag.OverflowHidden = String.Empty;
            }
            
            TempData["Tab"] = 1;
            return PartialView();
        }
        public ActionResult LoadAllAcademicService(int pageSize = 8, int pageNumber = 1)
        {
            //get list user academic service
            ViewBag.Pagination = _superAdminServices.GetAcaServicePagination(pageSize, pageNumber);
            if (ViewBag.Pagination.PagingData.Count == 0)
            {
                ViewBag.OverflowHidden = "overflow-hidden";
            }
            else
            {
                ViewBag.OverflowHidden = String.Empty;
            }
            TempData["Tab"] = 2;
            return PartialView();
        }
        public void DeleteAdmin(int userId, int campusId, int roleId)
        {
            _userServices.DeleteAdmin(userId, campusId, roleId);
            TempData["Msg"] = "Delete admin user successfully";
        }
        public JsonResult DeleteCampus(int campusId)
        {
            Result result = new Result();
            int numberOfCert = _superAdminServices.GetCountCertificateByCampus(campusId);
            if (numberOfCert != 0)
            {
                result.IsSuccess = false;
                result.Message = "This campus can not be deleted because there has already existed certificates that provided by this campus.";
            }
            else
            {
                _superAdminServices.DeleteCampus(campusId);
                result.IsSuccess = true;
                result.Message = "Delete campus successfully";
            }
           

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteEducation(int eduSystemId)
        {
            Result result = new Result();
            int numberOfCert = _superAdminServices.GetCountCertificateByEdu(eduSystemId);
            if (numberOfCert != 0)
            {
                result.IsSuccess = false;
                result.Message = "This campus can not be deleted because there has already existed certificates that provided by this Education System.";
            }
            else
            {
                _superAdminServices.DeleteEducation(eduSystemId);
                result.IsSuccess = true;
                result.Message = "Delete Education System successfully";
            }


            return Json(result, JsonRequestBehavior.AllowGet);
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
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
            
        }
        public ActionResult LoadListOfEducationSystem(int pageSize = 8, int pageNumber = 1)
        {
            ViewBag.Pagination = _superAdminServices.GetEducatinSystemPagination(pageSize, pageNumber);
            return PartialView();
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

        public ActionResult LoadListCampusByEdu(int eduSystemId, int pageSize = 8, int pageNumber = 1)
        {
            //get list user academic service
            ViewBag.Pagination = _superAdminServices.GetListCampusById(pageSize, pageNumber, eduSystemId);
            return PartialView();
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
        public ActionResult AddEducation(EducationSystemViewModel educationSystemViewModel)
        {
            if (ModelState.IsValid)
            {
                //Check logo image file exists
                if (educationSystemViewModel.LogoImageFile == null)
                {
                    //Thông báo lỗi
                    ViewBag.Msg = "Upload logo fail.";
                    return View();
                }
                else
                {
                    //Check logo image file format
                    Result logoResult = _fileServices.ValidateUploadedFile(educationSystemViewModel.LogoImageFile, new string[] { "png", "jpg", "jpeg" }, 5);
                    if (logoResult.IsSuccess == false)
                    {
                        //TempData["Msg"] = logoResult.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        int count = _superAdminServices.GetCountEduByName(educationSystemViewModel.EducationName.ToLower());
                        if (count != 0)
                        {
                            ViewBag.Msg = "This education system has already existed. Please input other name.";
                            return View();
                        }
                        //Try to upload file
                        try
                        {
                            _superAdminServices.UploadEducationSystemLogoImage(educationSystemViewModel);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            ViewBag.Msg = "Upload failed";
                            return View();
                        }
                       
                        //Add to database education system & campus
                        _superAdminServices.AddEducationSystem(educationSystemViewModel);
                    }
                }
                TempData["Msg"] = "Create Education System successfully";
                return View();
            }
            return View();
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
        public ActionResult AddSignature(SignatureViewModel signatureViewModel)
        {
            if (ModelState.IsValid)
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
                TempData["Msg"] = "Create Signature successfully";
                return View();
            }
            return View();
        }


        [HttpPost]
        public ActionResult ImportCertificateSuperAdmin(ImportExcel importExcelFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string errorMsg = "Invalid upload file. Please check the error cells below.<br/>";
                    errorMsg += "<br/>";
                    string errorFullname = String.Empty;
                    string labelRequire = String.Empty;
                    string labelSpecialChar = String.Empty;
                    
                    

                    ResultExcel resultExcel = _adminServices.ImportCertificatesByExcel(importExcelFile.File, Server.MapPath("~/Uploads/"), TypeImportExcel.IMPORT_CERT, importExcelFile.CampusId, importExcelFile.SignatureId);
                    if (resultExcel.ListRowError.Count != 0)
                    {
                        foreach (RowExcel rowExcel in resultExcel.ListRowError)
                        {
                            if (rowExcel.TypeError == 1)
                            {
                                if (rowExcel.Rows.Count != 0)
                                {
                                    if (labelRequire == String.Empty)
                                    {
                                        labelRequire += "<br/>REQUIRED ERROR:";
                                        errorMsg += labelRequire;
                                    }
                                    
                                    errorMsg += "Column " + rowExcel.ColumnName + " are reqired at rows ";
                                    foreach (int row in rowExcel.Rows)
                                    {
                                        errorMsg += row + ", ";
                                    }
                                    errorMsg = errorMsg.Remove(errorMsg.Length - 1);
                                    errorMsg = errorMsg.Remove(errorMsg.Length - 1);
                                    errorMsg += "<br/>";
                                }
                            }
                            else if (rowExcel.TypeError == 2)
                            {
                                if (rowExcel.Rows.Count != 0)
                                {
                                    if (labelSpecialChar == String.Empty)
                                    {
                                        labelSpecialChar += "<br/>SPECIAL CHARACTERS ERROR:";
                                        errorMsg += labelSpecialChar;
                                    }
                                   
                                    errorFullname += "Column " + rowExcel.ColumnName + " can not contain digit or special characters at rows ";
                                    foreach (int row in rowExcel.Rows)
                                    {
                                        errorFullname += row + ", ";
                                    }
                                    errorFullname = errorFullname.Remove(errorFullname.Length - 1);
                                    errorFullname = errorFullname.Remove(errorFullname.Length - 1);
                                    errorFullname += "<br/>";
                                }
                            }

                        }
                        errorMsg = errorMsg += "<br/>";
                        ViewBag.MessageError = errorMsg += errorFullname;
                    }
                    else
                    {
                        ViewBag.MessageSuccess = resultExcel.RowCountSuccess + " rows are imported succesfully";
                    }


                }
            }
            catch
            {
                ViewBag.MessageError = "File is not valid";
            }

            return View();
        }

        [HttpPost]
        public ActionResult ImportDiplomaSuperAdmin(ImportExcel importExcelFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string errorMsg = String.Empty;
                    string errorMsgInvalidDate = String.Empty;
                    string errorFullname = String.Empty;
                    ResultExcel resultExcel = _adminServices.ImportCertificatesByExcel(importExcelFile.File, Server.MapPath("~/Uploads/"), TypeImportExcel.IMPORT_DIPLOMA, importExcelFile.CampusId, importExcelFile.SignatureId);
                    if (resultExcel.ListRowError.Count != 0)
                    {
                        foreach (RowExcel rowExcel in resultExcel.ListRowError)
                        {
                            if (rowExcel.TypeError == 1)
                            {
                                if (rowExcel.Rows.Count != 0)
                                {
                                    errorMsg += "Column " + rowExcel.ColumnName + " are reqired at rows ";
                                    foreach (int row in rowExcel.Rows)
                                    {
                                        errorMsg += row + ", ";
                                    }
                                    errorMsg = errorMsg.Remove(errorMsg.Length - 1);
                                    errorMsg = errorMsg.Remove(errorMsg.Length - 1);
                                    errorMsg += "<br/>";
                                }
                            }
                            else if (rowExcel.TypeError == 2)
                            {
                                if (rowExcel.Rows.Count != 0)
                                {
                                    errorMsgInvalidDate += "Column " + rowExcel.ColumnName + " are invalid format at rows ";
                                    foreach (int row in rowExcel.Rows)
                                    {
                                        errorMsgInvalidDate += row + ", ";
                                    }
                                    errorMsgInvalidDate = errorMsgInvalidDate.Remove(errorMsgInvalidDate.Length - 1);
                                    errorMsgInvalidDate = errorMsgInvalidDate.Remove(errorMsgInvalidDate.Length - 1);
                                    errorMsgInvalidDate += "<br/>";
                                }
                            }
                            else if (rowExcel.TypeError == 3)
                            {
                                if (rowExcel.Rows.Count != 0)
                                {
                                    errorFullname += "Column " + rowExcel.ColumnName + " can not contain digit at rows ";
                                    foreach (int row in rowExcel.Rows)
                                    {
                                        errorFullname += row + ", ";
                                    }
                                    errorFullname = errorFullname.Remove(errorFullname.Length - 1);
                                    errorFullname = errorFullname.Remove(errorFullname.Length - 1);
                                    errorFullname += "<br/>";
                                }
                            }

                        }
                        errorMsg = errorMsg += "<br/>";
                        errorMsg += errorMsgInvalidDate;
                        errorMsg = errorMsg += "<br/>";
                        ViewBag.MessageError = errorMsg += errorFullname;
                    }
                    else
                    {
                        ViewBag.MessageSuccess = resultExcel.RowCountSuccess + " rows are imported succesfully";
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.MessageError = "File is not valid";
            }

            return View();
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
                if (TempData["Tab"] == null)
                {
                    TempData["Tab"] = 1;
                }
                
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
    }
}