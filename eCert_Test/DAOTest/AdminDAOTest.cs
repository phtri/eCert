using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using eCert.Daos;
using eCert.Models.Entity;
using eCert.Utilities;
using System.Collections.Generic;

namespace eCert_Test.DAOTest
{
    [TestClass]
    public class AdminDAOTest
    {
        private AdminDAO adminDAO;

        [TestInitialize]
        public void Init()
        {
            adminDAO = new AdminDAO();
        }

        [TestMethod]
        public void Quantity_Of_Education_System_Get_By_Admin()
        {
            //Arrange
            int userID = 108; //User: Admin
            List<EducationSystem> listEducationSystemArr = new List<EducationSystem>()
            {
                new EducationSystem() { EducationName = "Đại học FPT Greenwich"},
            };

            //Actual
            List<EducationSystem> listEducationSystemAct = adminDAO.GetEducationSystem(userID);

            //Assert
            Assert.AreEqual(listEducationSystemArr.Count, listEducationSystemAct.Count);
        }

        [TestMethod]
        public void First_Item_Of_Education_System_Get_By_Admin()
        {
            //Arrange
            int userID = 108; //User: Admin
            List<EducationSystem> listEducationSystemArr = new List<EducationSystem>()
            {
                new EducationSystem() { EducationName = "Đại học FPT Greenwich"},
            };

            //Actual
            List<EducationSystem> listEducationSystemAct = adminDAO.GetEducationSystem(userID);

            //Assert
            Assert.AreEqual(listEducationSystemArr[0].EducationName, listEducationSystemAct[0].EducationName);
        }

        [TestMethod]
        public void Quantity_Of_Education_System_Get_By_SuperAdmin()
        {
            //Arrange
            int userID = 83; //User: Super Admin
            List<EducationSystem> listEducationSystemArr = new List<EducationSystem>()
            {
                new EducationSystem() { EducationName = "Đại học FPT Greenwich"},
                new EducationSystem() { EducationName = "Đại học FPT"},
            };

            //Actual
            List<EducationSystem> listEducationSystemAct = adminDAO.GetEducationSystem(userID);

            //Assert
            Assert.AreEqual(listEducationSystemArr.Count, listEducationSystemAct.Count);
        }

        [TestMethod]
        public void First_Item_Of_Education_System_Get_By_SuperAdmin()
        {
            //Arrange
            int userID = 83; //User: Super Admin
            List<EducationSystem> listEducationSystemArr = new List<EducationSystem>()
            {
                new EducationSystem() { EducationName = "Đại học FPT Greenwich"},
                new EducationSystem() { EducationName = "Đại học FPT"},
            };

            //Actual
            List<EducationSystem> listEducationSystemAct = adminDAO.GetEducationSystem(userID);

            //Assert
            Assert.AreEqual(listEducationSystemArr[0].EducationName, listEducationSystemAct[0].EducationName);
        }

        [TestMethod]
        public void Quantity_Of_Education_System_Get_By_AcademicService()
        {
            //Arrange
            int userID = 111; //User: Academic Service
            List<EducationSystem> listEducationSystemArr = new List<EducationSystem>()
            {
                new EducationSystem() { EducationName = "Đại học FPT Greenwich"},
                new EducationSystem() { EducationName = "Đại học FPT"},
            };

            //Actual
            List<EducationSystem> listEducationSystemAct = adminDAO.GetEducationSystem(userID);

            //Assert
            Assert.AreEqual(listEducationSystemArr.Count, listEducationSystemAct.Count);
        }

        [TestMethod]
        public void First_Item_Of_Education_System_Get_By_AcademicService()
        {
            //Arrange
            int userID = 111; //User: Academic Service
            List<EducationSystem> listEducationSystemArr = new List<EducationSystem>()
            {
                new EducationSystem() { EducationName = "Đại học FPT Greenwich"},
                new EducationSystem() { EducationName = "Đại học FPT"},
            };

            //Actual
            List<EducationSystem> listEducationSystemAct = adminDAO.GetEducationSystem(userID);

            //Assert
            Assert.AreEqual(listEducationSystemArr[0].EducationName, listEducationSystemAct[0].EducationName);
        }

        [TestMethod]
        public void Quantity_Of_Signature_Get_By_FPTUniversity()
        {
            //Arrange
            int eduSystemID = 12; //FPT University
            List<Signature> listArr = new List<Signature>()
            {
                new Signature() { FullName = "Bach Hoang", Position = "Hieu truong"}
            };

            //Actual
            List<Signature> listAct = adminDAO.GetSignatireByEduId(eduSystemID);

            //Assert
            Assert.AreEqual(listArr.Count, listAct.Count);
        }

        [TestMethod]
        public void Quantity_Of_Signature_Get_By_FPTGreenwich()
        {
            //Arrange
            int eduSystemID = 12; //FPT University
            List<Signature> listArr = new List<Signature>()
            {
                new Signature() { FullName = "Bach Hoang", Position = "Hieu truong"}
            };

            //Actual
            List<Signature> listAct = adminDAO.GetSignatireByEduId(eduSystemID);

            //Assert
            Assert.AreEqual(listArr.Count, listAct.Count);
        }

        [TestMethod]
        public void Quantity_Of_List_Campus_Of_FPTUniversity_By_Admin()
        {
            //Arrange
            int eduSystemID = 12; //FPT University
            int userID = 108; //Admin
            List<Campus> listArr = new List<Campus>()
            {
                new Campus() { CampusName = "Campus Hà Nội"}
            };

            //Actual
            List<Campus> listAct = adminDAO.GetListCampusByUserId(userID, eduSystemID);

            //Assert
            Assert.AreEqual(listArr.Count, listAct.Count);
        }

        [TestMethod]
        public void First_Item_Of_List_Campus_Of_FPTUniversity_By_Admin()
        {
            //Arrange
            int eduSystemID = 12; //FPT University
            int userID = 108; //Admin
            List<Campus> listArr = new List<Campus>()
            {
                new Campus() { CampusName = "Campus Hà Nội"}
            };

            //Actual
            List<Campus> listAct = adminDAO.GetListCampusByUserId(userID, eduSystemID);

            //Assert
            Assert.AreEqual(listArr[0].CampusName, listAct[0].CampusName);
        }

        [TestMethod]
        public void Quantity_Of_List_Campus_Of_FPTUniversity_By_SuperAdmin()
        {
            //Arrange
            int eduSystemID = 12; //FPT University
            int userID = 83; //Super Admin
            List<Campus> listArr = new List<Campus>()
            {
                new Campus() { CampusName = "Campus Hà Nội"}
            };

            //Actual
            List<Campus> listAct = adminDAO.GetListCampusByUserId(userID, eduSystemID);

            //Assert
            Assert.AreEqual(listArr.Count, listAct.Count);
        }

        [TestMethod]
        public void First_Item_Of_List_Campus_Of_FPTUniversity_By_SuperAdmin()
        {
            //Arrange
            int eduSystemID = 12; //FPT University
            int userID = 83; //Super Admin
            List<Campus> listArr = new List<Campus>()
            {
                new Campus() { CampusName = "Campus Hà Nội"}
            };

            //Actual
            List<Campus> listAct = adminDAO.GetListCampusByUserId(userID, eduSystemID);

            //Assert
            Assert.AreEqual(listArr[0].CampusName, listAct[0].CampusName);
        }

        [TestMethod]
        public void Quantity_Of_List_Campus_Of_FPTUniversity_By_AcademicService()
        {
            //Arrange
            int eduSystemID = 12; //FPT University
            int userID = 111; //Admin
            List<Campus> listArr = new List<Campus>()
            {
                new Campus() { CampusName = "Campus Hà Nội"}
            };

            //Actual
            List<Campus> listAct = adminDAO.GetListCampusByUserId(userID, eduSystemID);

            //Assert
            Assert.AreEqual(listArr.Count, listAct.Count);
        }

        [TestMethod]
        public void First_Item_Of_List_Campus_Of_FPTUniversity_By_AcademicService()
        {
            //Arrange
            int eduSystemID = 12; //FPT University
            int userID = 111; //Admin
            List<Campus> listArr = new List<Campus>()
            {
                new Campus() { CampusName = "Campus Hà Nội"}
            };

            //Actual
            List<Campus> listAct = adminDAO.GetListCampusByUserId(userID, eduSystemID);

            //Assert
            Assert.AreEqual(listArr[0].CampusName, listAct[0].CampusName);
        }

        [TestMethod]
        public void Quantity_Of_List_Campus_Of_FPTGreenwich_By_Admin()
        {
            //Arrange
            int eduSystemID = 13; //FPT University
            int userID = 108; //Admin
            List<Campus> listArr = new List<Campus>()
            {
                new Campus() { CampusName = "Campus Hà Nội"}
            };

            //Actual
            List<Campus> listAct = adminDAO.GetListCampusByUserId(userID, eduSystemID);

            //Assert
            Assert.AreEqual(listArr.Count, listAct.Count);
        }

        [TestMethod]
        public void First_Item_Of_List_Campus_Of_FPTGreenwich_By_Admin()
        {
            //Arrange
            int eduSystemID = 13; //FPT University
            int userID = 108; //Admin
            List<Campus> listArr = new List<Campus>()
            {
                new Campus() { CampusName = "Campus Hà Nội"}
            };

            //Actual
            List<Campus> listAct = adminDAO.GetListCampusByUserId(userID, eduSystemID);

            //Assert
            Assert.AreEqual(listArr[0].CampusName, listAct[0].CampusName);
        }

        [TestMethod]
        public void Quantity_Of_List_Campus_Of_FPTGreenwich_By_SuperAdmin()
        {
            //Arrange
            int eduSystemID = 13; //FPT University
            int userID = 83; //Super Admin
            List<Campus> listArr = new List<Campus>()
            {
                new Campus() { CampusName = "Campus Hà Nội"}
            };

            //Actual
            List<Campus> listAct = adminDAO.GetListCampusByUserId(userID, eduSystemID);

            //Assert
            Assert.AreEqual(listArr.Count, listAct.Count);
        }

        [TestMethod]
        public void First_Item_Of_List_Campus_Of_FPTGreenwich_By_SuperAdmin()
        {
            //Arrange
            int eduSystemID = 13; //FPT University
            int userID = 83; //Super Admin
            List<Campus> listArr = new List<Campus>()
            {
                new Campus() { CampusName = "Campus Hà Nội"}
            };

            //Actual
            List<Campus> listAct = adminDAO.GetListCampusByUserId(userID, eduSystemID);

            //Assert
            Assert.AreEqual(listArr[0].CampusName, listAct[0].CampusName);
        }

        [TestMethod]
        public void Quantity_Of_List_Campus_Of_FPTGreenwich_By_AcademicService()
        {
            //Arrange
            int eduSystemID = 13; //FPT University
            int userID = 111; //Academic Service
            List<Campus> listArr = new List<Campus>()
            {
                new Campus() { CampusName = "Campus Hà Nội"}
            };

            //Actual
            List<Campus> listAct = adminDAO.GetListCampusByUserId(userID, eduSystemID);

            //Assert
            Assert.AreEqual(listArr.Count, listAct.Count);
        }

        [TestMethod]
        public void First_Item_Of_List_Campus_Of_FPTGreenwich_By_AcademicService()
        {
            //Arrange
            int eduSystemID = 13; //FPT University
            int userID = 111; //Academic Service
            List<Campus> listArr = new List<Campus>()
            {
                new Campus() { CampusName = "Campus Hà Nội"}
            };

            //Actual
            List<Campus> listAct = adminDAO.GetListCampusByUserId(userID, eduSystemID);

            //Assert
            Assert.AreEqual(listArr[0].CampusName, listAct[0].CampusName);
        }
    }
}
