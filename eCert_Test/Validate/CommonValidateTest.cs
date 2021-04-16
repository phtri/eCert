using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCert.Daos;

namespace eCert_Test.Validate
{
    [TestClass]
    public class CommonValidateTest
    {
        private AdminDAO adminDAO;

        [TestInitialize]
        public void Init()
        {
            adminDAO = new AdminDAO();
        }
        [TestMethod]
        public void Validate_Date_Time()
        {
            bool isValid = adminDAO.validateDateTime("2020/4/13");
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Remove_Space()
        {
            //Arrange
            string strAfterRemove = "Electronic Certificate";

            //Actual
            string strBeforeRemove = "   Electronic   Certificate  ";

            //Assert
            Assert.AreEqual(strAfterRemove, adminDAO.removeSpace(strBeforeRemove));
        }

        [TestMethod]
        public void Validate_Contain_Invalid_Character()
        {
            bool valid = adminDAO.validateContainInvalidCharacter("Àabcdef");
            Assert.IsTrue(valid);
        }


    }
}
