using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCert.Daos;
using eCert.Models.Entity;
using eCert.Utilities;
using System.Collections.Generic;

namespace eCert_Test.DAOTest
{
    [TestClass]
    public class SuperAdminDAOTest
    {
        private SuperAdminDAO superAdminDAO;

        [TestInitialize]
        public void Init()
        {
            superAdminDAO = new SuperAdminDAO();
        }

        [TestMethod]
        public void Check_Quantity_Of_List_All_EducationSystem()
        {
            //Arrange
            int quantityArr = 10;

            //Actual
            List<EducationSystem> listEduSys = superAdminDAO.GetAllEducationSystem();

            //Assert
            Assert.AreEqual(quantityArr, listEduSys.Count);
        }

        [TestMethod]
        public void First_Item_Of_List_All_EducationSystem()
        {
            //Arrange
            string firstItemName = "Đại học FPT Greenwich";

            //Actual
            List<EducationSystem> listEduSys = superAdminDAO.GetAllEducationSystem();

            //Assert
            Assert.AreEqual(firstItemName, listEduSys[0].EducationName);
        }

        [TestMethod]
        public void Check_Quantity_Of_List_All_Signature()
        {
            //Arrange
            int quantityArr = 6;

            //Actual
            List<Signature> listSignature = superAdminDAO.GetAllSignature();

            //Assert
            Assert.AreEqual(quantityArr, listSignature.Count);
        }

        [TestMethod]
        public void First_Item_Of_List_All_Signature()
        {
            //Arrange
            string firstItemName = "Bach Hoang";

            //Actual
            List<Signature> listSignature = superAdminDAO.GetAllSignature();

            //Assert
            Assert.AreEqual(firstItemName, listSignature[0].FullName);
        }

        [TestMethod]
        public void Check_Quantity_Of_List_Campus_Get_By_EducationSystem()
        {
            //Arrange
            int quantityArr = 2;
            int eduSystemID = 12; //FPT University

            //Actual
            List<Campus> listCampus = superAdminDAO.GetListCampusById(eduSystemID);

            //Assert
            Assert.AreEqual(quantityArr, listCampus.Count);
        }

        [TestMethod]
        public void First_Item_Of_List_Campus_Get_By_EducationSystem()
        {
            //Arrange
            string firstItemName = "Campus Hà Nội";
            int eduSystemID = 12; //FPT University

            //Actual
            List<Campus> listCampus = superAdminDAO.GetListCampusById(eduSystemID);

            //Assert
            Assert.AreEqual(firstItemName, listCampus[0].CampusName);
        }

        [TestMethod]
        public void Check_Quantity_Of_List_Admin()
        {
            //Arrange
            int quantityArr = 3;

            //Actual
            List<Staff> listAdmin = superAdminDAO.GetAllAdmin();

            //Assert
            Assert.AreEqual(quantityArr, listAdmin.Count);
        }

        [TestMethod]
        public void Check_Role_Of_First_Item_Is_Admin_Or_Not()
        {
            //Arrange
            int roleIDArr = 296; //Role ID of Admin

            //Actual
            List<Staff> listAdmin = superAdminDAO.GetAllAdmin();

            //Assert
            Assert.AreEqual(roleIDArr, listAdmin[0].RoleId);
        }

        [TestMethod]
        public void Check_Quantity_Of_List_AcademicService()
        {
            //Arrange
            int quantityArr = 2;

            //Actual
            List<Staff> listAcademicService = superAdminDAO.GetAllAcaService();

            //Assert
            Assert.AreEqual(quantityArr, listAcademicService.Count);
        }

        [TestMethod]
        public void Check_Role_Of_First_Item_Is_AcademicService_Or_Not()
        {
            //Arrange
            int roleIDArr = 288; //Role ID of Academic Service

            //Actual
            List<Staff> listAcademicService = superAdminDAO.GetAllAcaService();

            //Assert
            Assert.AreEqual(roleIDArr, listAcademicService[0].RoleId);
        }

        [TestMethod]
        public void Count_Campus_By_Name()
        {
            //Arrange
            string campusName = "Campus Hà Nội";
            int countArr = 1;

            //Actual
            int countAct = superAdminDAO.GetCountCampusByName(campusName);

            //Assert 
            Assert.AreEqual(countArr, countAct);
        }

        [TestMethod]
        public void Count_Certificate_By_EducationSystem()
        {
            //Arrange
            int eduSysID = 12; //FPT University
            int countArr = 93;

            //Actual
            int countAct = superAdminDAO.GetCountCertificateByEdu(eduSysID);

            //Assert 
            Assert.AreEqual(countArr, countAct);
        }

        [TestMethod]
        public void Count_Certificate_By_Campus()
        {
            //Arrange
            int campusID = 23; //Campus Hà Nội
            int countArr = 93;

            //Actual
            int countAct = superAdminDAO.GetCountCertificateByCampus(campusID);

            //Assert 
            Assert.AreEqual(countArr, countAct);
        }


        [TestMethod]
        public void Count_Certificate_By_Signature()
        {
            //Arrange
            int signatureID = 9;
            int countArr = 0;

            //Actual
            int countAct = superAdminDAO.GetCountCertificateBySignature(signatureID);

            //Assert 
            Assert.AreEqual(countArr, countAct);
        }

        [TestMethod]
        public void Quantity_Of_List_Role_Get_By_Campus()
        {
            //Arrange
            int campusID = 23; //Campus Hà Nội
            int countArr = 3;

            //Actual
           List<Role> list = superAdminDAO.GetRoleByCampusId(campusID);

            //Assert 
            Assert.AreEqual(countArr, list.Count);
        }

        [TestMethod]
        public void Number_Of_User_Get_By_ID()
        {
            //Arrange
            int userID = 122;
            int countArr = 0;

            //Actual
            int countAct = superAdminDAO.GetNumberOfUser(userID);

            //Assert
            Assert.AreEqual(countArr, countAct);
        }
    }
}
