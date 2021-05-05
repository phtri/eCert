using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCert.Controllers;
using System.Web;
using System.Web.Mvc;
using eCert_Test.Helper;
using eCert.Utilities;
using eCert.AutoMapperConfig;

namespace eCert_Test.ControllerTest
{
    [TestClass]
    public class CertificateControllerTest
    {

        private CertificateController _certController;
        private MockHelper _mockHelper;

        [TestInitialize]
        public void Init()
        {
            _mockHelper = new MockHelper();
            _certController = new CertificateController();
            HttpContext.Current = _mockHelper.FakeHttpContext();
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
        }

        [TestMethod]
        public void Index_No_Login()
        {
            HttpContext.Current.Session["RollNumber"] = null;
            HttpContext.Current.Session["isUpdatedEmail"] = false;

            var result = _certController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Index_Owner_Not_Update_Email()
        {
            HttpContext.Current.Session["RollNumber"] = Constants.Role.OWNER;
            HttpContext.Current.Session["isUpdatedEmail"] = false;

            var result = _certController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("UpdatePersonalEmail", result.ViewName);
        }

        [TestMethod]
        public void Index_Owner_Update_Email()
        {
            HttpContext.Current.Session["RollNumber"] = Constants.Role.OWNER;
            HttpContext.Current.Session["isUpdatedEmail"] = true;

            var result = _certController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void ListReport_No_Login()
        {
            HttpContext.Current.Session["RollNumber"] = null;
            HttpContext.Current.Session["isUpdatedEmail"] = false;

            var result = _certController.ListReportt(HttpContext.Current) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void ListReport_Owner_Not_Update_Email()
        {
            HttpContext.Current.Session["RollNumber"] = Constants.Role.OWNER;
            HttpContext.Current.Session["isUpdatedEmail"] = false;

            var result = _certController.ListReportt(HttpContext.Current) as ViewResult;

            Assert.AreEqual("UpdatePersonalEmail", result.ViewName);
        }

        [TestMethod]
        public void ListReport_Owner_Update_Email()
        {
            HttpContext.Current.Session["RollNumber"] = Constants.Role.OWNER;
            HttpContext.Current.Session["isUpdatedEmail"] = true;

            var result = _certController.ListReportt(HttpContext.Current) as ViewResult;

            Assert.AreEqual("ListReport", result.ViewName);
        }

        
    }
}
