using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCert.Services;
using eCert.Models.ViewModel;
using eCert.Utilities;
using eCert.AutoMapperConfig;

namespace eCert_Test.ServiceTest
{
    [TestClass]
    public class UserServiceTest
    {
        private UserServices _userService;
        
        [TestInitialize]
        public void Initialize()
        {
            _userService = new UserServices();
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
        }

        [TestMethod]
        public void Mappping_User_UserViewModel_Get_User_By_Academic_Email()
        {
            //Arrange
            UserViewModel uvmArr = new UserViewModel()
            {
                AcademicEmail = "tuannmhe130642@fpt.edu.vn",
                Role = new RoleViewModel() { RoleName = Constants.Role.ADMIN }
            };

            //Act
            UserViewModel uvmAct = _userService.GetUserByAcademicEmail("tuannmhe130642@fpt.edu.vn");

            //Assert
            Assert.AreEqual(uvmArr.AcademicEmail, uvmAct.AcademicEmail);
        }

        [TestMethod]
        public void UserViewModel_Has_Right_Role_Or_Not()
        {
            //Arrange
            UserViewModel uvmArr = new UserViewModel()
            {
                AcademicEmail = "tuannmhe130642@fpt.edu.vn",
                Role = new RoleViewModel() { RoleName = Constants.Role.ADMIN }
            };

            //Act
            UserViewModel uvmAct = _userService.GetUserByAcademicEmail("tuannmhe130642@fpt.edu.vn");

            //Assert
            Assert.AreEqual(uvmArr.Role.RoleName, uvmAct.Role.RoleName);
        }

        

        [TestMethod]
        public void Mappping_User_UserViewModel_Get_Active_Admin_By_CampusID()
        {
            //Arrange
            UserViewModel uvmArr = new UserViewModel()
            {
                AcademicEmail = "tuannmhe130642@fpt.edu.vn",
                Role = new RoleViewModel() { RoleName = Constants.Role.ADMIN }
            };
            int campusID = 46;
            

            //Act
            UserViewModel uvmAct = _userService.GetActiveAdminByCampusId(campusID);

            //Assert
            Assert.AreEqual(uvmArr.AcademicEmail, uvmAct.AcademicEmail);
        }

        [TestMethod]
        public void UserViewModel_Has_Role_Admin_Or_Not()
        {
            //Arrange
            UserViewModel uvmArr = new UserViewModel()
            {
                AcademicEmail = "tuannmhe130642@fpt.edu.vn",
                Role = new RoleViewModel() { RoleName = Constants.Role.ADMIN }
            };
            int campusID = 46;
            string acaEmail = "tuannmhe130642@fpt.edu.vn";

            //Act
            UserViewModel uvmAct = _userService.GetActiveAdminByCampusId(campusID, acaEmail);

            //Assert
            Assert.AreEqual(uvmArr.Role.RoleName, uvmAct.Role.RoleName);
        }

        [TestMethod]
        public void Mappping_User_UserViewModel_Get_Active_AcademicService_By_CampusID()
        {
            //Arrange
            UserViewModel uvmArr = new UserViewModel()
            {
                AcademicEmail = "bachhvhe130603@fpt.edu.vn",
                Role = new RoleViewModel() { RoleName = Constants.Role.ADMIN }
            };

            //Act
            int campusID = 46;
            UserViewModel uvmAct = _userService.GetActiveAcaServiceByCampusId(campusID);

            //Assert
            Assert.AreEqual(uvmArr.AcademicEmail, uvmAct.AcademicEmail);
        }

        

        [TestMethod]
        public void Mapping_User_UserViewModel_Get_Admin_By_CampusID_And_AcademicEmail()
        {
            //Arrange
            UserViewModel uvmArr = new UserViewModel()
            {
                AcademicEmail = "tuannmhe130642@fpt.edu.vn",
                Role = new RoleViewModel() { RoleName = Constants.Role.ADMIN }
            };


            //Act
            int campusID = 46;
            string acaEmail = "tuannmhe130642@fpt.edu.vn";
            UserViewModel uvmAct = _userService.GetAdminByCampusIdAndAcaEmail(campusID, acaEmail);

            //Assert
            Assert.AreEqual(uvmArr.AcademicEmail, uvmAct.AcademicEmail);
        }

        [TestMethod]
        public void UserViewModel_Has_Mapped_Has_Role_Admin_Or_Not()
        {
            //Arrange
            UserViewModel uvmArr = new UserViewModel()
            {
                AcademicEmail = "tuannmhe130642@fpt.edu.vn",
                Role = new RoleViewModel() { RoleName = Constants.Role.ADMIN }
            };


            //Act
            int campusID = 46;
            string acaEmail = "tuannmhe130642@fpt.edu.vn";
            UserViewModel uvmAct = _userService.GetAdminByCampusIdAndAcaEmail(campusID, acaEmail);

            //Assert
            Assert.AreEqual(uvmArr.Role.RoleName, uvmAct.Role.RoleName);
        }

        [TestMethod]
        public void Mapping_UserViewModel_After_Login_As_SuperAdmin()
        {
            //Arrange
            UserViewModel uvmArr = new UserViewModel()
            {
                MemberCode = "sa",
                Role = new RoleViewModel() { RoleName = Constants.Role.SUPER_ADMIN }
            };

            //Act
            string memberCode = "sa";
            string password = "123456";
            UserViewModel uvmAct = _userService.Login(memberCode, password);

            //Assert
            Assert.AreEqual(uvmArr.MemberCode, uvmAct.MemberCode);
        }

        [TestMethod]
        public void UserViewModel_After_Login_Has_Role_SuperAdmin_Or_Not()
        {
            //Arrange
            UserViewModel uvmArr = new UserViewModel()
            {
                MemberCode = "sa",
                Role = new RoleViewModel() { RoleName = Constants.Role.SUPER_ADMIN }
            };

            //Act
            string memberCode = "sa";
            string password = "123456";
            UserViewModel uvmAct = _userService.Login(memberCode, password);

            //Assert
            Assert.AreEqual(uvmArr.Role.RoleName, uvmAct.Role.RoleName);
        }

        [TestMethod]
        public void Mappping_User_UserViewModel_Get_User_By_RollNumber()
        {
            //Arrange
            UserViewModel uvmArr = new UserViewModel()
            {
                RollNumber = "HE130576",
                Role = new RoleViewModel() { RoleName = Constants.Role.OWNER }
            };

            //Act
            UserViewModel uvmAct = _userService.GetUserByRollNumber("HE130576");

            //Assert
            Assert.AreEqual(uvmArr.RollNumber, uvmAct.RollNumber);
        }

        [TestMethod]
        public void UserViewModel_Get_By_RollNumber_Has_Role_Owner_Or_Not()
        {
            //Arrange
            UserViewModel uvmArr = new UserViewModel()
            {
                RollNumber = "HE130576",
                Role = new RoleViewModel() { RoleName = Constants.Role.OWNER }
            };

            //Act
            UserViewModel uvmAct = _userService.GetUserByRollNumber("HE130576");

            //Assert
            Assert.AreEqual(uvmArr.Role.RoleName, uvmAct.Role.RoleName);
        }

        [TestMethod]
        public void Mappping_User_UserViewModel_Get_User_By_UserID()
        {
            //Arrange
            UserViewModel uvmArr = new UserViewModel()
            {
                UserId = 176,
                AcademicEmail = "hapthe130576@fpt.edu.vn",
                Role = new RoleViewModel() { RoleName = Constants.Role.OWNER }
            };

            //Act
            UserViewModel uvmAct = _userService.GetUserByUserId(176);

            //Assert
            Assert.AreEqual(uvmArr.AcademicEmail, uvmAct.AcademicEmail);
        }

        
    }
}
