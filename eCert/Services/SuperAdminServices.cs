using eCert.Daos;
using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using static eCert.Utilities.Constants;

namespace eCert.Services
{
    public class SuperAdminServices
    {
        private readonly SuperAdminDAO _superAdminDao;
        

        public SuperAdminServices()
        {
            _superAdminDao = new SuperAdminDAO();
        }
        public void AddAcademicSerivce(UserViewModel userViewModel, int campusId)
        {
            User user = AutoMapper.Mapper.Map<UserViewModel, User>(userViewModel);
            //Insert to User & User_Role table
            _superAdminDao.AddAcademicSerivce(user, campusId);
        }
        public void AddAdminSerivce(UserViewModel userViewModel, int campusId)
        {
            User user = AutoMapper.Mapper.Map<UserViewModel, User>(userViewModel);
            //Insert to User & User_Role table
            _superAdminDao.AddAdminSerivce(user, campusId);
        }
        public void DeleteSignature(int signatureId, int eduSystemid)
        {
            _superAdminDao.DeleteSignature(signatureId, eduSystemid);
        }
        public void DeleteCampus(int campusId)
        {
            _superAdminDao.DeleteCampus(campusId);
        }
        public void DeleteEducation(int campusId)
        {
            _superAdminDao.DeleteEducation(campusId);
        }
        public int GetCountCertificateByCampus(int campusId)
        {
            return _superAdminDao.GetCountCertificateByCampus(campusId);
        }
        public int GetCountCertificateBySignature(int signatureId)
        {
            return _superAdminDao.GetCountCertificateBySignature(signatureId);
        }
        public int GetCountCertificateByEdu(int eduSystemId)
        {
            return _superAdminDao.GetCountCertificateByEdu(eduSystemId);
        }
        //Check education system logo image file
        public Result ValidateEducationSystemLogoImage(HttpPostedFileBase logo)
        {
            const int sizeLimit = 5; //5Mb

            int totalSize = 0;

            string[] supportedTypes = { "jpg", "jpeg", "png", "JPG", "JPEG", "PNG" };
            string fileExt = Path.GetExtension(logo.FileName).Substring(1).ToLower();
            totalSize += logo.ContentLength;
            if (Array.IndexOf(supportedTypes, fileExt) < 0)
            {
                return new Result()
                {
                    IsSuccess = false,
                    Message = "File Extension Is InValid - Only Upload PNG/JPG/JPEG file"
                };
            }
            //Total files size > 5mb
            else if (totalSize > (sizeLimit * 1024 * 1024))
            {
                return new Result()
                {
                    IsSuccess = false,
                    Message = "Total size of files can not exceed " + sizeLimit + "Mb"
                };
            }

            return new Result()
            {
                IsSuccess = true
            };
        }

        public Pagination<StaffViewModel> GetAdminPagination(int pageSize, int pageNumber)
        {
            Pagination<Staff> admins = _superAdminDao.GetAdminPagination(pageSize, pageNumber);
            Pagination<StaffViewModel> adminViewModel = AutoMapper.Mapper.Map<Pagination<Staff>, Pagination<StaffViewModel>>(admins);

            return adminViewModel;
        }
        public Pagination<StaffViewModel> GetAcaServicePagination(int pageSize, int pageNumber)
        {
            Pagination<Staff> acaServices = _superAdminDao.GetAcaServicePagination(pageSize, pageNumber);
            Pagination<StaffViewModel> acaServiceViewModel = AutoMapper.Mapper.Map<Pagination<Staff>, Pagination<StaffViewModel>>(acaServices);

            return acaServiceViewModel;
        }
        public Pagination<CampusViewModel> GetListCampusById(int pageSize, int pageNumber, int eduSystemId)
        {
            Pagination<Campus> campus = _superAdminDao.GetCampusByEduPagination(pageSize, pageNumber, eduSystemId);
            Pagination<CampusViewModel> campusViewModel = AutoMapper.Mapper.Map<Pagination<Campus>, Pagination<CampusViewModel>>(campus);

            return campusViewModel;
        }
        //Upload education system image
        public void UploadEducationSystemLogoImage(EducationSystemViewModel educationSystemViewModel)
        {
            string saveFolder = SaveLocation.EducationSystemLogoImageFolder;
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
            string logoExtension = Path.GetExtension(educationSystemViewModel.LogoImageFile.FileName).Substring(1).ToLower();
            string logoNewName = Guid.NewGuid().ToString() + "." + logoExtension;
            educationSystemViewModel.LogoImage = logoNewName;
            string savePath = Path.Combine(saveFolder, logoNewName);
            educationSystemViewModel.LogoImageFile.SaveAs(savePath);
        }
        //Upload singature image
        public void UploadEducationSystemSingatureImage(SignatureViewModel signatureViewModel)
        {
            string saveFolder = SaveLocation.EducationSystemSignatureImageFolder;
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
            string logoExtension = Path.GetExtension(signatureViewModel.SignatureImageFile.FileName).Substring(1).ToLower();
            string logoNewName = Guid.NewGuid().ToString() + "." + logoExtension;
            signatureViewModel.ImageFile = logoNewName;
            string savePath = Path.Combine(saveFolder, logoNewName);
            signatureViewModel.SignatureImageFile.SaveAs(savePath);
        }
        //Add education system to database
        public void AddEducationSystem(EducationSystemViewModel educationSystemViewModel)
        {
            EducationSystem educationSystem = AutoMapper.Mapper.Map<EducationSystemViewModel, EducationSystem>(educationSystemViewModel);
            //Add campus name to education system
            //foreach (string campusName in educationSystemViewModel.CampusNames)
            //{
            //    Campus newCampus = new Campus()
            //    {
            //        CampusName = campusName
            //    };
            //    educationSystem.Campuses.Add(newCampus);
            //}
            //Add to database
            _superAdminDao.AddEducationSystem(educationSystem);
        }
        public void AddCampus(CampusViewModel campusViewModel)
        {
            Campus campus = AutoMapper.Mapper.Map<CampusViewModel, Campus>(campusViewModel);
            _superAdminDao.AddCampus(campus);
        }

        //Add signature to database
        public void AddSignature(SignatureViewModel signatureViewModel)
        {
            Signature signature = AutoMapper.Mapper.Map<SignatureViewModel, Signature>(signatureViewModel);
            
            //Add to database
            _superAdminDao.AddSignature(signature);
        }
        
        public int GetCountEduByName(string eduName)
        {
            return _superAdminDao.GetCountEduByName(eduName);
        }
        public int GetCountCampusByName(string eduName, int eduSystemId)
        {
            return _superAdminDao.GetCountCampusByName(eduName, eduSystemId);
        }
        public List<EducationSystemViewModel> GetAllEducatinSystem()
        {
            List<EducationSystem> educationSystems = _superAdminDao.GetAllEducationSystem();
            return AutoMapper.Mapper.Map<List<EducationSystem>, List<EducationSystemViewModel>>(educationSystems);
        }
        public Pagination<SignatureViewModel> GetSignaturePagination(int pageSize, int pageNumber)
        {
            Pagination<Signature> signatures = _superAdminDao.GetSignaturePagination(pageSize, pageNumber);
            Pagination<SignatureViewModel> signatureViewmodel = AutoMapper.Mapper.Map<Pagination<Signature>, Pagination<SignatureViewModel>>(signatures);

            return signatureViewmodel;
        }
        public List<EducationSystemViewModel> GetEducatinSystemPagination(int pageSize, int pageNumber)
        {
            List<EducationSystem> educationList = _superAdminDao.GetEduSystemPagination(pageSize, pageNumber);
            List<EducationSystemViewModel> educationListViewModel = AutoMapper.Mapper.Map<List<EducationSystem>, List<EducationSystemViewModel>>(educationList);
            return educationListViewModel;
        }
        public List<CampusViewModel> GetListCampusById(int eduSystemId)
        {
            List<Campus> campuses = _superAdminDao.GetListCampusById(eduSystemId);
            return AutoMapper.Mapper.Map<List<Campus>, List<CampusViewModel>>(campuses);
        }


    }
}