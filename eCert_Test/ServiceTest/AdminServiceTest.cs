using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCert.Services;
using eCert.Models.ViewModel;
using eCert.AutoMapperConfig;
using System.Collections.Generic;
using eCert.Utilities;

namespace eCert_Test.ServiceTest
{
    [TestClass]
    public class AdminServiceTest
    {

        private AdminServices _adminService;
        [TestInitialize]
        public void Initialize()
        {
            _adminService = new AdminServices();
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
        }

        [TestMethod]
        public void Mapping_List_Education_System_EducationSystemViewModel_Quantity()
        {
            //Arrange
            List<EducationSystemViewModel> listArr = new List<EducationSystemViewModel>()
            {
                new EducationSystemViewModel() {EducationName = "Đại học FPT Greenwich"},
            };

            //Act
            int userID = 178; //Admin
            List<EducationSystemViewModel> listAct = _adminService.GetEducationSystem(userID);

            //Assert
            Assert.AreEqual(listArr.Count, listAct.Count);
        }

        [TestMethod]
        public void Mapping_List_Campus_CampusViewModel_Quantity()
        {
            //Arrange
            List<CampusViewModel> listArr = new List<CampusViewModel>()
            {
                new CampusViewModel() {CampusName = "Campus Hà Nội"},
            };

            //Act
            int userID = 178; //Admin
            int eduSysID = 58;
            List<CampusViewModel> listAct = _adminService.GetCampusByUserId(userID, eduSysID);

            //Assert
            Assert.AreEqual(listArr.Count, listAct.Count);
        }

        [TestMethod]
        public void Mapping_List_Signature_SignatureViewModel_Quantity()
        {
            //Arrange
            List<SignatureViewModel> listArr = new List<SignatureViewModel>()
            {
                new SignatureViewModel() {FullName = "Bach Hoang"},
                new SignatureViewModel() {FullName = "Ha Pham"},
                new SignatureViewModel() {FullName = "2121@"},
                new SignatureViewModel() {FullName = "Ha Pham"},
                new SignatureViewModel() {FullName = "Hoàng Việt Bách"},
            };

            //Act
            int eduSysID = 58;
            List<SignatureViewModel> listAct = _adminService.GetSignatireByEduId(eduSysID);

            //Assert
            Assert.AreEqual(listArr.Count, listAct.Count);
        }

        [TestMethod]
        public void Import_Excel_Excel_File_Is_Null()
        {
            //Arrange
            ResultExcel resultArr = new ResultExcel()
            {
                IsSuccess = false,
                Message = "There is no file to upload."
            };

            //Act
            ResultExcel resultAct = _adminService.ImportCertificatesByExcel(null, "", 0, 0, 0);

            //Assert
            Assert.AreEqual(resultArr.IsSuccess, resultAct.IsSuccess);
        }

    }
}
