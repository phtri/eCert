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
        private readonly EmailServices _emailServices;
        public UserServices()
        {
            _userDao = new UserDAO();
            _emailServices = new EmailServices();
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
        //Login
        public UserViewModel Login(string memberCode, string password)
        {
            User user = _userDao.GetUserByMemberCode(memberCode);
            //Check password
            
            bool passWordresult = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!passWordresult)
                return null;
            return AutoMapper.Mapper.Map<User, UserViewModel>(user);
        }
        //Add user to database
        public void AddUser(UserViewModel userViewModel)
        {
            //Generate password hash & password salt
            string randomPassword = GenereateRandomString(6);
            int costParameter = 12;
            string hasedPassword = BCrypt.Net.BCrypt.HashPassword(randomPassword, costParameter);

            User user = AutoMapper.Mapper.Map<UserViewModel, User>(userViewModel);
            user.PasswordHash = hasedPassword;
            user.Role = new Models.Entity.Role() { 
                RoleId = 1
            };
            _userDao.AddUser(user);

            //Send email to user
            _emailServices.SendEmail(userViewModel.AcademicEmail, "Your new account information", "Username: " + userViewModel.MemberCode + "\nPassword: " + randomPassword);
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

        private string GenereateRandomString(int length)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}