using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using eCert.Services;
using eCert.Models.ViewModel;
using eCert.Utilities;
using System;

namespace eCert_Test.ServiceTest
{
    [TestClass]
    public class CertificateServiceTest
    {
        private CertificateServices _certService;
        [TestInitialize]
        public void Init()
        {
            _certService = new CertificateServices();
        }

        [TestMethod]
        public void Validate_Certificate_Name_Is_Not_Null()
        {
            CertificateViewModel cvm = new CertificateViewModel()
            {
                CertificateName = null
            };

            Result r = _certService.ValidateCertificateInfor(cvm);

            Assert.AreEqual("The certificate name is required.", r.Message);
        }

        [TestMethod]
        public void Validate_Certificate_Expiry_Date_Ealier_Than_Issue_Date()
        {
            CertificateViewModel cvm = new CertificateViewModel()
            {
                CertificateName = ".NET C#",
                DateOfIssue = DateTime.Parse("2021/4/26"),
                DateOfExpiry = DateTime.Parse("2021/4/25")
            };
           

            Result r = _certService.ValidateCertificateInfor(cvm);

            Assert.AreEqual("Issue date have to be ealier than expiry date.", r.Message);
        }

        [TestMethod]
        public void Validate_Certificate_Issue_Date_Is_Future_Date()
        {
            CertificateViewModel cvm = new CertificateViewModel()
            {
                CertificateName = ".NET C#",
                DateOfExpiry = DateTime.Parse("2021/4/29"),
                DateOfIssue = DateTime.Parse("2021/4/28"),
            };


            Result r = _certService.ValidateCertificateInfor(cvm);

            Assert.AreEqual("Issue Date can not be in the future.", r.Message);
        }
    }
}
