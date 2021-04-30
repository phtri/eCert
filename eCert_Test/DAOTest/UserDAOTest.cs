using eCert.Daos;
using eCert.Models.Entity;
using eCert.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace eCert_Test.DAOTest
{
    [TestClass]
    public class UserDAOTest
    {
        private UserDAO userDAO = null;

        [TestInitialize]
        public void Initialize()
        {
            //_userDAO = new UserDAO();
            //mockUserDAO.Setup(x => x.GetUserByMemberCode("admin")).Returns(new User()
            //{
            //    MemberCode = "admin"
            //});
            userDAO = new UserDAO();
        }

        [TestMethod]
        public void Member_Code_Admin_Is_Exist()
        {
            string memCode = "hapthe130576";
            User u = userDAO.GetUserByMemberCode(memCode);
            Assert.IsNotNull(u);
        }

        [TestMethod]
        public void Name_Of_User_Has_Role_Admin_Is_True()
        {
            //Arrange
            Role roleAdminArr = new Role()
            {
                RoleName = Constants.Role.ADMIN
            };

            //Actual
            Role roleAdminAct = userDAO.GetRoleByRoleName(Constants.Role.ADMIN);

            //Assert
            Assert.AreEqual(roleAdminArr.RoleName, roleAdminAct.RoleName);
        }

        [TestMethod]
        public void Name_Of_User_Has_Role_SuperAdmin_Is_True()
        {
            //Arrange
            Role roleAdminArr = new Role()
            {
                RoleName = Constants.Role.SUPER_ADMIN
            };

            //Actual
            Role roleAdminAct = userDAO.GetRoleByRoleName(Constants.Role.SUPER_ADMIN);

            //Assert
            Assert.AreEqual(roleAdminArr.RoleName, roleAdminAct.RoleName);
        }

        [TestMethod]
        public void Name_Of_User_Has_Role_Owner_Is_True()
        {
            //Arrange
            Role roleAdminArr = new Role()
            {
                RoleName = Constants.Role.OWNER
            };

            //Actual
            Role roleAdminAct = userDAO.GetRoleByRoleName(Constants.Role.OWNER);

            //Assert
            Assert.AreEqual(roleAdminArr.RoleName, roleAdminAct.RoleName);
        }

        [TestMethod]
        public void Name_Of_User_Has_Role_AcademicService_Is_True()
        {
            //Arrange
            Role roleAdminArr = new Role()
            {
                RoleName = Constants.Role.FPT_UNIVERSITY_ACADEMIC
            };

            //Actual
            Role roleAdminAct = userDAO.GetRoleByRoleName(Constants.Role.FPT_UNIVERSITY_ACADEMIC);

            //Assert
            Assert.AreEqual(roleAdminArr.RoleName, roleAdminAct.RoleName);
        }

        [TestMethod]
        public void Update_User()
        {
            
        }

        [TestMethod]
        public void Number_Of_Admin_Role_By_UserID()
        {
            //Arrange 
            int numberAdminArr = 0;
            int userID = 108;

            //Actual
            int numberAdminAct = userDAO.GetNumberOfAdminRole(userID);

            //Assert 
            Assert.AreEqual(numberAdminArr, numberAdminAct);
        }

        [TestMethod]
        public void Number_Of_Academic_Service_Role_By_UserID()
        {
            //Arrange 
            int numberAcaArr = 0;
            int userID = 111;

            //Actual
            int numberAcaAct = userDAO.GetNumberOfAdminRole(userID);

            //Assert 
            Assert.AreEqual(numberAcaArr, numberAcaAct);
        }

        [TestMethod]
        public void UserGetByUserIDIsExist()
        {
            //Arrange 
            int userID = 83;

            //Actual
            User u = userDAO.GetUserByUserId(userID);

            //Assert
            Assert.IsNotNull(u);
        }
       
        [TestMethod]
        public void User_Get_By_RollNumber_Is_Exist()
        {
            //Arrange 
            string rollNum = "HE130576";

            //Actual
            User u = userDAO.GetUserByRollNumber(rollNum);

            //Assert
            Assert.IsNotNull(u);
        }

        [TestMethod]
        public void User_Get_By_AcademicEmail_Is_Exist()
        {
            //Arrange 
            string email = "hapthe130576@fpt.edu.vn";

            //Actual
            User u = userDAO.GetUserByAcademicEmail(email);

            //Assert
            Assert.IsNotNull(u);
        }

        [TestMethod]
        public void Check_Role_Of_Campus_Is_Admin_Or_Not()
        {
            //Arrange 
            int campusID = 23;
            string acaEmail = "tuannmhe130642@fpt.edu.vn";

            //Actual
            User u = userDAO.GetAdminByCampusIdAndAcaEmail(campusID, acaEmail);

            //Assert
            Assert.AreEqual(Constants.Role.ADMIN, u.Role.RoleName);
        }

        [TestMethod]
        public void Check_Role_Of_Campus_Is_AcademicService_Or_Not()
        {
            //Arrange 
            int campusID = 23;
            string acaEmail = "bachhvhe130603@fpt.edu.vn";

            //Actual
            User u = userDAO.GetAcaServiceByCampusId(campusID, acaEmail);

            //Assert
            Assert.AreEqual(Constants.Role.FPT_UNIVERSITY_ACADEMIC, u.Role.RoleName);
        }
    }
}
