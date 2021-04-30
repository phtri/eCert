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
using static eCert.Utilities.Constants;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using eCert.Utilities;

namespace eCert.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserServices _userServices;
        private readonly EmailServices _emailServices;
        public AuthenticationController()
        {
            _userServices = new UserServices();
            _emailServices = new EmailServices();
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
                if (currentRoleName == Role.OWNER && !(bool)Session["isUpdatedEmail"])
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
                if (Session["RoleName"] != null)
                {
                    if (currentRoleName == Role.ADMIN)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (currentRoleName == Role.SUPER_ADMIN)
                    {
                        return RedirectToAction("Index", "SuperAdmin");
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
                else if (currentRoleName == Role.OWNER && !(bool)Session["isVerifyMail"])
                {
                    return View();
                }
                else if (currentRoleName == Role.OWNER && (bool)Session["isUpdatedEmail"])
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
        public ActionResult NotificationCheckMail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdatePersonalEmail(PersonalEmailViewModel personalEmailViewModel)
        {
            if (ModelState.IsValid)
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(personalEmailViewModel.PersonalEmail);
                if (string.IsNullOrEmpty(personalEmailViewModel.PersonalEmail))
                {
                    ViewBag.MessageErr = "This field is required.";

                }
                else if (!match.Success)
                {
                    ViewBag.MessageErr = "Email is invalid";
                }
                else
                {
                    UserViewModel user = _userServices.GetUserByRollNumber(Session["RollNumber"].ToString());
                    Result r = _userServices.UpdatePersonalEmail(user, personalEmailViewModel.PersonalEmail);
                    if (r.IsSuccess)
                    {
                        //Display check email
                        TempData["PersonalEmail"] = personalEmailViewModel.PersonalEmail;
                        return RedirectToAction("NotificationCheckMail", "Authentication");
                    }
                    else
                    {
                        //Display error message
                    }
                    //Session["isUpdatedEmail"] = true;
                    return RedirectToAction("Index", "Certificate");
                }
            }
            return RedirectToAction("ChangePersonalEmail", "Authentication");
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
            if (user == null)
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
                else
                {
                    //email is invalid because not exist in FAP system 
                    return RedirectToAction("Index");
                }
            }
            else
            {
                UserViewModel userViewModel = null;
                if (!String.IsNullOrEmpty(user.RollNumber))
                {
                    userViewModel = _userServices.GetUserByRollNumber(user.RollNumber);
                }
                else
                {
                    userViewModel = _userServices.GetUserByAcademicEmail(loginInfo.emailaddress);
                }

                //add to session

                Session["Fullname"] = loginInfo.name;
                if (userViewModel.Role.RoleName == Role.OWNER && !string.IsNullOrEmpty(userViewModel.PersonalEmail) && userViewModel.IsVerifyMail == false)
                {
                    Session["RoleName"] = userViewModel.Role.RoleName;
                    Session["RollNumber"] = user.RollNumber;
                    Session["isUpdatedEmail"] = true;
                    Session["isVerifyMail"] = false;
                    TempData["PersonalEmail"] = userViewModel.PersonalEmail;
                    return RedirectToAction("NotificationCheckMail", "Authentication");

                }
                else if (userViewModel.Role.RoleName == Role.OWNER && string.IsNullOrEmpty(userViewModel.PersonalEmail))
                {
                    Session["RoleName"] = userViewModel.Role.RoleName;
                    Session["RollNumber"] = user.RollNumber;
                    Session["isUpdatedEmail"] = false;
                    Session["isVerifyMail"] = false;
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                }
                else if (userViewModel.Role.RoleName == Role.OWNER && !string.IsNullOrEmpty(userViewModel.PersonalEmail) && userViewModel.IsVerifyMail)
                {
                    Session["RoleName"] = userViewModel.Role.RoleName;
                    Session["RollNumber"] = user.RollNumber;
                    Session["isUpdatedEmail"] = true;
                    return RedirectToAction("Index", "Certificate");
                }
                else if (userViewModel.Role.RoleName == Role.ADMIN)
                {
                    //if (!userViewModel.IsActive)
                    //{
                    //    TempData["Msg"] = "This account is inactive. Please contact with higher level user to active.";
                    //    return RedirectToAction("Index", "Authentication");
                    //}
                    //else
                    //{
                    Session["RoleName"] = userViewModel.Role.RoleName;
                    Session["AcademicEmail"] = userViewModel.AcademicEmail;
                    return RedirectToAction("Index", "Admin");
                    //}
                }
                else if (userViewModel.Role.RoleName == Role.SUPER_ADMIN)
                {
                    Session["RoleName"] = userViewModel.Role.RoleName;
                    return RedirectToAction("Index", "SuperAdmin");
                }
                else if (userViewModel.Role.RoleName == Role.FPT_UNIVERSITY_ACADEMIC)
                {
                    Session["RoleName"] = userViewModel.Role.RoleName;
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
                    if (userViewModel.Role.RoleName == Role.OWNER && (string.IsNullOrEmpty(userViewModel.PersonalEmail) || userViewModel.IsVerifyMail == false))
                    {
                        Session["RollNumber"] = userViewModel.RollNumber;
                        Session["isUpdatedEmail"] = false;
                        //redirect to update personal email page
                        return RedirectToAction("UpdatePersonalEmail", "Authentication");
                    }
                    else if (userViewModel.Role.RoleName == Role.OWNER && !string.IsNullOrEmpty(userViewModel.PersonalEmail) && userViewModel.IsVerifyMail)
                    {
                        Session["RollNumber"] = userViewModel.RollNumber;
                        Session["isUpdatedEmail"] = true;
                        return RedirectToAction("Index", "Certificate");
                    }
                    else if (userViewModel.Role.RoleName == Role.ADMIN)
                    {
                        Session["AcademicEmail"] = userViewModel.AcademicEmail;
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (userViewModel.Role.RoleName == Role.SUPER_ADMIN)
                    {
                        return RedirectToAction("Index", "SuperAdmin");
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

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string email, string token)
        {
            bool result = _userServices.ConfirmPersonalEmail(email, token);
            if (result)
            {
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
                return RedirectToAction("Index", "Authentication");
            }

            return RedirectToAction("Index", "Certificate");
        }


        public ActionResult ChangePassword()
        {
            if (Session["RollNumber"] != null)
            {
                if (!String.IsNullOrEmpty(Session["isUpdatedEmail"].ToString()) && (bool)Session["isUpdatedEmail"])
                {
                    return View();
                }
                else
                {
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                }
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }

        public ActionResult ChangePersonalEmail()
        {
            if (Session["RollNumber"] != null)
            {
                if (!String.IsNullOrEmpty(Session["isUpdatedEmail"].ToString()) && (bool)Session["isUpdatedEmail"])
                {
                    UserViewModel userViewModel = _userServices.GetUserByRollNumber(Session["RollNumber"].ToString());
                    ViewBag.CurrentMail = userViewModel.PersonalEmail;
                    return View();
                }
                else
                {
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                }
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
        [HttpPost]
        public ActionResult ChangePersonalEmail(PersonalEmailViewModel personalEmailViewModel)
        {
            if (string.IsNullOrEmpty(personalEmailViewModel.PersonalEmail))
            {
                ViewBag.MessageErr = "Email is required, please enter your new personal email address";
                return View();
            }
            UserViewModel userViewModel = _userServices.GetUserByRollNumber(Session["RollNumber"].ToString());
            ViewBag.CurrentMail = userViewModel.PersonalEmail;
            if (personalEmailViewModel.PersonalEmail == userViewModel.PersonalEmail)
            {
                ViewBag.MessageErr = "Your new personal email must be different with current personal email";
                return View();
            }
            if (!_emailServices.IsMailValid(personalEmailViewModel.PersonalEmail))
            {
                ViewBag.MessageErr = "Email is wrong format";
                return View();
            }
            if (personalEmailViewModel.PersonalEmail.Contains("@fpt.edu.vn"))
            {
                ViewBag.MessageErr = "Please enter your personal email address, you can not enter fpt email address";
                return View();
            }

            else
            {
                UserViewModel user = _userServices.GetUserByRollNumber(Session["RollNumber"].ToString());
                Result r = _userServices.UpdatePersonalEmail(user, personalEmailViewModel.PersonalEmail);
                if (r.IsSuccess)
                {
                    //Display check email
                    TempData["PersonalEmail"] = personalEmailViewModel.PersonalEmail;
                    return View();
                }
                else
                {
                    ViewBag.MessageErr = r.Message;
                    return View();
                }
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(PasswordViewModel passwordViewModel)
        {
            if (ModelState.IsValid)
            {
                //check current password
                string rollNumber = Session["RollNumber"].ToString();
                UserViewModel userViewModel = _userServices.GetUserByRollNumber(rollNumber);
                bool passWordresult = false;
                bool newPassWordresult = false;
                if (userViewModel != null)
                {
                    passWordresult = BCrypt.Net.BCrypt.Verify(passwordViewModel.CurrentPassword, userViewModel.PasswordHash);
                    newPassWordresult = BCrypt.Net.BCrypt.Verify(passwordViewModel.NewPassword, userViewModel.PasswordHash);
                }
                if (!passWordresult)
                {
                    ModelState.AddModelError("ErrorMessage", "The current password is incorrect.");
                    return View();
                }
                if (newPassWordresult)
                {
                    ModelState.AddModelError("ErrorMessage", "The new password and current password can not be matched. Please re-type new password");
                    return View();
                }
                Regex rgx = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
                if (!rgx.IsMatch(passwordViewModel.NewPassword))
                {
                    ModelState.AddModelError("ErrorMessage", "The new password does not meet complexity requirements. Please re-type new password.");
                    return View();
                }
                if (!passwordViewModel.ConfirmPassword.Equals(passwordViewModel.NewPassword))
                {
                    ModelState.AddModelError("ErrorMessage", "The new and confirm passwords must match. Please re-type them.");
                    return View();
                }
                //Change password
                userViewModel.PasswordHash = passwordViewModel.NewPassword;
                _userServices.ChangePassword(userViewModel);
                ModelState.AddModelError("SuccessMessage", "Change password successfully.");
            }
            return View();

        }

        public ActionResult ResetPassword()
        {
            if (Session["RollNumber"] != null)
            {
                return RedirectToAction("Index", "Certificate");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (String.IsNullOrEmpty(resetPasswordViewModel.PersonalEmail))
            {
                ModelState.AddModelError("PersonalEmail", "You can not leave your personal email address empty, please enter your personal email address");
                return View();
            }
            //Email address format
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(resetPasswordViewModel.PersonalEmail);
            if (!match.Success)
            {
                ModelState.AddModelError("PersonalEmail", "Your personal email address is not in correct format");
                return View();
            }

            Result resetPswdResult = _userServices.ResetPassword(resetPasswordViewModel.PersonalEmail);
            if (!resetPswdResult.IsSuccess)
            {
                ModelState.AddModelError("PersonalEmail", resetPswdResult.Message);
                return View();
            }
            ModelState.AddModelError("PersonalEmail", "Please check your personal email address for new account password");
            return View();
        }

        public ActionResult Indexx(HttpContext ctx)
        {
            string currentRoleName = "";
            if (ctx.Session["RoleName"] != null)
            {
                currentRoleName = ctx.Session["RoleName"].ToString();
            }
            if (ctx.Session["RollNumber"] != null)
            {
                if (currentRoleName == Role.OWNER && !(bool)ctx.Session["isUpdatedEmail"])
                {
                    //redirect to update personal email page
                    return View("~/Views/Authentication/UpdatePersonalEmail.cshtml");
                }
                else if (currentRoleName == Role.OWNER && (bool)ctx.Session["isUpdatedEmail"])
                {
                    return View("~/Views/Certificate/Index.cshtml");
                }

            }
            else
            {
                if (ctx.Session["RoleName"] != null)
                {
                    if (currentRoleName == Role.ADMIN)
                    {
                        return View("~/Views/Admin/Index.cshtml");
                    }
                    else if (currentRoleName == Role.SUPER_ADMIN)
                    {
                        return View("~/Views/SuperAdmin/Index.cshtml");
                    }
                    else if (currentRoleName == Role.FPT_UNIVERSITY_ACADEMIC)
                    {
                        return View("~/Views/AcademicService/Index.cshtml");
                    }
                }
                else
                {
                    return View("~/Views/Authentication/Index.cshtml");
                }

            }
            return View("~/Views/Authentication/Index.cshtml");
        }

        public ActionResult UpdatePersonalEmaill(HttpContext ctx)
        {
            string currentRoleName = "";
            if (ctx.Session["RoleName"] != null)
            {
                currentRoleName = ctx.Session["RoleName"].ToString();
            }
            if (ctx.Session["RollNumber"] != null)
            {
                if (currentRoleName == Role.OWNER && !(bool)ctx.Session["isUpdatedEmail"])
                {
                    return View("~/Views/Authentication/UpdatePersonalEmail.cshtml");
                }
                else if (currentRoleName == Role.OWNER && !(bool)ctx.Session["isVerifyMail"])
                {
                    return View("~/Views/Authentication/UpdatePersonalEmail.cshtml");
                }
                else if (currentRoleName == Role.OWNER && (bool)ctx.Session["isUpdatedEmail"])
                {
                    return View("~/Views/Certifcate/Index.cshtml");
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(ctx.Session["RoleId"].ToString()) && currentRoleName == Role.ADMIN)
                {
                    return View("~/Views/Admin/Index.cshtml");
                }
                else if (!String.IsNullOrEmpty(ctx.Session["RoleId"].ToString()) && currentRoleName == Role.FPT_UNIVERSITY_ACADEMIC)
                {
                    return View("~/Views/AcademicService/Index.cshtml");
                }
            }
            return View("~/Views/Authentication/Index.cshtml");

        }

        public ActionResult ChangePasswordd(HttpContext ctx)
        {
            if (ctx.Session["RollNumber"] != null)
            {
                if (!String.IsNullOrEmpty(ctx.Session["isUpdatedEmail"].ToString()) && (bool)ctx.Session["isUpdatedEmail"])
                {
                    return View("ChangePassword", "Authentication");
                }
                else
                {
                    //redirect to update personal email page
                    return View("UpdatePersonalEmail", "Authentication");
                }
            }
            else
            {
                return View("Index", "Authentication");
            }
        }

        public ActionResult ResetPasswordd(HttpContext ctx)
        {
            if (ctx.Session["RollNumber"] != null)
            {
                return View("Index", "Certificate");
            }
            else
            {
                return View("ResetPassword", "Authentication");
            }
           
        }
    }
}