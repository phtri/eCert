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
        public UserViewModel GetUserByCampusId(int campusId)
        {
            return AutoMapper.Mapper.Map<User, UserViewModel>(_userDao.GetUserByCampusId(campusId));
        }

        //get user by provided email and password
        public UserViewModel GetUserByProvidedEmailAndPass(string email, string password)
        {
            return AutoMapper.Mapper.Map<User, UserViewModel>(_userDao.GetUserByProvidedEmailAndPass(email, password));
        }

        //Add user to database
        public void AddUser(UserViewModel userViewModel)
        {
            //Generate password hash & password salt
            User user = AutoMapper.Mapper.Map<UserViewModel, User>(userViewModel);
            user.PasswordHash = "TEST_HASH";
            user.PasswordSalt = "TEST_SALT";
            user.Role = new Models.Entity.Role() { 
                RoleId = 1
            };
            _userDao.AddUser(user);
        }

        public UserViewModel GetUserByRollNumber(string rollNumber)
        {
            UserViewModel viewModel = AutoMapper.Mapper.Map<User, UserViewModel>(_userDao.GetUserByRollNumber(rollNumber));
            return viewModel;
        }

        //Delete user
        public void DeleteUserAcademicService(int userId, int campusId)
        {
            _userDao.DeleteUserAcademicService(userId, campusId);
        }

        //Update user
        public void UpdateUser(UserViewModel userViewModel)
        {
            User user = AutoMapper.Mapper.Map<UserViewModel, User>(userViewModel);
            _userDao.UpdateUser(user);
        }
    }
}