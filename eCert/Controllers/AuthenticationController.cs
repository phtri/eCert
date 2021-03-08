using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using System.Security.Claims;
using Microsoft.Owin.Security;
using eCert.Models.ViewModel;
using eCert.Services;

namespace eCert.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserServices _userServices;
        public AuthenticationController()
        {
            _userServices = new UserServices();
        }
        public ActionResult Index()
        {
            return View();
        }

        public void SignInGoogle(string ReturnUrl = "/", string type = "")
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
            HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Redirect("~/");
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
            //Nếu chưa có trong db -> Call sang FAP
            if(user == null)
            {
                FAP_Service.UserWebServiceSoapClient client = new FAP_Service.UserWebServiceSoapClient();
                FAP_Service.User userFap = client.GetUserByAcademicEmail(loginInfo.emailaddress);
                //Sau khi có từ FAP -> Add vào db ecert
            }

            //add to session

            
            return RedirectToAction("Index", "Certificate");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(NormalLoginViewModel LoginViewModel)
        {
            if (IsValidUser(LoginViewModel.Email, LoginViewModel.Password))
            {
                //add session


                return RedirectToAction("Home", "Index");
            }
            else
            {
                ModelState.AddModelError("", "Your Email and password is incorrect");
            }
            return View(LoginViewModel);
        }

        private bool IsValidUser(string email, string password)
        {
            //var encryptpassowrd = Base64Encode(password);
            //return IsValid;
            return true;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}