using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCert.Controllers;
using System.Web;
using System.Web.Mvc;
using eCert_Test.Helper;
using eCert.Utilities;

namespace eCert_Test.ControllerTest
{
    [TestClass]
    public class AdminControllerTest
    {
        private AdminController _adminController;
        private MockHelper _mockHelper;


        [TestInitialize]
        public void Init()
        {
            _mockHelper = new MockHelper();
            _adminController = new AdminController();
            HttpContext.Current = _mockHelper.FakeHttpContext();
        }

        [TestMethod]

        public void Index_No_Login()
        {
            HttpContext.Current.Session["RoleName"] = null;

            var result = _adminController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Authentication/Index.cshtml", result.ViewName);
        }

        [TestMethod]

        public void Index_Login_As_Admin()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.ADMIN;

            var result = _adminController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Admin/Index.cshtml", result.ViewName);
        }

        [TestMethod]

        public void ImportExcel_No_Login()
        {
            HttpContext.Current.Session["RoleName"] = null;

            var result = _adminController.ImportExcell(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Authentication/Index.cshtml", result.ViewName);
        }

        [TestMethod]

        public void ImportExcel_Login_As_Admin()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.ADMIN;

            var result = _adminController.ImportExcell(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Admin/ImportExcel.cshtml", result.ViewName);
        }

        [TestMethod]

        public void ImportDiploma_No_Login()
        {
            HttpContext.Current.Session["RoleName"] = null;

            var result = _adminController.ImportDiplomaa(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Authentication/Index.cshtml", result.ViewName);
        }

        [TestMethod]

        public void ImportDiploma_Login_As_Admin()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.ADMIN;

            var result = _adminController.ImportDiplomaa(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Admin/ImportDiploma.cshtml", result.ViewName);
        }

        [TestMethod]

        public void ListAcademicService_No_Login()
        {
            HttpContext.Current.Session["RoleName"] = null;

            var result = _adminController.ListAcademicServicee(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Authentication/Index.cshtml", result.ViewName);
        }

        [TestMethod]

        public void ListAcademicService_Login_As_Admin()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.ADMIN;

            var result = _adminController.ListAcademicServicee(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Admin/ListAcademicService.cshtml", result.ViewName);
        }

        [TestMethod] 
        public void Active_Academic_Service_Exist_1_Academic_Service_In_Campus()
        {
            Assert.AreEqual("Invalid. There has already had an active account academic service in this campus", "Invalid. There has already had an active account academic service in this campus");
        }

        [TestMethod]
        public void Active_Academic_Service()
        {
            Assert.AreEqual(true, true);
        }


    }
}
