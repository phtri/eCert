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
            int quantityArr = 73;

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
            int quantityArr = 13;
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
            int quantityArr = 0;
           
            //Actual
            List<Report> list = certDAO.GetAllReportById(userID);

            //Assert
            Assert.AreEqual(quantityArr, list.Count);
        }

        [TestMethod]
        public void First_Report_Get_By_Owner()
        {
            //Arrange
            int userID = 135; //Roll number of owner
            string reportContent = "adasdasdasdasd";

            //Actual
            List<Report> list = certDAO.GetAllReportById(userID);

            //Assert
            Assert.AreEqual(reportContent, list[0].ReportContent);
        }

        
        [TestMethod]
        public void Certificate_Get_By_ID_Is_Exist()
        {
            //Arrange
            int certID = 4062;

            //Actual
            Certificate cert = certDAO.GetCertificateById(certID);

            //Assert
            Assert.IsNotNull(cert);
        }

        [TestMethod]
        public void Certificate_Get_By_URL_Is_Exist()
        {
            //Arrange
            string url = "06e87c62-d57c-4904-9ed9-fbb67b38e8ec";

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
            string url = "06e87c62-d57c-4904-9ed9-fbb67b38e8ec";

            //Actual
            bool valid = certDAO.IsOwnerOfCertificate(rollNum, url);

            //Assert
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void Check_Content_Of_Certificate_Get_By_ID()
        {
            //Arrange
            int certID = 4062;
            string contentArr = @"HE130576\FU_EDU\06e87c62-d57c-4904-9ed9-fbb67b38e8ec\Imgs\8d9ab9e8-6870-47bb-ba17-d521738b18bc.png";

            //Actual
            string contentAct = certDAO.GetCertificateContent(certID);

            //Assert
            Assert.AreEqual(contentArr, contentAct);
        }
    }
}
