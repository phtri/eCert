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
             if (Session["RollNumber"] != null)
             {
                if(Int32.Parse(Session["RoleId"].ToString()) == RoleCons.OWNER)
                {
                    return RedirectToAction("Index", "Certificate");
                }else if(Int32.Parse(Session["RoleId"].ToString()) == RoleCons.ADMIN)
                {
                    return RedirectToAction("Index", "Admin");
                }
             }
             else
             {
                return View();
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
            Session.Remove("RollNumber");
            Session.Remove("RoleId");
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
            //Nếu chưa có trong eCert, tức là owner -> Call sang FAP
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
                        Ethnicity = userFap.Ethnicity
                    };
                    //Add to database
                    _userServices.AddUser(userViewModel);
                    //add to session
                    Session["RollNumber"] = userFap.RollNumber;
                    Session["RoleId"] = RoleCons.OWNER;
                    return RedirectToAction("Index", "Certificate");
                }
                else {
                    //email is invalid because not exist in FAP system 
                    return RedirectToAction("Index");
                }
            }
            else
            {
                UserViewModel userViewModel = _userServices.GetUserByRollNumber(user.RollNumber);
                //add to session
                Session["RollNumber"] = user.RollNumber;
                Session["RoleId"] = userViewModel.Role.RoleId;
                Session["Fullname"] = loginInfo.name;
                if (userViewModel.Role.RoleId == RoleCons.OWNER)
                {
                    return RedirectToAction("Index", "Certificate");
                }else if(userViewModel.Role.RoleId == RoleCons.ADMIN)
                {
                    return RedirectToAction("Index", "Admin");
                }
                
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Login(NormalLoginViewModel LoginViewModel)
        {
            if (IsValidUser(LoginViewModel.Email, LoginViewModel.Password))
            {
                ////add session
                //Session["RollNumber"] = user.RollNumber;
                //Session["RoleId"] = userViewModel.Role.RoleId;
                //Session["Fullname"] = loginInfo.name;

                //if (userViewModel.Role.RoleId == RoleCons.OWNER)
                //{
                //    return RedirectToAction("Index", "Certificate");
                //}
                //else if (userViewModel.Role.RoleId == RoleCons.ADMIN)
                //{
                //    return RedirectToAction("Index", "Admin");
                //}
            }
            else
            {
                ModelState.AddModelError("", "Your Email and password is incorrect");
            }
            return View(LoginViewModel);
        }

        private bool IsValidUser(string email, string password)
        {
            string encryptPassword = CreateMD5(password).ToLower(); ;
            //check exist email in DB
            UserViewModel user = _userServices.GetUserByProvidedEmailAndPass(email, encryptPassword);
            if(user != null)
            {
                return true;
            }
            return false;
        }

        public string encryption(String password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            //encrypt the given password string into Encrypted data  
            encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
        }

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