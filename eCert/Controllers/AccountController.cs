using eCert.Models.Entity;
using eCert.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace eCert.Controllers
{
    public class AccountController : Controller
    {
        // GET:Login this Action method simple return the Login View
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //Post:When user click on the submit button then this method will call
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel LoginViewModel)
        {
            if (IsValidUser(LoginViewModel.Email, LoginViewModel.Password))
            {
                //FormsAuthentication.SetAuthCookie(LoginViewModel.Email, false);
                //add session


                return RedirectToAction("Home", "Index");
            }
            else
            {
                ModelState.AddModelError("", "Your Email and password is incorrect");
            }
            return View(LoginViewModel);
        }
       

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        
        private bool IsValidUser(string email, string password)
        {
            //var encryptpassowrd = Base64Encode(password);
            //bool IsValid = false;
            //string query = "select * from TblUsers where Email=@email and Password=@Password";
            //using (SqlCommand cmd = new SqlCommand(query, con))
            //{
            //    cmd.Parameters.AddWithValue("@Email", email);
            //    cmd.Parameters.AddWithValue("@Password", encryptpassowrd);
            //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //    DataTable dt = new DataTable();
            //    sda.Fill(dt);
            //    con.Open();
            //    int i = cmd.ExecuteNonQuery();
            //    con.Close();
            //    if (dt.Rows.Count > 0)
            //    {
            //        IsValid = true;
            //    }
            //}
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