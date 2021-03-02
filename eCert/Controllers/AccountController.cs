using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using System.Security.Claims;
using Microsoft.Owin.Security;
using eCert.Models.ViewModel;

namespace eCert.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public void SignIn(string ReturnUrl = "/", string type = "")
        {
            if (Request.IsAuthenticated)
            {
                if (type == "Google")
                {
                    HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "Account/GoogleLoginCallback" }, "Google");
                }
            }
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

            //add to session

            return Redirect("~/");

        }
    }
}