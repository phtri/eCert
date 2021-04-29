using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCert.Services;
using eCert.Models.ViewModel;
using eCert.Utilities;
using eCert.AutoMapperConfig;
using System.Collections.Generic;
using System.Web;

namespace eCert_Test.ServiceTest
{
    [TestClass]
    public class SuperAdminServiceTest
    {
        private SuperAdminServices _saService;

        [TestInitialize]
        public void Initialize()
        {
            _saService = new SuperAdminServices();
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
        }

        [TestMethod]
        public void Get_Admin_Pagination_With_Page_Size_2_Page_Number_1()
        {
            //Arrange 
            List<StaffViewModel> listArr = new List<StaffViewModel>() {
                new StaffViewModel() { AcademicEmail = "trihphe130589@fpt.edu.vn",},
                new StaffViewModel() { AcademicEmail = "tuannmhe130642@fpt.edu.vn"},
                new StaffViewModel() { AcademicEmail = "huynnhe130303@fpt.edu.vn"},
            };
            Pagination<StaffViewModel> paginationArr = new Pagination<StaffViewModel>().GetPagination(listArr, 2, 1);

            //Act
            Pagination<StaffViewModel> paginationAct = _saService.GetAdminPagination(2, 1);

            //Assert
            Assert.AreEqual(paginationArr.ReturnFirstItem().AcademicEmail, paginationAct.ReturnFirstItem().AcademicEmail);
        }

        [TestMethod]
        public void Get_Admin_Pagination_With_Page_Size_2_Page_Number_2()
        {
            //Arrange 
            List<StaffViewModel> listArr = new List<StaffViewModel>() {
                new StaffViewModel() { AcademicEmail = "trihphe130589@fpt.edu.vn",},
                new StaffViewModel() { AcademicEmail = "tuannmhe130642@fpt.edu.vn"},
                new StaffViewModel() { AcademicEmail = "huynnhe130303@fpt.edu.vn"},
            };
            Pagination<StaffViewModel> paginationArr = new Pagination<StaffViewModel>().GetPagination(listArr, 2, 2);

            //Act
            Pagination<StaffViewModel> paginationAct = _saService.GetAdminPagination(2, 2);

            //Assert
            Assert.AreEqual(paginationArr.ReturnFirstItem().AcademicEmail, paginationAct.ReturnFirstItem().AcademicEmail);
        }

        [TestMethod]
        public void Get_AcademicService_Pagination_With_Page_Size_1_Page_Number_1()
        {
            //Arrange 
            List<StaffViewModel> listArr = new List<StaffViewModel>() {
                new StaffViewModel() { AcademicEmail = "bachhvhe130603@fpt.edu.vn"},
                new StaffViewModel() { AcademicEmail = "huynnhe130595@fpt.edu.vn"},
            };
            Pagination<StaffViewModel> paginationArr = new Pagination<StaffViewModel>().GetPagination(listArr, 1, 1);

            //Act
            Pagination<StaffViewModel> paginationAct = _saService.GetAcaServicePagination(1, 1);

            //Assert
            Assert.AreEqual(paginationArr.ReturnFirstItem().AcademicEmail, paginationAct.ReturnFirstItem().AcademicEmail);
        }

        [TestMethod]
        public void Get_AcademicService_Pagination_With_Page_Size_1_Page_Number_2()
        {
            //Arrange 
            List<StaffViewModel> listArr = new List<StaffViewModel>() {
                new StaffViewModel() { AcademicEmail = "bachhvhe130603@fpt.edu.vn"},
                new StaffViewModel() { AcademicEmail = "huynnhe130595@fpt.edu.vn"},
            };
            Pagination<StaffViewModel> paginationArr = new Pagination<StaffViewModel>().GetPagination(listArr, 1, 2);

            //Act
            Pagination<StaffViewModel> paginationAct = _saService.GetAcaServicePagination(1, 2);

            //Assert
            Assert.AreEqual(paginationArr.ReturnFirstItem().AcademicEmail, paginationAct.ReturnFirstItem().AcademicEmail);
        }

        [TestMethod]
        public void Get_Campus_Pagination_With_Page_Size_1_Page_Number_1()
        {
            //Arrange 
            List<CampusViewModel> listArr = new List<CampusViewModel>() {
                new CampusViewModel() { CampusName = "Campus Hà Nội"},
                new CampusViewModel() { CampusName = "Campus Hồ Chí Minh"},
            };
            Pagination<CampusViewModel> paginationArr = new Pagination<CampusViewModel>().GetPagination(listArr, 1, 1);

            //Act
            int eduSysID = 12;
            Pagination<CampusViewModel> paginationAct = _saService.GetListCampusById(1, 1, eduSysID);

            //Assert
            Assert.AreEqual(paginationArr.ReturnFirstItem().CampusName, paginationAct.ReturnFirstItem().CampusName);
        }

        [TestMethod]
        public void Get_Campus_Pagination_With_Page_Size_1_Page_Number_2()
        {
            //Arrange 
            List<CampusViewModel> listArr = new List<CampusViewModel>() {
                new CampusViewModel() { CampusName = "Campus Hà Nội"},
                new CampusViewModel() { CampusName = "Campus Hồ Chí Minh"},
            };
            Pagination<CampusViewModel> paginationArr = new Pagination<CampusViewModel>().GetPagination(listArr, 1, 2);

            //Act
            int eduSysID = 12;
            Pagination<CampusViewModel> paginationAct = _saService.GetListCampusById(1, 2, eduSysID);

            //Assert
            Assert.AreEqual(paginationArr.ReturnFirstItem().CampusName, paginationAct.ReturnFirstItem().CampusName);
        }

        
       
    }
}
