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
        public ResultExcel AddCertificatesFromExcel(string excelConnectionString, int typeImport, int campusId)
        {
                List<Certificate> certificates = new List<Certificate>();
                DataTable dataTable = new DataTable();
                ResultExcel resultExcel = new ResultExcel();
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
                            FullName = _certificateServices.ConvertToUnSign3(row["Fullname"].ToString()),
                            Nationality = row["Nationality"].ToString(),
                            CertificateName = row["Content"].ToString(),
                            PlaceOfBirth = row["PlaceOfBirth"].ToString(),
                            VerifyCode = row["RegNo"].ToString(),
                            IssuerType = CertificateIssuer.FPT,
                            Url = Guid.NewGuid().ToString(),
                            //SubjectCode = row["SubjectCode"].ToString(),
                            ViewCount = 0,
                            DateOfIssue = DateTime.Now,
                            //DateOfExpiry = DateTime.Now,
                            CampusId = campusId
                        };

                        certificates.Add(certificate);
                    }
                //validate certificate
                resultExcel = ValidateImportCertificate(certificates);
                if (resultExcel.ListRowError.Count != 0)
                {
                    return resultExcel;
                }
                else
                {
                    resultExcel.RowCountSuccess = _certificateServices.AddMultipleCertificates(certificates, typeImport);
                }
            }
            else if(typeImport == TypeImportExcel.IMPORT_DIPLOMA)
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
                            GraduationYear = row["GraduationYear"].ToString(),
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
                    //validate certificate
                    resultExcel = ValidateImportDiploma(certificates);
                    if (resultExcel.ListRowError.Count != 0)
                    {
                        return resultExcel;
                    }
                    else
                    {
                        resultExcel.RowCountSuccess = _certificateServices.AddMultipleCertificates(certificates, typeImport);
                    }

            }
               
            return resultExcel;
        }
        public ResultExcel ValidateImportCertificate(List<Certificate> certificates)
        {
            int row = 0;
            ResultExcel resultExcel = new ResultExcel()
            {
                ListRowError = new List<RowExcel>()
            };
            RowExcel rowRollNumber = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "RollNumber",
                Rows = new List<int>()
            };
            RowExcel rowFullName = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "FullName",
                Rows = new List<int>()
            };
            RowExcel rowNationality = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "Nationality",
                Rows = new List<int>()
            };
            RowExcel rowCertificateName = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "CertificateName",
                Rows = new List<int>()
            };
            RowExcel rowPlaceOfBirth = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "PlaceOfBirth",
                Rows = new List<int>()
            };
            RowExcel rowVerifyCode = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "RegNo",
                Rows = new List<int>()
            };
            
            foreach (Certificate certificate in certificates)
            {
                row++;
                if (String.IsNullOrEmpty(certificate.RollNumber))
                {
                    rowRollNumber.Rows.Add(row); 
                }
                if (String.IsNullOrEmpty(certificate.FullName))
                {
                    rowFullName.Rows.Add(row);
                }
                if (String.IsNullOrEmpty(certificate.Nationality))
                {
                    rowNationality.Rows.Add(row);
                }
                if (String.IsNullOrEmpty(certificate.CertificateName))
                {
                    rowCertificateName.Rows.Add(row);
                }
                if (String.IsNullOrEmpty(certificate.PlaceOfBirth))
                {
                    rowPlaceOfBirth.Rows.Add(row);
                }
                if (String.IsNullOrEmpty(certificate.VerifyCode))
                {
                    rowVerifyCode.Rows.Add(row);
                }
            }
                if(rowRollNumber.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowRollNumber);
                }
                if (rowFullName.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowFullName);
                }
                if (rowNationality.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowNationality);
                }
                if (rowCertificateName.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowCertificateName);
                }
                if (rowPlaceOfBirth.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowPlaceOfBirth);
                }
                if (rowVerifyCode.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowVerifyCode);
                }
            return resultExcel;
        }

        public ResultExcel ValidateImportDiploma(List<Certificate> certificates)
        {
            int row = 0;
            ResultExcel resultExcel = new ResultExcel()
            {
                ListRowError = new List<RowExcel>()
            };
            RowExcel rowRollNumber = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "RollNumber",
                Rows = new List<int>()
            };
            RowExcel rowFullName = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "FullName",
                Rows = new List<int>()
            };
            RowExcel rowNationality = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "Nationality",
                Rows = new List<int>()
            };
            RowExcel rowPlaceOfBirth = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "PlaceOfBirth",
                Rows = new List<int>()
            };
            RowExcel rowVerifyCode = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "RegNo",
                Rows = new List<int>()
            };
            RowExcel rowCurriculum = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "Curriculum",
                Rows = new List<int>()
            };
            RowExcel rowGraduationYear = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "GraduationYear",
                Rows = new List<int>()
            };
            RowExcel rowGraduationYearValidDate = new RowExcel()
            {
                TypeError = 2,
                ColumnName = "GraduationYear",
                Rows = new List<int>()
            };
            RowExcel rowGraduationGrade = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "GraduationGrade",
                Rows = new List<int>()
            };
            RowExcel rowGraduationDecisionNumber = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "GraduationDecisionNumber",
                Rows = new List<int>()
            };
            RowExcel rowDiplomaNumber = new RowExcel()
            {
                TypeError = 1,
                ColumnName = "DiplomaNumber",
                Rows = new List<int>()
            };

            foreach (Certificate certificate in certificates)
            {
                row++;
                if (String.IsNullOrEmpty(certificate.RollNumber))
                {
                    rowRollNumber.Rows.Add(row);
                }
                if (String.IsNullOrEmpty(certificate.FullName))
                {
                    rowFullName.Rows.Add(row);
                }
                if (String.IsNullOrEmpty(certificate.Nationality))
                {
                    rowNationality.Rows.Add(row);
                }
                if (String.IsNullOrEmpty(certificate.PlaceOfBirth))
                {
                    rowPlaceOfBirth.Rows.Add(row);
                }
                if (String.IsNullOrEmpty(certificate.Curriculum))
                {
                    rowCurriculum.Rows.Add(row);
                }
                if (String.IsNullOrEmpty(certificate.VerifyCode))
                {
                    rowVerifyCode.Rows.Add(row);
                }
                if (certificate.GraduationYear == null)
                {
                    rowGraduationYear.Rows.Add(row);
                }
                if (String.IsNullOrEmpty(certificate.GraduationGrade))
                {
                    rowGraduationGrade.Rows.Add(row);
                }
                if (String.IsNullOrEmpty(certificate.GraduationDecisionNumber))
                {
                    rowGraduationDecisionNumber.Rows.Add(row);
                }
                if (String.IsNullOrEmpty(certificate.DiplomaNumber))
                {
                    rowDiplomaNumber.Rows.Add(row);
                }
                if (!validateDateTime(certificate.GraduationYear))
                {
                    rowGraduationYearValidDate.Rows.Add(row);
                }
            }
                if(rowRollNumber.Rows.Count != 0)
                {
                resultExcel.ListRowError.Add(rowRollNumber);
                }
                if (rowFullName.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowFullName);
                }
                if (rowNationality.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowNationality);
                }
                if (rowPlaceOfBirth.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowPlaceOfBirth);
                }
                if (rowVerifyCode.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowVerifyCode);
                }
                if (rowCurriculum.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowCurriculum);
                }
                if (rowGraduationYear.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowGraduationYear);
                }
                if (rowGraduationGrade.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowGraduationGrade);
                }
                if (rowGraduationDecisionNumber.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowGraduationDecisionNumber);
                }
                if (rowDiplomaNumber.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowDiplomaNumber);
                }
                if (rowGraduationYearValidDate.Rows.Count != 0)
                {
                    resultExcel.ListRowError.Add(rowGraduationYearValidDate);
                }

            return resultExcel;
        }
        public bool validateDateTime (string date)
        {
            try
            {
                DateTime resultDate = DateTime.Parse(date);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
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