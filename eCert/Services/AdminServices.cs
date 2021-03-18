﻿
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

namespace eCert.Services
{
    public class AdminServices
    {
        private readonly AdminDAO _adminDAO;
        public AdminServices()
        {
            _adminDAO = new AdminDAO();
        }
        public List<EducationSystemViewModel> GetAllEducatinSystem()
        {
            List<EducationSystem> educationSystems = _adminDAO.GetAllEducationSystem();
            return AutoMapper.Mapper.Map<List<EducationSystem>, List<EducationSystemViewModel>>(educationSystems);
        }

        //get list of academic service
        public Pagination<UserViewModel> GetAcademicServicePagination(int pageSize, int pageNumber)
        {
            Pagination<User> academicService = _adminDAO.GetAcademicSerivcePagination(pageSize, pageNumber);
            Pagination<UserViewModel> academicServiceViewModel = AutoMapper.Mapper.Map<Pagination<User>, Pagination<UserViewModel>>(academicService);

            return academicServiceViewModel;
        }

        //Import certificate in excel files
        public void ImportCertificatesByExcel(HttpPostedFileBase excelFile, string serverMapPath, int typeImport)
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
                    _adminDAO.AddCertificatesFromExcel(excelConnectionString, typeImport);

                    
                }
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
        
    }
}