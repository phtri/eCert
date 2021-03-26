
using eCert.Daos;
using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using static eCert.Utilities.Constants;

namespace eCert.Services
{
    public class AdminServices
    {
        private readonly AdminDAO _adminDAO;
        public AdminServices()
        {
            _adminDAO = new AdminDAO();
        }
        public List<EducationSystemViewModel> GetEducationSystem(int userId)
        {
            List<EducationSystem> educationSystems = _adminDAO.GetEducationSystem(userId);
            return AutoMapper.Mapper.Map<List<EducationSystem>, List<EducationSystemViewModel>>(educationSystems);
        }
        public List<CampusViewModel> GetCampusByUserId(int userId, int eduSystemId)
        {
            List<Campus> educationSystems = _adminDAO.GetListCampusByUserId(userId, eduSystemId);
            return AutoMapper.Mapper.Map<List<Campus>, List<CampusViewModel>>(educationSystems);
        }
        public List<EducationSystemViewModel> GetAllEducatinSystem()
        {
            List<EducationSystem> educationSystems = _adminDAO.GetAllEducationSystem();
            return AutoMapper.Mapper.Map<List<EducationSystem>, List<EducationSystemViewModel>>(educationSystems);
        }
        public List<CampusViewModel> GetListCampusById(int eduSystemId)
        {
            List<Campus> campuses = _adminDAO.GetListCampusById(eduSystemId);
            return AutoMapper.Mapper.Map<List<Campus>, List<CampusViewModel>>(campuses);
        }
        //get list of academic service
        public Pagination<UserViewModel> GetAcademicServicePagination(int pageSize, int pageNumber)
        {
            Pagination<User> academicService = _adminDAO.GetAcademicSerivcePagination(pageSize, pageNumber);
            Pagination<UserViewModel> academicServiceViewModel = AutoMapper.Mapper.Map<Pagination<User>, Pagination<UserViewModel>>(academicService);

            return academicServiceViewModel;
        }

        //Import certificate in excel files
        public ResultExcel ImportCertificatesByExcel(HttpPostedFileBase excelFile, string serverMapPath, int typeImport, int campusId)
        {
            try
            {
                string filePath = string.Empty;
                if (excelFile != null)
                {
                    //config location to save file excel
                    if (!Directory.Exists(serverMapPath))
                    {
                        Directory.CreateDirectory(serverMapPath);
                    }

                    filePath = serverMapPath + Path.GetFileName(excelFile.FileName);
                    string excelExtension = Path.GetExtension(excelFile.FileName);
                    //save excel file to server
                    excelFile.SaveAs(filePath);

                    //Excel connection string to read file
                    string excelConnectionString = string.Empty;
                    switch (excelExtension)
                    {
                        case ".xls": //Excel 97-03.
                            excelConnectionString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            excelConnectionString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }

                    excelConnectionString = string.Format(excelConnectionString, filePath);
                    //Add to database
                    return _adminDAO.AddCertificatesFromExcel(excelConnectionString, typeImport, campusId);
                }
                return null;
            }
            catch(Exception e)
            {
                throw e;
                
            }
           

        }
        public void AddAcademicSerivce(User user)
        {
            //Insert to User & User_Role table
            _adminDAO.AddAcademicSerivce(user);
        }
        //Check education system logo image file
        public Result ValidateEducationSystemLogoImage(HttpPostedFileBase logo)
        {
            const int sizeLimit = 5; //20Mb

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
        public void UploadEducationSystemLogoImage(EducationSystemViewModel educationSystemViewModel)
        {
            string saveFolder = SaveLocation.EducationSystemFolder;
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


    }
}