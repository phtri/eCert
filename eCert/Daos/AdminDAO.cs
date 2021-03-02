using eCert.Models.Entity;
using eCert.Services;
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
        public AdminDAO()
        {
            _certificateServices = new CertificateServices();
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
                    CertificateName = row["CertificateName"].ToString(),
                    VerifyCode = Guid.NewGuid().ToString(),
                    Issuer = row["Issuer"].ToString(),
                    Description = row["Description"].ToString(),
                    ViewCount = 0,
                    DateOfIssue = DateTime.Now,
                    DateOfExpiry = DateTime.Now,
                    UserId = 1,
                    
                };

                certificates.Add(certificate);
            }

            //Add certificate to database
            _certificateServices.AddMultipleCertificates(certificates);
        }
    }
}