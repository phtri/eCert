﻿using eCert.Daos;
using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Utilities;
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
        public UserViewModel GetAcaServiceByCampusId(int campusId)
        {
            return AutoMapper.Mapper.Map<User, UserViewModel>(_userDao.GetAcaServiceByCampusId(campusId));
        }
        public UserViewModel GetActiveAdminByCampusId(int campusId)
        {
            return AutoMapper.Mapper.Map<User, UserViewModel>(_userDao.GetActiveAdminByCampusId(campusId));
        }
        public UserViewModel GetActiveAcaServiceByCampusId(int campusId)
        {
            return AutoMapper.Mapper.Map<User, UserViewModel>(_userDao.GetActiveAcaServiceByCampusId(campusId));
        }
        //get user admin by campus id
        public UserViewModel GetAdminByCampusId(int campusId)
        {
            return AutoMapper.Mapper.Map<User, UserViewModel>(_userDao.GetAdminByCampusId(campusId));
        }
        public UserViewModel GetAdminByUserId(int userId)
        {
            return AutoMapper.Mapper.Map<User, UserViewModel>(_userDao.GetAdminByUserId(userId));
        }
        public UserViewModel GetAcaServiceByUserId(int userId)
        {
            return AutoMapper.Mapper.Map<User, UserViewModel>(_userDao.GetAcaServiceByUserId(userId));
        }
        //Login
        public UserViewModel Login(string memberCode, string password)
        {
            User user = _userDao.GetUserByMemberCode(memberCode);
            bool passWordresult = false;
            //Check password
            if (user != null)
            {
                passWordresult = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            }
            
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
            
            //Get owner role id
            int ownerRoleId = _userDao.GetRoleByRoleName(Constants.Role.OWNER).RoleId;
            user.Role = new Models.Entity.Role()
            {
                RoleId = ownerRoleId
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
        public void DeleteUserAcademicService(int userId, int campusId, int roleId)
        {
            _userDao.DeleteUserAcademicService(userId, campusId, roleId);
        }

        public void DeleteAdmin(int userId, int campusId, int roleId)
        {
            _userDao.DeleteAdmin(userId, campusId, roleId);
        }

        //Update user
        public void UpdateUser(UserViewModel userViewModel)
        {
            User user = AutoMapper.Mapper.Map<UserViewModel, User>(userViewModel);
            _userDao.UpdateUser(user);
        }
        public void UpdateUserRole(UserRoleViewModel userRoleViewModel)
        {
            UserRole userRole = AutoMapper.Mapper.Map<UserRoleViewModel, UserRole>(userRoleViewModel);
            _userDao.UpdateUserRole(userRole);
        }
        private string GenereateRandomString(int length)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public UserViewModel GetUserByUserId(int userId)
        {
            return AutoMapper.Mapper.Map<User, UserViewModel>(_userDao.GetUserByUserId(userId));
        }
    }
}