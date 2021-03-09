using eCert.Daos;
using eCert.Models.Entity;
using eCert.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static eCert.Utilities.Constants;

namespace eCert.Services
{
    public class UserServices
    {
        private readonly UserDAO _userDao;
        public UserServices()
        {
            _userDao = new UserDAO();
        }

        //Get User by academic (FU Email)
        public UserViewModel GetUserByAcademicEmail(string email)
        {
            return AutoMapper.Mapper.Map<User, UserViewModel>(_userDao.GetUserByAcademicEmail(email));
        }

        //Add user to database
        public void AddUser(UserViewModel userViewModel)
        {
            //Generate password hash & password salt
            User user = AutoMapper.Mapper.Map<UserViewModel, User>(userViewModel);
            user.PasswordHash = "TEST_HASH";
            user.PasswordSalt = "TEST_SALT";
            user.Role = new Role() { 
                RoleId = RoleCons.OWNER
            };
            _userDao.AddUser(user);
        }
    }
}