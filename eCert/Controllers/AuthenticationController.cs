﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using System.Security.Claims;
using Microsoft.Owin.Security;
using eCert.Models.ViewModel;
using eCert.Services;
using static eCert.Utilities.Constants;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace eCert.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserServices _userServices;
        private readonly AuthenticationService _authenticationService;
        public AuthenticationController()
        {
            _userServices = new UserServices();
            _authenticationService = new AuthenticationService();
        }
        public ActionResult Index()
        {
            string currentRoleName = "";
            if (Session["RoleName"] != null)
            {
                currentRoleName = Session["RoleName"].ToString();
            }
             if (Session["RollNumber"] != null)
             {
                if(currentRoleName == Role.OWNER && !(bool)Session["isUpdatedEmail"])
                {
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                }
                else if (currentRoleName == Role.OWNER && (bool)Session["isUpdatedEmail"])
                {
                    return RedirectToAction("Index", "Certificate");
                }
                
             }
             else
             {
                if (Session["RoleId"] != null) {
                    if (currentRoleName == Role.ADMIN)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (currentRoleName == Role.FPT_UNIVERSITY_ACADEMIC)
                    {
                        return RedirectToAction("Index", "AcademicService");
                    }
                }
                else
                {
                    return View();
                }
                
             }
            return View();

        }
        public ActionResult UpdatePassword()
        {
            return View();
        }
        public ActionResult UpdatePersonalEmail()
        {
            string currentRoleName = "";
            if (Session["RoleName"] != null)
            {
                currentRoleName = Session["RoleName"].ToString();
            }
            if (Session["RollNumber"] != null)
            {
                if (currentRoleName == Role.OWNER && !(bool)Session["isUpdatedEmail"])
                {
                    return View();
                }
                else if(currentRoleName == Role.OWNER && (bool)Session["isUpdatedEmail"])
                {
                    return RedirectToAction("Index", "Certificate");
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(Session["RoleId"].ToString()) && currentRoleName == Role.ADMIN)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (!String.IsNullOrEmpty(Session["RoleId"].ToString()) && currentRoleName == Role.FPT_UNIVERSITY_ACADEMIC)
                {
                    return RedirectToAction("Index", "AcademicService");
                }
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult UpdatePersonalEmail(PersonalEmailViewModel personalEmailViewModel)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(personalEmailViewModel.PersonalEmail);
            if (string.IsNullOrEmpty(personalEmailViewModel.PersonalEmail))
            {
                ViewBag.MessageErr = "This field is required.";
            }
            else if(!match.Success)
            {
                ViewBag.MessageErr = "Email is invalid";
            }
            else
            {
                UserViewModel user = _userServices.GetUserByRollNumber(Session["RollNumber"].ToString());
                user.PersonalEmail = personalEmailViewModel.PersonalEmail;
                _userServices.UpdateUser(user);
                Session["isUpdatedEmail"] = true;
                return RedirectToAction("Index", "Certificate");
            }
                return View();
        }
        public void SignInGoogle(string type = "")
        { 
            //if (!Request.IsAuthenticated)
            //{
                if (type == "Google")
                {
                    HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "Authentication/GoogleLoginCallback" }, "Google");
                }
            //}
        }
        public ActionResult SignOut()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult GoogleLoginCallback()
        {
            var claimsPrincipal = HttpContext.User.Identity as ClaimsIdentity;
            //get login information when log in by google
            var loginInfo = GoogleLoginViewModel.GetLoginInfo(claimsPrincipal);

            //if get info fail from google
            if (loginInfo == null)
            {
                return RedirectToAction("Index");
            }
            //handle login

            //check exist email in DB
            UserViewModel user = _userServices.GetUserByAcademicEmail(loginInfo.emailaddress);
            //Nếu chưa có trong eCert, tức là owner -> Call sang danh sach sinh vien cua FAP
            if(user == null)
            {
                FAP_Service.UserWebServiceSoapClient client = new FAP_Service.UserWebServiceSoapClient();
                FAP_Service.User userFap = client.GetUserByAcademicEmail(loginInfo.emailaddress);
                if (userFap != null)
                {
                    //Sau khi có từ FAP -> Add vào db ecert
                    UserViewModel userViewModel = new UserViewModel()
                    {
                        Gender = userFap.Gender,
                        DOB = userFap.DOB,
                        PhoneNumber = userFap.PhoneNumber,
                        AcademicEmail = userFap.AcademicEmail,
                        RollNumber = userFap.RollNumber,
                        MemberCode = userFap.MemberCode,
                        Ethnicity = userFap.Ethnicity
                    };
                    //Add to eCert database
                    _userServices.AddUser(userViewModel);
                    //add to session
                    Session["RollNumber"] = userFap.RollNumber;
                    Session["RoleName"] = Role.OWNER;
                    Session["Fullname"] = loginInfo.name;
                    Session["isUpdatedEmail"] = false;
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                    //return RedirectToAction("Index", "Certificate");
                }
                else {
                    //email is invalid because not exist in FAP system 
                    return RedirectToAction("Index");
                }
            }
            else
            {
                UserViewModel userViewModel = null;
                if (!String.IsNullOrEmpty(user.RollNumber))
                {  
                    userViewModel  = _userServices.GetUserByRollNumber(user.RollNumber);
                }
                else
                {
                    userViewModel = _userServices.GetUserByAcademicEmail(loginInfo.emailaddress);
                }
                
                //add to session
                
                Session["RoleName"] = userViewModel.Role.RoleName;
                Session["Fullname"] = loginInfo.name;
                if (userViewModel.Role.RoleName == Role.OWNER && string.IsNullOrEmpty(userViewModel.PersonalEmail))
                {
                    Session["RollNumber"] = user.RollNumber;
                    Session["isUpdatedEmail"] = false;
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                }
                else if (userViewModel.Role.RoleName == Role.OWNER && !string.IsNullOrEmpty(userViewModel.PersonalEmail))
                {
                    Session["RollNumber"] = user.RollNumber;
                    Session["isUpdatedEmail"] = true;
                    return RedirectToAction("Index", "Certificate");
                }else if(userViewModel.Role.RoleName == Role.ADMIN | userViewModel.Role.RoleName == Role.SUPER_ADMIN)
                {
                    Session["AcademicEmail"] = userViewModel.AcademicEmail;
                    return RedirectToAction("Index", "Admin");
                }else if(userViewModel.Role.RoleName == Role.FPT_UNIVERSITY_ACADEMIC)
                {
                    Session["AcademicEmail"] = userViewModel.AcademicEmail;
                    return RedirectToAction("Index", "AcademicService");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Index(NormalLoginViewModel LoginViewModel)
        {
            if (ModelState.IsValid)
            {
                UserViewModel userViewModel = _userServices.Login(LoginViewModel.Email, LoginViewModel.Password);
                if (userViewModel != null)
                {
                    //add session
                    Session["RoleName"] = userViewModel.Role.RoleName;
                    Session["Fullname"] = userViewModel.AcademicEmail;
                    if (userViewModel.Role.RoleName == Role.OWNER && string.IsNullOrEmpty(userViewModel.PersonalEmail))
                    {
                        Session["RollNumber"] = userViewModel.RollNumber;
                        Session["isUpdatedEmail"] = false;
                        //redirect to update personal email page
                        return RedirectToAction("UpdatePersonalEmail", "Authentication");
                    }
                    else if (userViewModel.Role.RoleName == Role.OWNER && !string.IsNullOrEmpty(userViewModel.PersonalEmail))
                    {
                        Session["RollNumber"] = userViewModel.RollNumber;
                        Session["isUpdatedEmail"] = true;
                        return RedirectToAction("Index", "Certificate");
                    }
                    else if (userViewModel.Role.RoleName == Role.ADMIN | userViewModel.Role.RoleName == Role.SUPER_ADMIN)
                    {
                        Session["AcademicEmail"] = userViewModel.AcademicEmail;
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (userViewModel.Role.RoleName == Role.FPT_UNIVERSITY_ACADEMIC)
                    {
                        Session["AcademicEmail"] = userViewModel.AcademicEmail;
                        return RedirectToAction("Index", "AcademicService");
                    }
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Password", "Your Username or Password is incorrect");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        private UserViewModel IsValidUser(string email, string password)
        {
            string encryptPassword = CreateMD5(password).ToLower(); 
            //check exist email in DB
            UserViewModel userViewModel = _userServices.GetUserByProvidedEmailAndPass(email, encryptPassword);
            if(userViewModel != null)
            {
                return userViewModel;
            }
            return null;
        }

        //public string encryption(String password)
        //{
        //    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        //    byte[] encrypt;
        //    UTF8Encoding encode = new UTF8Encoding();
        //    //encrypt the given password string into Encrypted data  
        //    encrypt = md5.ComputeHash(encode.GetBytes(password));
        //    StringBuilder encryptdata = new StringBuilder();
        //    //Create a new string by using the encrypted data  
        //    for (int i = 0; i < encrypt.Length; i++)
        //    {
        //        encryptdata.Append(encrypt[i].ToString());
        //    }
        //    return encryptdata.ToString();
        //}

        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}