using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCert.Daos;
using eCert.Models.Entity;
using eCert.Utilities;
using System.Collections.Generic;

namespace eCert_Test.DAOTest
{
    [TestClass]
    public class CertificateDAOTest
    {
        private CertificateDAO certDAO;

        [TestInitialize]
        public void Init()
        {
            certDAO = new CertificateDAO();
        }

        [TestMethod]
        public void Quantity_Of_List_Certificate_Without_Search()
        {
            //Arrange
            string rollNumber = "HE130576"; //Roll number of owner
            int quantityArr = 23;

            //Actual
            List<Certificate> list = certDAO.GetAllCertificates(rollNumber, null);

            //Assert
            Assert.AreEqual(quantityArr, list.Count);
        }

        [TestMethod]
        public void First_Item_Of_List_Certificate_Without_Search()
        {
            //Arrange
            string rollNumber = "HE130576"; //Roll number of owner
            string firstItemName = "Đàn tranh";

            //Actual
            List<Certificate> list = certDAO.GetAllCertificates(rollNumber, null);

            //Assert
            Assert.AreEqual(firstItemName, list[0].CertificateName);
        }

        [TestMethod]
        public void Quantity_Of_List_Certificate_With_Search_Keyword()
        {
            //Arrange
            string rollNumber = "HE130576"; //Roll number of owner
            int quantityArr = 4;
            string keyword = "Đàn tranh";

            //Actual
            List<Certificate> list = certDAO.GetAllCertificates(rollNumber, keyword);

            //Assert
            Assert.AreEqual(quantityArr, list.Count);
        }

        [TestMethod]
        public void First_Item_Of_List_Certificate_With_Search_Keyword()
        {
            //Arrange
            string rollNumber = "HE130576"; //Roll number of owner
            string firstItemName = "Đàn tranh";
            string keyword = "Đàn tranh";

            //Actual
            List<Certificate> list = certDAO.GetAllCertificates(rollNumber, keyword);

            //Assert
            Assert.AreEqual(firstItemName, list[0].CertificateName);
        }

        [TestMethod]
        public void Quantity_Of_List_Report_Get_By_Owner()
        {
            //Arrange
            int userID = 107; //Roll number of owner
            int quantityArr = 2;
           
            //Actual
            List<Report> list = certDAO.GetAllReportById(userID);

            //Assert
            Assert.AreEqual(quantityArr, list.Count);
        }

        [TestMethod]
        public void First_Report_Get_By_Owner()
        {
            //Arrange
            int userID = 107; //Roll number of owner
            string reportContent = "Tên report chưa đúng";

            //Actual
            List<Report> list = certDAO.GetAllReportById(userID);

            //Assert
            Assert.AreEqual(reportContent, list[0].ReportContent);
        }

        public void Quantity_Of_List_All_Report()
        {
            //Arrange
            int quantityArr = 2;

            //Actual
            List<Report> list = certDAO.GetAllReport();

            //Assert
            Assert.AreEqual(quantityArr, list.Count);
        }

        [TestMethod]
        public void First_Report_Of_List_All_Report()
        {
            //Arrange
            string reportContent = "Tên report chưa đúng";

            //Actual
            List<Report> list = certDAO.GetAllReport();

            //Assert
            Assert.AreEqual(reportContent, list[0].ReportContent);
        }

        [TestMethod]
        public void Certificate_Get_By_ID_Is_Exist()
        {
            //Arrange
            int certID = 481;

            //Actual
            Certificate cert = certDAO.GetCertificateById(certID);

            //Assert
            Assert.IsNotNull(cert);
        }

        [TestMethod]
        public void Certificate_Get_By_URL_Is_Exist()
        {
            //Arrange
            string url = "4e89a38d-79b8-4a3c-affd-f513e0186bc4";

            //Actual
            Certificate cert = certDAO.GetCertificateByUrl(url);

            //Assert
            Assert.IsNotNull(cert);
        }

        [TestMethod]
        public void Certificate_Get_By_Rollnumber_And_SubjectCode_Is_Exist()
        {
            //Arrange
            string rollNum = "HE130576";
            string subCode = "PRN292";

            //Actual
            Certificate cert = certDAO.GetCertificateByRollNumberAndSubjectCode(rollNum, subCode);

            //Assert
            Assert.IsNotNull(cert);
        }

        [TestMethod]
        public void Check_Owner_Has_Certificate_Or_Not()
        {
            //Arrange
            string rollNum = "HE130576";
            string url = "68d70994-04fd-4ba7-9f77-63b3b9792d9e";

            //Actual
            bool valid = certDAO.IsOwnerOfCertificate(rollNum, url);

            //Assert
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void Check_Content_Of_Certificate_Get_By_ID()
        {
            //Arrange
            int certID = 486;
            string contentArr = @"HE130576\FU_EDU\68d70994-04fd-4ba7-9f77-63b3b9792d9e\Imgs\8a9d6f4f-5f65-475c-a12a-5eacae9ce1cf.png";

            //Actual
            string contentAct = certDAO.GetCertificateContent(certID);

            //Assert
            Assert.AreEqual(contentArr, contentAct);
        }
    }
}
