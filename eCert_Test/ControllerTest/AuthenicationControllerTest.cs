using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCert.Models.ViewModel;
using eCert.Controllers;
using System.Web.Mvc;
using Moq;
using System.Web;
using System.Web.Routing;

namespace eCert_Test.ControllerTest
{
    [TestClass]
    public class AuthenicationControllerTest
    {
        private AuthenticationController _authenController;
  

        [TestInitialize]
        public void Init()
        {
            _authenController = new AuthenticationController();
            
        }

        [TestMethod]
        public void Index_Page_No_Login()
        {
            //Arrange
            ViewResult viewResultArrange = new ViewResult
            {
                ViewName = "Index"
            };

            //Act
            var viewResultAct = _authenController.Index() as ViewResult;

            //Assert
            Assert.AreEqual("Index", viewResultAct.ViewName);
        }
    }
}
