using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCert.Controllers
{
    public class OwnerController : Controller
    {
        // GET: Owner
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult ChangePersonalEmail()
        {
            return View();
        }
    }
}