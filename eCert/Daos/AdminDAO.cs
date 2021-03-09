using eCert.Models.Entity;
using eCert.Services;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace eCert.Daos
{
    public class AdminDAO
    {
        private readonly CertificateServices _certificateServices;
        private readonly DataProvider<User> _userProvider;
        public AdminDAO()
        {
            _certificateServices = new CertificateServices();
            _userProvider = new DataProvider<User>();
        }

        public Pagination<User> GetAcademicSerivcePagination(int pageSize, int pageNumber)
        {
            List<User> academicServices = GetAllAcademicService();

            Pagination<User> pagination = new Pagination<User>().GetPagination(academicServices, pageSize, pageNumber);
            return pagination;
        }

        public List<User> GetAllAcademicService()
        {
            string query = "";

            List<User> listCertificate = _userProvider.GetListObjects<User>(query, new object[] { });
            return listCertificate;
        }

        //Get certificates from excel file
        public void AddCertificatesFromExcel(string excelConnectionString)
        {
            
                List<Certificate> certificates = new List<Certificate>();
                DataTable dataTable = new DataTable();

                //Fill data from excel to data table
                using (OleDbConnection connExcel = new OleDbConnection(excelConnectionString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dataTable);
                            connExcel.Close();
                        }
                    }
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    Certificate certificate = new Certificate()
                    {
                        CertificateName = row["Content"].ToString(),
                        VerifyCode = row["RegNo"].ToString(),
                        Issuer = "FPT University",
                        SubjectCode = row["SubjectCode"].ToString(),
                        ViewCount = 0,
                        OrganizationId = 1,
                        //DateOfIssue = DateTime.Now,
                        //DateOfExpiry = DateTime.Now,
                    };

                    certificates.Add(certificate);
                }

                //Add certificate to database
                _certificateServices.AddMultipleCertificates(certificates);
            
            
        }
    }
}