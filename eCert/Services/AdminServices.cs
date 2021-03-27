
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
        public List<SignatureViewModel> GetSignatireByEduId(int eduSystemId)
        {
            List<Signature> signatures = _adminDAO.GetSignatireByEduId(eduSystemId);
            return AutoMapper.Mapper.Map<List<Signature>, List<SignatureViewModel>>(signatures);
        }


        //get list of academic service
        public Pagination<UserAcaServiceViewModel> GetAcademicServicePagination(int pageSize, int pageNumber, int userId)
        {
            Pagination<UserAcaService> academicService = _adminDAO.GetAcademicSerivcePagination(pageSize, pageNumber, userId);
            Pagination<UserAcaServiceViewModel> academicServiceViewModel = AutoMapper.Mapper.Map<Pagination<UserAcaService>, Pagination<UserAcaServiceViewModel>>(academicService);

            return academicServiceViewModel;
        }

        //Import certificate in excel files
        public ResultExcel ImportCertificatesByExcel(HttpPostedFileBase excelFile, string serverMapPath, int typeImport, int campusId, int signatureId)
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
                    return _adminDAO.AddCertificatesFromExcel(excelConnectionString, typeImport, campusId, signatureId);
                }
                return null;
            }
            catch(Exception e)
            {
                throw e;
                
            }
           

        }
        public void AddAcademicSerivce(UserViewModel userViewModel)
        {
            User user = AutoMapper.Mapper.Map<UserViewModel, User>(userViewModel);
            //Insert to User & User_Role table
            _adminDAO.AddAcademicSerivce(user);
        }
        //Check education system logo image file
       
       


    }
}