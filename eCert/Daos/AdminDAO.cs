using eCert.Models.Entity;
using eCert.Services;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using static eCert.Utilities.Constants;

namespace eCert.Daos
{
    public class AdminDAO
    {
        private readonly CertificateServices _certificateServices;
        private readonly DataProvider<User> _userProvider;
        string connStr = WebConfigurationManager.ConnectionStrings["Database"].ConnectionString;
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
            string query = "select U.* from [User] U, [User_Role] UR where U.UserId = UR.UserId and UR.RoleId = 3";

            List<User> listAcademicService = _userProvider.GetListObjects<User>(query, new object[] { });
            return listAcademicService;
        }

        //Get certificates from excel file
        public void AddCertificatesFromExcel(string excelConnectionString, int typeImport)
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
                if(typeImport == TypeImportExcel.IMPORT_CERT)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Certificate certificate = new Certificate()
                        {
                            RollNumber = row["RollNumber"].ToString(),
                            FullName = row["Fullname"].ToString(),
                            Nationality = row["Nationality"].ToString(),
                            CertificateName = row["Content"].ToString(),
                            PlaceOfBirth = row["PlaceOfBirth"].ToString(),
                            VerifyCode = row["RegNo"].ToString(),
                            Issuer = "FPT University",
                            //SubjectCode = row["SubjectCode"].ToString(),
                            ViewCount = 0,
                            OrganizationId = 1,
                            //DateOfIssue = DateTime.Now,
                            //DateOfExpiry = DateTime.Now,
                        };

                        certificates.Add(certificate);
                    }
                }else if(typeImport == TypeImportExcel.IMPORT_DIPLOMA)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Certificate certificate = new Certificate()
                        {
                            RollNumber = row["RollNumber"].ToString(),
                            FullName = row["Fullname"].ToString(),
                            PlaceOfBirth = row["PlaceOfBirth"].ToString(),
                            Nationality = row["Nationality"].ToString(),
                            Curriculum = row["Curriculum"].ToString(),
                            GraduationYear = (DateTime)row["GraduationYear"],
                            GraduationGrade = row["GraduationGrade"].ToString(),
                            GraduationDecisionNumber = row["GraduationDecisionNumber"].ToString(),
                            DiplomaNumber = row["DiplomaNumber"].ToString(),
                            VerifyCode = row["RegNo"].ToString(),
                            Issuer = "FPT University",
                            ViewCount = 0,
                            OrganizationId = 1,
                            //DateOfIssue = DateTime.Now,
                            //DateOfExpiry = DateTime.Now,
                        };

                        certificates.Add(certificate);
                    }
                }
                

                //Add certificate to database
                _certificateServices.AddMultipleCertificates(certificates, typeImport);
        }

        public void AddAcademicSerivce(User user)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("eCert_Transaction");
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    //Insert to table [User]
                    command.CommandText = "sp_Insert_User";
                   
                    command.Parameters.Add(new SqlParameter("@PasswordHash", "abc"));
                    command.Parameters.Add(new SqlParameter("@PasswordSalt", "abc"));
                    command.Parameters.Add(new SqlParameter("@Gender", false));
                    command.Parameters.Add(new SqlParameter("@DOB", "1/1/1999"));
                    command.Parameters.Add(new SqlParameter("@PhoneNumber", user.PhoneNumber));
                    command.Parameters.Add(new SqlParameter("@PersonalEmail", ""));
                    command.Parameters.Add(new SqlParameter("@AcademicEmail", user.AcademicEmail));
                    command.Parameters.Add(new SqlParameter("@RollNumber", ""));
                    command.Parameters.Add(new SqlParameter("@Ethnicity", ""));

                    //Get id of new certificate inserted to the database
                    int insertedUserId = Int32.Parse(command.ExecuteScalar().ToString());

                    command.CommandText = "sp_Insert_User_Role";
                    
                    //Remove old parameters
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@UserId", insertedUserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", RoleCons.FPT_UNIVERSITY_ACADEMIC));
                    command.ExecuteNonQuery();


                    //Commit the transaction
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    transaction.Rollback();
                    throw new Exception();
                }

            }
        }
       

    }
}