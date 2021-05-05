using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCert.Controllers;
using System.Web;
using System.Web.Mvc;
using eCert_Test.Helper;
using eCert.Utilities;

namespace eCert_Test.ControllerTest
{
    [TestClass]
    public class SuperAdminControllerTest
    {

        private SuperAdminController _saController;
        private MockHelper _mockHelper;


        [TestInitialize]
        public void Init()
        {
            _mockHelper = new MockHelper();
            _saController = new SuperAdminController();
            HttpContext.Current = _mockHelper.FakeHttpContext();
        }
        [TestMethod]

        public void Index_No_Login()
        {
            HttpContext.Current.Session["RoleName"] = null;

            var result = _saController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]

        public void Index_Login_As_SAdmin()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.SUPER_ADMIN;

            var result = _saController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]

        public void ManageSignature_No_Login()
        {
            HttpContext.Current.Session["RoleName"] = null;

            var result = _saController.ManageSignaturee(HttpContext.Current) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]

        public void ManageSignature_No_Login_Login_As_SAdmin()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.SUPER_ADMIN;

            var result = _saController.ManageSignaturee(HttpContext.Current) as ViewResult;

            Assert.AreEqual("ManageSignature", result.ViewName);
        }

        [TestMethod]

        public void AddAcademicService_No_Login()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]

        public void AddAcademicService_Login_As_SAdmin()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]

        public void AddAdmin_No_Login()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]

        public void AddAdmin_Login_As_SAdmin()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]

        public void AddCampus_No_Login()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]

        public void AddCampus_Login_As_SAdmin()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]

        public void Load_All_Admin()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]

        public void Load_All_AcademicService()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
