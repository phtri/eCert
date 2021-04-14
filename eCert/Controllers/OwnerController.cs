using eCert.Models.ViewModel;
using eCert.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
                if (userViewModel != null)
                {
                    passWordresult = BCrypt.Net.BCrypt.Verify(passwordViewModel.CurrentPassword, userViewModel.PasswordHash);
                }
                if (!passWordresult)
                {
                    ModelState.AddModelError("ErrorMessage", "The current password is incorrect.");
                }
                return View();
            }
            return View();

        }
    }
}