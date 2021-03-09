using eCert.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Daos
{
    public class UserDAO
    {
        public UserDAO()
        {
            _userProvider = new DataProvider<User>();
        }
        private readonly DataProvider<User> _userProvider;
        public User GetUserByAcademicEmail(string email)
        {
            string query = "SELECT * FROM [User] where AcademicEmail = @param1";
            User user = _userProvider.GetObject<User>(query, new object[] { email });
            return user;
        }
    }
}