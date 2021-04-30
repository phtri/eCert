using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCert.Controllers;
using System.Web;
using System.Web.Mvc;
using eCert_Test.Helper;
using eCert.Utilities;

namespace eCert_Test.ControllerTest
{
    [TestClass]
    public class AuthenicationControllerTest
    {
        private AuthenticationController _authenController;
        private MockHelper _mockHelper;


        [TestInitialize]
        public void Init()
        {
            _mockHelper = new MockHelper();
            _authenController = new AuthenticationController();
            HttpContext.Current = _mockHelper.FakeHttpContext();
        }

        [TestMethod]
        public void Index_Page_No_Login()
        {
            HttpContext.Current.Session["RoleName"] = null;
            HttpContext.Current.Session["RollNumber"] = null;

            var result = _authenController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Authentication/Index.cshtml", result.ViewName);
        }

        [TestMethod]
        public void Index_Page_Login_As_Admin()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.ADMIN;
            HttpContext.Current.Session["RollNumber"] = null;

            var result = _authenController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Admin/Index.cshtml", result.ViewName);

        }

        [TestMethod]
        public void Index_Page_Login_As_SuperAdmin()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.SUPER_ADMIN;
            HttpContext.Current.Session["RollNumber"] = null;

            var result = _authenController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/SuperAdmin/Index.cshtml", result.ViewName);

        }

        [TestMethod]
        public void Index_Page_Login_As_AcademicService()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.FPT_UNIVERSITY_ACADEMIC;
            HttpContext.Current.Session["RollNumber"] = null;

            var result = _authenController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/AcademicService/Index.cshtml", result.ViewName);

        }

        [TestMethod]
        public void Index_Page_Login_As_Owner_Not_Update_Personal_Email()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.OWNER;
            HttpContext.Current.Session["RollNumber"] = "HE130576";
            HttpContext.Current.Session["isUpdatedEmail"] = false;

            var result = _authenController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Authentication/UpdatePersonalEmail.cshtml", result.ViewName);

        }

        [TestMethod]
        public void Index_Page_Login_As_Owner_Update_Personal_Email()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.OWNER;
            HttpContext.Current.Session["RollNumber"] = "HE130576";
            HttpContext.Current.Session["isUpdatedEmail"] = true;

            var result = _authenController.Indexx(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Certificate/Index.cshtml", result.ViewName);

        }

        [TestMethod]
        public void Update_Personal_Email_Role_Owner()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.OWNER;
            HttpContext.Current.Session["RollNumber"] = "HE130576";
            HttpContext.Current.Session["isUpdatedEmail"] = false;

            //HttpContext.Current.Session["RoleId"] = null;

            var result = _authenController.UpdatePersonalEmaill(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Authentication/UpdatePersonalEmail.cshtml", result.ViewName);
        }

        [TestMethod]
        public void Update_Personal_Email_Role_Owner_Not_Verify_Email()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.OWNER;
            HttpContext.Current.Session["RollNumber"] = "HE130576";
            HttpContext.Current.Session["isUpdatedEmail"] = true;
            HttpContext.Current.Session["isVerifyMail"] = false;

            var result = _authenController.UpdatePersonalEmaill(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Authentication/UpdatePersonalEmail.cshtml", result.ViewName);
        }

        [TestMethod]
        public void Update_Personal_Email_Role_Owner_Verify_Email()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.OWNER;
            HttpContext.Current.Session["RollNumber"] = "HE130576";
            HttpContext.Current.Session["isUpdatedEmail"] = true;
            HttpContext.Current.Session["isVerifyMail"] = true;

            var result = _authenController.UpdatePersonalEmaill(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Certifcate/Index.cshtml", result.ViewName);
        }

        [TestMethod]
        public void Update_Personal_Email_Role_Admin()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.ADMIN;
            HttpContext.Current.Session["RollNumber"] = null;
            HttpContext.Current.Session["RoleId"] = 123;

            var result = _authenController.UpdatePersonalEmaill(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/Admin/Index.cshtml", result.ViewName);

        }

        [TestMethod]
        public void Update_Personal_Email_Role_AcademicService()
        {
            HttpContext.Current.Session["RoleName"] = Constants.Role.FPT_UNIVERSITY_ACADEMIC;
            HttpContext.Current.Session["RollNumber"] = null;
            HttpContext.Current.Session["RoleId"] = 123;

            var result = _authenController.UpdatePersonalEmaill(HttpContext.Current) as ViewResult;

            Assert.AreEqual("~/Views/AcademicService/Index.cshtml", result.ViewName);

        }

        [TestMethod]
        public void Change_Password_Not_Update_Email()
        {
            HttpContext.Current.Session["RollNumber"] = Constants.Role.OWNER;
            HttpContext.Current.Session["isUpdatedEmail"] = false;

            var result = _authenController.ChangePasswordd(HttpContext.Current) as ViewResult;

            Assert.AreEqual("UpdatePersonalEmail", result.ViewName);
        }

        [TestMethod]
        public void Change_Password_Not_Login()
        {
            HttpContext.Current.Session["RollNumber"] = null;
            HttpContext.Current.Session["isUpdatedEmail"] = false;

            var result = _authenController.ChangePasswordd(HttpContext.Current) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Change_Password_Updated_Email()
        {
            HttpContext.Current.Session["RollNumber"] = Constants.Role.OWNER;
            HttpContext.Current.Session["isUpdatedEmail"] = true;

            var result = _authenController.ChangePasswordd(HttpContext.Current) as ViewResult;

            Assert.AreEqual("ChangePassword", result.ViewName);
        }

        [TestMethod]
        public void Reset_Password_No_Login()
        {
            HttpContext.Current.Session["RollNumber"] = null;

            var result = _authenController.ResetPasswordd(HttpContext.Current) as ViewResult;

            Assert.AreEqual("ResetPassword", result.ViewName);
        }

        [TestMethod]
        public void Reset_Password_Login()
        {
            HttpContext.Current.Session["RollNumber"] = Constants.Role.OWNER;

            var result = _authenController.ResetPasswordd(HttpContext.Current) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
