using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCert.Daos;
using eCert.Services;

namespace eCert_Test.Validate
{
    [TestClass]
    public class CommonValidateTest
    {
        private AdminDAO adminDAO;
        private CertificateServices certService;

        [TestInitialize]
        public void Init()
        {
            adminDAO = new AdminDAO();
            certService = new CertificateServices();
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

        [TestMethod]
        public void Normalized_Searched_Keyword()
        {
            //Arrange
            string strAfterNormalize = "nguyen minh tuan";

            //Actual
            string strBeforeNormalize = "   Nguyễn Minh Tuấn   ";

            //Assert
            Assert.AreEqual(strAfterNormalize, certService.NormalizeSearchedKeyWord(strBeforeNormalize));
        }
    }
}
