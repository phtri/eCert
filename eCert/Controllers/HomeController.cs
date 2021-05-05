using eCert.Models.ViewModel;
using eCert.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCert.Controllers
{
    public class HomeController : Controller
    {
        private readonly CertificateServices _certificateServices;
        public HomeController()
        {
            _certificateServices = new CertificateServices();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Share(string url)
        {
            ViewBag.Title = "FPT Certificate Detail";
            CertificateViewModel certViewModel = _certificateServices.GetCertificateByUrl(url);
            return View(certViewModel);
        }


        public ActionResult About()
        {
            return View();
        }
        public ActionResult Certificate()
        {
            return View();
        }
    }
}