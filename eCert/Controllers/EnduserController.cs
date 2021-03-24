using eCert.Models.ViewModel;
using eCert.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCert.Controllers
{
    public class EnduserController : Controller
    {
        private readonly CertificateServices _certificateServices;
        public EnduserController()
        {
            _certificateServices = new CertificateServices();
        }
        // GET: Enduser
        public ActionResult Index()
        {
            return View("~/Views/Shared/LandingPage.cshtml");
        }

        public ActionResult Share(string url)
        {
            ViewBag.Title = "FPT Certificate Detail";
            CertificateViewModel certViewModel = _certificateServices.GetCertificateByUrl(url);
            return View(certViewModel);
        }

        public ActionResult Certificate()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
    }
}