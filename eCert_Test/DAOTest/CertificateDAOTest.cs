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
            int quantityArr = 10;

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
            string firstItemName = "Software Architecture and Design";

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
            int quantityArr = 0;
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
            string firstItemName = "Software Architecture and Design";
            string keyword = "Software Architecture and Design";

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
            int userID = 176; //Roll number of owner
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
            int certID = 4479;

            //Actual
            Certificate cert = certDAO.GetCertificateById(certID);

            //Assert
            Assert.IsNotNull(cert);
        }

        [TestMethod]
        public void Certificate_Get_By_URL_Is_Exist()
        {
            //Arrange
            string url = "5c9f5f32-9cc5-4173-8105-83e22ce55e5e";

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
            string url = "5c9f5f32-9cc5-4173-8105-83e22ce55e5e";

            //Actual
            bool valid = certDAO.IsOwnerOfCertificate(rollNum, url);

            //Assert
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void Check_Content_Of_Certificate_Get_By_ID()
        {
            //Arrange
            int certID = 4479;
            string contentArr = @"HE130576\FU_EDU\5c9f5f32-9cc5-4173-8105-83e22ce55e5e\Imgs\75c22a96-8145-44d3-b871-a125e8e2942c.png";

            //Actual
            string contentAct = certDAO.GetCertificateContent(certID);

            //Assert
            Assert.AreEqual(contentArr, contentAct);
        }
    }
}
