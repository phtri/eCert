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
        private readonly DataProvider<EducationSystem> _eduSystemProvider;
        private readonly DataProvider<Campus> _campusProvider;
        string connStr = WebConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        public AdminDAO()
        {
            _certificateServices = new CertificateServices();
            _userProvider = new DataProvider<User>();
            _eduSystemProvider = new DataProvider<EducationSystem>();
            _campusProvider = new DataProvider<Campus>();
        }
        public List<Campus> GetListCampusById(int eduSystemId)
        {
            List<Campus> campuses = new List<Campus>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //Certificate
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Campus");
                connection.Open();
                SqlCommand command = null;
                command = new SqlCommand("SELECT * FROM CAMPUS WHERE EDUCATIONSYSTEMID = @PARAM1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", eduSystemId);

                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("Campus");
                adapter.Fill(dataSet);


                connection.Close();

                DataTable certTable = dataSet.Tables["Campus"];
                campuses = _campusProvider.GetListObjects<Campus>(certTable.Rows);

            }
            return campuses;

        }
        //Get all education system
        public List<EducationSystem> GetAllEducationSystem()
        {
            List<EducationSystem> educationSystems = new List<EducationSystem>();

            using (SqlConnection connection = new SqlConnection(connStr))
            {

                //Certificate
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "EducationSystem");
                connection.Open();
                SqlCommand command = null;

                command = new SqlCommand("SELECT * FROM EDUCATIONSYSTEM", connection);
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command;

                //Fill data set
                DataSet dataSet = new DataSet("EducationSystem");
                adapter.Fill(dataSet);
                connection.Close();
                DataTable eduSystemTable = dataSet.Tables["EducationSystem"];
                educationSystems = _eduSystemProvider.GetListObjects<EducationSystem>(eduSystemTable.Rows);

                //Get certificate content
                foreach (EducationSystem educationSystem in educationSystems)
                {
                    SqlDataAdapter campusAdapter = new SqlDataAdapter();
                    campusAdapter.TableMappings.Add("Table", "Campus");
                    SqlCommand campusCommand = new SqlCommand("SELECT * FROM CAMPUS WHERE EDUCATIONSYSTEMID = @PARAM1", connection);
                    campusCommand.Parameters.AddWithValue("@PARAM1", educationSystem.EducationSystemId);
                    campusAdapter.SelectCommand = campusCommand;
                    campusAdapter.Fill(dataSet);
                    DataTable campusTable = dataSet.Tables["Campus"];
                    educationSystem.Campus = _campusProvider.GetListObjects<Campus>(campusTable.Rows);
                    campusTable.Clear();
                }
            }
            return educationSystems;
        }
        public Pagination<User> GetAcademicSerivcePagination(int pageSize, int pageNumber)
        {
            List<User> academicServices = GetAllAcademicService();

            Pagination<User> pagination = new Pagination<User>().GetPagination(academicServices, pageSize, pageNumber);
            return pagination;
        }
        public List<User> GetAllAcademicService()
        {
            string query = "select U.* from [User] U, [User_Role] UR where U.UserId = UR.UserId and UR.RoleId = @PARAM1";

            List<User> listAcademicService = _userProvider.GetListObjects<User>(query, new object[] { Constants.Role.FPT_UNIVERSITY_ACADEMIC });
            return listAcademicService;
        }

        //Get certificates from excel file
        public int AddCertificatesFromExcel(string excelConnectionString, int typeImport, int campusId)
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
                            FullName = _certificateServices.convertToUnSign3(row["Fullname"].ToString()),
                            Nationality = row["Nationality"].ToString(),
                            CertificateName = row["Content"].ToString(),
                            PlaceOfBirth = row["PlaceOfBirth"].ToString(),
                            VerifyCode = row["RegNo"].ToString(),
                            IssuerType = CertificateIssuer.FPT,
                            Url = Guid.NewGuid().ToString(),
                            //SubjectCode = row["SubjectCode"].ToString(),
                            ViewCount = 0,
                            //DateOfIssue = DateTime.Now,
                            //DateOfExpiry = DateTime.Now,
                            CampusId = campusId
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
                            IssuerType = CertificateIssuer.FPT,
                            ViewCount = 0,
                            //DateOfIssue = DateTime.Now,
                            //DateOfExpiry = DateTime.Now,
                            CampusId = campusId
                        };

                        certificates.Add(certificate);
                    }
                }
                

                //Add certificate to database
                return _certificateServices.AddMultipleCertificates(certificates, typeImport);
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
                    command.Parameters.Add(new SqlParameter("@RoleId", Constants.Role.FPT_UNIVERSITY_ACADEMIC));
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