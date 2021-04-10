using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Services;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using static eCert.Utilities.Constants;

namespace eCert.Daos
{
    public class CertificateDAO
    {
        private readonly DataProvider<Certificate> _certProvider;
        private readonly DataProvider<CertificateContents> _certContentProvider;
        private readonly DataProvider<User> _userProvider;
        private readonly DataProvider<Signature> _signatureProvider;
        private readonly UserServices _userServices;
        private readonly EmailServices _emailServices;
        string connStr = WebConfigurationManager.ConnectionStrings["Database"].ConnectionString;

        public CertificateDAO()
        {
            _certProvider = new DataProvider<Certificate>();
            _certContentProvider = new DataProvider<CertificateContents>();
            _userProvider = new DataProvider<User>();
            _signatureProvider = new DataProvider<Signature>();
            _userServices = new UserServices();
            _emailServices = new EmailServices();
        }
        //Get all certificates of a user
        public List<Certificate> GetAllCertificates(string rollNumber, string keyword)
        {
            List<Certificate> certificates = new List<Certificate>();
            
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                
                //Certificate
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Certificate");
                connection.Open();
                SqlCommand command = null;
                if (!String.IsNullOrEmpty(keyword))
                {
                    command = new SqlCommand("SELECT * FROM CERTIFICATE WHERE ROLLNUMBER = @PARAM1 AND CERTIFICATENAME LIKE @PARAM2 COLLATE SQL_Latin1_General_Cp1_CI_AI", connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@PARAM1", rollNumber);
                    command.Parameters.AddWithValue("@PARAM2", "%" + keyword + "%");
                }
                else
                {
                    command = new SqlCommand("SELECT * FROM CERTIFICATE WHERE ROLLNUMBER = @PARAM1", connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@PARAM1", rollNumber);
                }
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("Certificate");
                adapter.Fill(dataSet);
                connection.Close();
                DataTable certTable = dataSet.Tables["Certificate"];
                certificates = _certProvider.GetListObjects<Certificate>(certTable.Rows);

                //Get certificate content
                foreach (Certificate certificate in certificates)
                {
                    SqlDataAdapter certificateContentAdapter = new SqlDataAdapter();
                    certificateContentAdapter.TableMappings.Add("Table", "CertificateContent");
                    SqlCommand certificateContentsCommand = new SqlCommand("SELECT * FROM CERTIFICATECONTENT WHERE CERTIFICATEID = @PARAM1", connection);
                    certificateContentsCommand.Parameters.AddWithValue("@PARAM1", certificate.CertificateId);
                    certificateContentAdapter.SelectCommand = certificateContentsCommand;
                    certificateContentAdapter.Fill(dataSet);
                    DataTable certContentTable = dataSet.Tables["CertificateContent"];
                    certificate.CertificateContents = _certContentProvider.GetListObjects<CertificateContents>(certContentTable.Rows);

                    

                    certContentTable.Clear();
                }
            }
            return certificates;
        }

        //Get all report of a user
        public List<Report> GetAllReportById(int userId)
        {
            List<Report> certificates = new List<Report>();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //Certificate
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Report");
                connection.Open();
                SqlCommand command = null;
                command = new SqlCommand("SELECT * FROM REPORT WHERE USERID = @PARAM1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", userId);
                
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("Report");
                adapter.Fill(dataSet);
                connection.Close();
                DataTable certTable = dataSet.Tables["Report"];
                certificates = _certProvider.GetListObjects<Report>(certTable.Rows);

            }
            return certificates;
        }
        //gett all report
        public List<Report> GetAllReport()
        {
            List<Report> certificates = new List<Report>();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //Certificate
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Report");
                connection.Open();
                SqlCommand command = null;
                command = new SqlCommand("SELECT * FROM REPORT", connection);
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("Report");
                adapter.Fill(dataSet);
                connection.Close();
                DataTable certTable = dataSet.Tables["Report"];
                certificates = _certProvider.GetListObjects<Report>(certTable.Rows);

            }
            return certificates;
        }
        //Get certificate by certificate Id
        public Certificate GetCertificateById(int certId)
        {
            Certificate certificate = new Certificate();
            
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //Certificate
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Certificate");
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM CERTIFICATE WHERE CERTIFICATEID = @PARAM1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", certId);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("Certificate");
                adapter.Fill(dataSet);

                //CertificateContent
                SqlDataAdapter certificateContentAdapter = new SqlDataAdapter();
                certificateContentAdapter.TableMappings.Add("Table", "CertificateContent");
                SqlCommand certificateContentsCommand = new SqlCommand("SELECT * FROM CERTIFICATECONTENT WHERE CERTIFICATEID = @PARAM1", connection);
                certificateContentsCommand.Parameters.AddWithValue("@PARAM1", certId);
                certificateContentAdapter.SelectCommand = certificateContentsCommand;
                certificateContentAdapter.Fill(dataSet);

                //User
                SqlDataAdapter userAdapter = new SqlDataAdapter();
                userAdapter.TableMappings.Add("Table", "User");
                SqlCommand userCommand = new SqlCommand("SELECT * FROM CERTIFICATE C, [USER] U WHERE C.CERTIFICATEID = @PARAM1 AND C.ROLLNUMBER = U.ROLLNUMBER", connection);
                userCommand.Parameters.AddWithValue("@PARAM1", certId);
                userAdapter.SelectCommand = userCommand;

                //Fill data set
                userAdapter.Fill(dataSet);

                //Signature
                SqlDataAdapter signatureAdapter = new SqlDataAdapter();
                signatureAdapter.TableMappings.Add("Table", "Signature");
                SqlCommand signatureCommand = new SqlCommand("SELECT * FROM SIGNATURE WHERE SIGNATUREID = @PARAM1", connection);
                signatureCommand.Parameters.AddWithValue("@PARAM1", certificate.SignatureId);
                signatureAdapter.SelectCommand = signatureCommand;
                signatureAdapter.Fill(dataSet);

                //Close connection
                connection.Close();
                DataTable certTable = dataSet.Tables["Certificate"];
                DataTable certContentTable = dataSet.Tables["CertificateContent"];
                DataTable userTable = dataSet.Tables["User"];
                DataTable signatureTable = dataSet.Tables["Signature"];
                certificate = _certProvider.GetItem<Certificate>(certTable.Rows[0]);
                if (certContentTable.Rows.Count > 0)
                {
                    certificate.CertificateContents = _certContentProvider.GetListObjects<CertificateContents>(certContentTable.Rows);
                }
                
                if(userTable.Rows.Count > 0)
                {
                    certificate.User = _userProvider.GetItem<User>(userTable.Rows[0]);
                }
                //Has signature
                if (signatureTable.Rows.Count > 0)
                {
                    certificate.Signature = _signatureProvider.GetItem<Signature>(signatureTable.Rows[0]);
                }
            }
            return certificate;
        }

        public Certificate GetCertificateByUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            Certificate certificate = null;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //Certificate
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Certificate");
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM CERTIFICATE WHERE URL = @PARAM1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", url);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("Certificate");
                adapter.Fill(dataSet);
                DataTable certTable = dataSet.Tables["Certificate"];
                if(certTable.Rows.Count == 0)
                {
                    return null;
                }
                certificate = _certProvider.GetItem<Certificate>(certTable.Rows[0]);
                
                //CertificateContent
                SqlDataAdapter certificateContentAdapter = new SqlDataAdapter();
                certificateContentAdapter.TableMappings.Add("Table", "CertificateContent");
                SqlCommand certificateContentsCommand = new SqlCommand("SELECT * FROM CERTIFICATECONTENT WHERE CERTIFICATEID = @PARAM1", connection);
                certificateContentsCommand.Parameters.AddWithValue("@PARAM1", certificate.CertificateId);
                certificateContentAdapter.SelectCommand = certificateContentsCommand;
                certificateContentAdapter.Fill(dataSet);

                //User
                SqlDataAdapter userAdapter = new SqlDataAdapter();
                userAdapter.TableMappings.Add("Table", "User");
                SqlCommand userCommand = new SqlCommand("SELECT * FROM CERTIFICATE C, [USER] U WHERE C.CERTIFICATEID = @PARAM1 AND C.ROLLNUMBER = U.ROLLNUMBER", connection);
                userCommand.Parameters.AddWithValue("@PARAM1", certificate.CertificateId);
                userAdapter.SelectCommand = userCommand;
                userAdapter.Fill(dataSet);

                //Signature
                SqlDataAdapter signatureAdapter = new SqlDataAdapter();
                signatureAdapter.TableMappings.Add("Table", "Signature");
                SqlCommand signatureCommand = new SqlCommand("SELECT * FROM SIGNATURE WHERE SIGNATUREID = @PARAM1", connection);
                signatureCommand.Parameters.AddWithValue("@PARAM1", certificate.SignatureId);
                signatureAdapter.SelectCommand = signatureCommand;
                signatureAdapter.Fill(dataSet);


                //Close connection
                connection.Close();
                DataTable certContentTable = dataSet.Tables["CertificateContent"];
                DataTable userTable = dataSet.Tables["User"];
                DataTable signatureTable = dataSet.Tables["Signature"];

                //Has certificate content
                if (certContentTable.Rows.Count > 0)
                {
                    certificate.CertificateContents = _certContentProvider.GetListObjects<CertificateContents>(certContentTable.Rows);
                }
                //Has user
                if (userTable.Rows.Count > 0)
                {
                    certificate.User = _userProvider.GetItem<User>(userTable.Rows[0]);
                }
                //Has signature
                if(signatureTable.Rows.Count > 0)
                {
                    certificate.Signature = _signatureProvider.GetItem<Signature>(signatureTable.Rows[0]);
                }
            }
            return certificate;
        }
        public Certificate GetCertificateByRollNumberAndSubjectCode(string rollNumber, string subjectCode)
        {
            Certificate certificate = null;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //Certificate
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Certificate");
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM CERTIFICATE WHERE ROLLNUMBER = @PARAM1 AND SUBJECTCODE = @PARAM2", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", rollNumber);
                command.Parameters.AddWithValue("@PARAM2", subjectCode);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("Certificate");
                adapter.Fill(dataSet);
                
                DataTable certTable = dataSet.Tables["Certificate"];
                if (certTable.Rows.Count == 0)
                {
                    return null;
                }
                certificate = _certProvider.GetItem<Certificate>(certTable.Rows[0]);
                //Does not have that certificate
                //CertificateContent
                SqlDataAdapter certificateContentAdapter = new SqlDataAdapter();
                certificateContentAdapter.TableMappings.Add("Table", "CertificateContent");
                SqlCommand certificateContentsCommand = new SqlCommand("SELECT * FROM CERTIFICATECONTENT WHERE CERTIFICATEID = @PARAM1", connection);
                certificateContentsCommand.Parameters.AddWithValue("@PARAM1", certificate.CertificateId);
                certificateContentAdapter.SelectCommand = certificateContentsCommand;
                certificateContentAdapter.Fill(dataSet);

                //User
                SqlDataAdapter userAdapter = new SqlDataAdapter();
                userAdapter.TableMappings.Add("Table", "User");
                SqlCommand userCommand = new SqlCommand("SELECT * FROM CERTIFICATE C, [USER] U WHERE C.CERTIFICATEID = @PARAM1 AND C.ROLLNUMBER = U.ROLLNUMBER", connection);
                userCommand.Parameters.AddWithValue("@PARAM1", certificate.CertificateId);
                userAdapter.SelectCommand = userCommand;
                //Fill data set
                userAdapter.Fill(dataSet);

                //Signature
                SqlDataAdapter signatureAdapter = new SqlDataAdapter();
                signatureAdapter.TableMappings.Add("Table", "Signature");
                SqlCommand signatureCommand = new SqlCommand("SELECT * FROM SIGNATURE WHERE SIGNATUREID = @PARAM1", connection);
                signatureCommand.Parameters.AddWithValue("@PARAM1", certificate.SignatureId);
                signatureAdapter.SelectCommand = signatureCommand;
                signatureAdapter.Fill(dataSet);

                //Close connection
                connection.Close();
                DataTable certContentTable = dataSet.Tables["CertificateContent"];
                DataTable userTable = dataSet.Tables["User"];
                DataTable signatureTable = dataSet.Tables["Signature"];

                if (certContentTable.Rows.Count > 0)
                {
                    certificate.CertificateContents = _certContentProvider.GetListObjects<CertificateContents>(certContentTable.Rows);
                }
                if (userTable.Rows.Count > 0)
                {
                    certificate.User = _userProvider.GetItem<User>(userTable.Rows[0]);
                }
                //Has signature
                if (signatureTable.Rows.Count > 0)
                {
                    certificate.Signature = _signatureProvider.GetItem<Signature>(signatureTable.Rows[0]);
                }
            }
            return certificate;
        }
        public void AddCertificateContent(List<CertificateContents> contents)
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
                    //Insert to table [CertificateContents]
                    //Change command store procedure name & parameters
                    command.CommandText = "sp_Insert_CertificateContent";
                    foreach (CertificateContents content in contents)
                    {
                        //Remove old parameters
                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@Content", content.Content));
                        command.Parameters.Add(new SqlParameter("@CertificateFormat", content.CertificateFormat));
                        command.Parameters.Add(new SqlParameter("@CertificateId", content.CertificateId));
                        command.ExecuteNonQuery();
                    }

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
        public void AddReport(Report report)
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
                    //Insert to table [Report]
                    command.CommandText = "sp_Insert_Report";
                    command.Parameters.Add(new SqlParameter("@ReportContent", report.ReportContent));
                    command.Parameters.Add(new SqlParameter("@Status", report.Status));
                    command.Parameters.Add(new SqlParameter("@UserId", report.UserId));
                    command.Parameters.Add(new SqlParameter("@Certificateid", report.CertificateId));
                    command.Parameters.Add(new SqlParameter("@Title", report.Title));
                    command.Parameters.Add(new SqlParameter("@CreateTime", report.CreateTime));
                    command.Parameters.Add(new SqlParameter("@UpdateTime", ""));
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
        public void AddCertificate(Certificate certificate)
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
                    //Insert to table [Certificates]
                    command.CommandText = "sp_Insert_Certificate";
                    command.Parameters.Add(new SqlParameter("@CertificateName", certificate.CertificateName));
                    command.Parameters.Add(new SqlParameter("@VerifyCode", certificate.VerifyCode));
                    command.Parameters.Add(new SqlParameter("Url", certificate.Url));
                    command.Parameters.Add(new SqlParameter("@IssuerType", certificate.IssuerType));
                    command.Parameters.Add(new SqlParameter("@IssuerName", String.IsNullOrEmpty(certificate.IssuerName) ? (object)DBNull.Value : certificate.IssuerName));
                    command.Parameters.Add(new SqlParameter("@Description", String.IsNullOrEmpty(certificate.Description) ? (object)DBNull.Value : certificate.Description));
                    command.Parameters.Add(new SqlParameter("@Hashing", certificate.Hashing));
                    command.Parameters.Add(new SqlParameter("@ViewCount", certificate.ViewCount));
                    command.Parameters.Add(new SqlParameter("@DateOfIssue", certificate.DateOfIssue == DateTime.MinValue ? (object)DBNull.Value : certificate.DateOfIssue));
                    command.Parameters.Add(new SqlParameter("@DateOfExpiry", certificate.DateOfExpiry == DateTime.MinValue ? (object)DBNull.Value : certificate.DateOfExpiry));
                    command.Parameters.Add(new SqlParameter("@SubjectCode", certificate.SubjectCode));
                    command.Parameters.Add(new SqlParameter("@RollNumber", certificate.RollNumber));
                    command.Parameters.Add(new SqlParameter("@FullName", certificate.FullName));
                    command.Parameters.Add(new SqlParameter("@Nationality", certificate.Nationality));
                    command.Parameters.Add(new SqlParameter("@PlaceOfBirth", certificate.PlaceOfBirth));
                    command.Parameters.Add(new SqlParameter("@Curriculum", certificate.Curriculum));
                    command.Parameters.Add(new SqlParameter("@GraduationYear", DateTime.Parse(certificate.GraduationYear) == DateTime.MinValue ? (object)DBNull.Value : DateTime.Parse(certificate.GraduationYear)));
                    command.Parameters.Add(new SqlParameter("@GraduationGrade", certificate.GraduationGrade));
                    command.Parameters.Add(new SqlParameter("@GraduationDecisionNumber", certificate.GraduationDecisionNumber));
                    command.Parameters.Add(new SqlParameter("@DiplomaNumber", certificate.DiplomaNumber));
                    command.Parameters.Add(new SqlParameter("@CampusId", certificate.CampusId == 0 ? (object)DBNull.Value : certificate.CampusId));
                    command.Parameters.Add(new SqlParameter("@SignatureId", certificate.SignatureId == 0 ? (object)DBNull.Value : certificate.SignatureId));
                    //Get id of new certificate inserted to the database
                    int insertedCertificateId = Int32.Parse(command.ExecuteScalar().ToString());
                    

                    //Insert to table [CertificateContents]
                    //Change command store procedure name & parameters
                    if(certificate.CertificateContents != null && certificate.CertificateContents.Count > 0)
                    {
                        command.CommandText = "sp_Insert_CertificateContent";
                        foreach (CertificateContents content in certificate.CertificateContents)
                        {
                            //Remove old parameters
                            command.Parameters.Clear();
                            command.Parameters.Add(new SqlParameter("@Content", content.Content));
                            command.Parameters.Add(new SqlParameter("@CertificateFormat", content.CertificateFormat));
                            command.Parameters.Add(new SqlParameter("@CertificateId", insertedCertificateId));
                            command.ExecuteNonQuery();
                        }
                    }
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
       
        public int AddMultipleCertificates(List<Certificate> certificates, int typeImport)
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
                    foreach (Certificate certificate in certificates)
                    {
                        command.Parameters.Clear();
                        //Insert to table [Certificates]
                        command.CommandText = "sp_Insert_Certificate";
                        command.Parameters.Add(new SqlParameter("@CertificateName", certificate.CertificateName));
                        command.Parameters.Add(new SqlParameter("@VerifyCode", certificate.VerifyCode));
                        command.Parameters.Add(new SqlParameter("Url", certificate.Url));
                        command.Parameters.Add(new SqlParameter("@IssuerType", certificate.IssuerType));
                        command.Parameters.Add(new SqlParameter("@IssuerName", certificate.IssuerName));
                        command.Parameters.Add(new SqlParameter("@Description", certificate.Description));
                        command.Parameters.Add(new SqlParameter("@Hashing", certificate.Hashing));
                        command.Parameters.Add(new SqlParameter("@ViewCount", certificate.ViewCount));
                        command.Parameters.Add(new SqlParameter("@DateOfIssue", certificate.DateOfIssue == DateTime.MinValue ? (object)DBNull.Value : certificate.DateOfIssue));
                        command.Parameters.Add(new SqlParameter("@DateOfExpiry", certificate.DateOfExpiry == DateTime.MinValue ? (object)DBNull.Value : certificate.DateOfExpiry));
                        command.Parameters.Add(new SqlParameter("@SubjectCode", certificate.SubjectCode));
                        command.Parameters.Add(new SqlParameter("@RollNumber", certificate.RollNumber));
                        command.Parameters.Add(new SqlParameter("@FullName", certificate.FullName));
                        command.Parameters.Add(new SqlParameter("@Nationality", certificate.Nationality));
                        command.Parameters.Add(new SqlParameter("@PlaceOfBirth", certificate.PlaceOfBirth));
                        command.Parameters.Add(new SqlParameter("@Curriculum", certificate.Curriculum));
                        command.Parameters.Add(new SqlParameter("@GraduationYear", (certificate.GraduationYear == null || (DateTime.Parse(certificate.GraduationYear) == DateTime.MinValue)) ? (object)DBNull.Value : DateTime.Parse(certificate.GraduationYear)));
                        command.Parameters.Add(new SqlParameter("@GraduationGrade", certificate.GraduationGrade));
                        command.Parameters.Add(new SqlParameter("@GraduationDecisionNumber", certificate.GraduationDecisionNumber));
                        command.Parameters.Add(new SqlParameter("@DiplomaNumber", certificate.DiplomaNumber));
                        command.Parameters.Add(new SqlParameter("@CampusId", certificate.CampusId));
                        command.Parameters.Add(new SqlParameter("@SignatureId", certificate.SignatureId == 0 ? (object)DBNull.Value : certificate.SignatureId));
                        //Get id of new certificate inserted to the database
                        int insertedCertificateId = Int32.Parse(command.ExecuteScalar().ToString());


                        if (certificate.CertificateContents != null && certificate.CertificateContents.Count > 0)
                        {
                            command.CommandText = "sp_Insert_CertificateContent";
                            foreach (CertificateContents content in certificate.CertificateContents)
                            {
                                //Remove old parameters
                                command.Parameters.Clear();
                                command.Parameters.Add(new SqlParameter("@Content", content.Content));
                                command.Parameters.Add(new SqlParameter("@CertificateFormat", content.CertificateFormat));
                                command.Parameters.Add(new SqlParameter("@CertificateId", insertedCertificateId));
                                command.ExecuteNonQuery();
                            }
                        }

                        UserViewModel userViewModel = _userServices.GetUserByRollNumber(certificate.RollNumber);
                        if(userViewModel != null)
                        {
                            //Send email to user
                            _emailServices.SendEmail(userViewModel.AcademicEmail, "New Certificate from FPT Education", "You got a new Certificate of " + certificate.CertificateName);
                        }
                        
                    }
                    //Commit the transaction
                    transaction.Commit();
                   
                    return certificates.Count;
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
        public void DeletePersonalCertificate(int certificateId)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    //Delete from table [CertificateContents]
                    command.CommandText = "sp_Delete_CertificateContent";
                    command.Parameters.Add(new SqlParameter("@CertificateId", certificateId));
                    command.ExecuteNonQuery();

                    //Delete from table [Certificates]
                    command.CommandText = "sp_Delete_Certificate";
                    command.ExecuteNonQuery();
                    //Commit the transaction
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("Message: {0}", ex.Message);
                    transaction.Rollback();
                    throw new Exception();
                }
            }
        }

        public Pagination<Certificate> GetCertificatesPagination(string rollNumber, int pageSize, int pageNumber, string keyword)
        {
            List<Certificate> certificates = GetAllCertificates(rollNumber, keyword);

            Pagination<Certificate> pagination = new Pagination<Certificate>().GetPagination(certificates, pageSize, pageNumber);
            return pagination;
        }
        public Pagination<Report> GetReportPagination(int userId, int pageSize, int pageNumber)
        {
            List<Report> reports = GetAllReportById(userId);

            Pagination<Report> pagination = new Pagination<Report>().GetPagination(reports, pageSize, pageNumber);
            return pagination;
        }
        public Pagination<Report> GetAllReportPagination(int pageSize, int pageNumber)
        {
            List<Report> reports = GetAllReport();

            Pagination<Report> pagination = new Pagination<Report>().GetPagination(reports, pageSize, pageNumber);
            return pagination;
        }
        public string GetCertificateContent(int certificateId)
        {
            string content = _certProvider.LIST_STRING("SELECT CONTENT FROM CERTIFICATECONTENT WHERE CERTIFICATEID = @param1 ", new object[] { certificateId }).FirstOrDefault();

            return content;
        }
        //Check if user own certificate
        public bool IsOwnerOfCertificate(string rollNumber, string certUrl)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //Certificate
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Certificate");
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM CERTIFICATE WHERE URL = @PARAM1 AND ROLLNUMBER = @PARAM2", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", certUrl);
                command.Parameters.AddWithValue("@PARAM2", rollNumber);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("Certificate");
                adapter.Fill(dataSet);
                DataTable certTable = dataSet.Tables["Certificate"];
                if (certTable.Rows.Count != 1)
                {
                    return false;
                }
                //Close connection
                connection.Close();
            }
            return true;
        }

        //Test + demo purpose
        public void Test()
        {
            StoreProcedureOption procedureOption = new StoreProcedureOption()
            {
                ProcedureName = "sp_Insert_Organization",
                Parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new SqlParameter("@OrganizationName", "Quay len anh em oi 7"),
                    new SqlParameter("@LogoImage", "An choi Ha Noi 7"),
                }
            };

            StoreProcedureOption procedureOption2 = new StoreProcedureOption()
            {
                ProcedureName = "sp_Insert_Organization",
                Parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new SqlParameter("@OrganizationName", "Quay len anh em oi 7!!!!"),
                    new SqlParameter("@LogoImage", "An choi Ha Noi 7!!!"),
                }
            };
            
        }

        //Get certificate list by list Id
        //public List<Certificate> GetListCertificateByListId(List<int> certIds)
        //{
        //    List<Certificate> certificates = new List<Certificate>();
        //    using (SqlConnection connection = new SqlConnection(connStr))
        //    {
        //        foreach (int certId in certIds)
        //        {
        //            Certificate certificate = new Certificate();
        //            //Certificate
        //            SqlDataAdapter adapter = new SqlDataAdapter();
        //            adapter.TableMappings.Add("Table", "Certificate");
        //            connection.Open();
        //            SqlCommand command = new SqlCommand("SELECT * FROM CERTIFICATE WHERE CERTIFICATEID = @PARAM1", connection);
        //            command.CommandType = CommandType.Text;
        //            command.Parameters.AddWithValue("@PARAM1", certId);
        //            adapter.SelectCommand = command;
        //            //Fill data set
        //            DataSet dataSet = new DataSet("Certificate");
        //            adapter.Fill(dataSet);

        //            //CertificateContent
        //            SqlDataAdapter certificateContentAdapter = new SqlDataAdapter();
        //            certificateContentAdapter.TableMappings.Add("Table", "CertificateContent");
        //            SqlCommand certificateContentsCommand = new SqlCommand("SELECT * FROM CERTIFICATECONTENT WHERE CERTIFICATEID = @PARAM1", connection);
        //            certificateContentsCommand.Parameters.AddWithValue("@PARAM1", certId);
        //            certificateContentAdapter.SelectCommand = certificateContentsCommand;
        //            certificateContentAdapter.Fill(dataSet);

        //            //User
        //            SqlDataAdapter userAdapter = new SqlDataAdapter();
        //            userAdapter.TableMappings.Add("Table", "User");
        //            SqlCommand userCommand = new SqlCommand("SELECT * FROM CERTIFICATE C, [USER] U WHERE C.CERTIFICATEID = @PARAM1 AND C.ROLLNUMBER = U.ROLLNUMBER", connection);
        //            userCommand.Parameters.AddWithValue("@PARAM1", certId);
        //            userAdapter.SelectCommand = userCommand;

        //            //Fill data set
        //            userAdapter.Fill(dataSet);
        //            //Close connection
        //            connection.Close();

        //            DataTable certTable = dataSet.Tables["Certificate"];
        //            DataTable certContentTable = dataSet.Tables["CertificateContent"];
        //            DataTable userTable = dataSet.Tables["User"];


        //            certificate = _certProvider.GetItem<Certificate>(certTable.Rows[0]);
        //            if (certContentTable.Rows.Count > 0)
        //            {
        //                certificate.CertificateContents = _certContentProvider.GetListObjects<CertificateContents>(certContentTable.Rows);
        //            }

        //            if (userTable.Rows.Count > 0)
        //            {
        //                certificate.User = _userProvider.GetItem<User>(userTable.Rows[0]);
        //            }
        //            certificates.Add(certificate);
        //        }

        //    }
        //    return certificates;
        //}
    }
}