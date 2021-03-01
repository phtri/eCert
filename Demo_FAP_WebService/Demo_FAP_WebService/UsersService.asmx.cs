using Demo_FAP_WebService.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Demo_FAP_WebService
{
    /// <summary>
    /// Summary description for UsersService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UsersService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public List<User> GetStudentList()
        {
            return new List<User>()
            {
                new User(){FirstName = "Pham", MiddleName = "Thanh", LastName = "Ha", Gender = true, DOB = new DateTime(1999, 9, 1), PhoneNumber = "0912345678", PersonalEmail = "ha@mail.com", AcademicEmail = "hafap@fpt.edu.vn", RollNumber = "HE130576"},
                new User(){FirstName = "Pham", MiddleName = "Thanh", LastName = "Ha", Gender = true, DOB = new DateTime(1999, 9, 1), PhoneNumber = "0912345678", PersonalEmail = "ha@mail.com", AcademicEmail = "hafap@fpt.edu.vn", RollNumber = "HE130576"},
                new User(){FirstName = "Pham", MiddleName = "Thanh", LastName = "Ha", Gender = true, DOB = new DateTime(1999, 9, 1), PhoneNumber = "0912345678", PersonalEmail = "ha@mail.com", AcademicEmail = "hafap@fpt.edu.vn", RollNumber = "HE130576"}
            };
        } 
    }
}
