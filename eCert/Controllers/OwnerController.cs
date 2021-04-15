using eCert.Models.ViewModel;
using eCert.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace eCert.Controllers
{
    public class OwnerController : Controller
    {
        private readonly UserServices _userServices;
        public OwnerController()
        {
            _userServices = new UserServices();
        }
        // GET: Owner
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
                    ViewBag.Title = "My Certificates";
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
                    ModelState.AddModelError("ErrorMessage", "The new password is not meet complexity requirements. Please re-type new password.");
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
       
    }
}