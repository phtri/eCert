using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCert.Controllers
{
    public class EnduserController : Controller
    {
        // GET: Enduser
        public ActionResult Index()
        {
            return View("~/Views/Shared/LandingPage.cshtml");
        }

        public ActionResult DetailCertificate()
        {
            return View();
        }
    }
}