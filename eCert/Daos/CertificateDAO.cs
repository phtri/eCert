using eCert.Models.Entity;
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
        string connStr = WebConfigurationManager.ConnectionStrings["Database"].ConnectionString;

        public CertificateDAO()
        {
            _certProvider = new DataProvider<Certificate>();
            _certContentProvider = new DataProvider<CertificateContents>();
            _userProvider = new DataProvider<User>();
        }
        //Get all certificates of a user
        public List<Certificate> GetAllCertificates(string rollNumber, string keyword)
        {
            string query = string.Empty;
            List<Certificate> listCertificate = new List<Certificate>();
            if (String.IsNullOrEmpty(keyword))
            {
                query = "SELECT * FROM CERTIFICATE WHERE ROLLNUMBER = @PARAM1";
                listCertificate = _certProvider.GetListObjects<Certificate>(query, new object[] { rollNumber });
            }
            else
            {
                query = "SELECT * FROM CERTIFICATE WHERE ROLLNUMBER = @PARAM1 AND CERTIFICATENAME LIKE N'%PARAM2%'";
                listCertificate = _certProvider.GetListObjects<Certificate>(query, new object[] { rollNumber, keyword });
            }
            
            return listCertificate;
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
                //Close connection
                connection.Close();

                DataTable certTable = dataSet.Tables["Certificate"];
                DataTable certContentTable = dataSet.Tables["CertificateContent"];
                DataTable userTable = dataSet.Tables["User"];


                certificate = _certProvider.GetItem<Certificate>(certTable.Rows[0]);
                if(certContentTable.Rows.Count > 0)
                {
                    certificate.CertificateContents = _certContentProvider.GetListObjects<CertificateContents>(certContentTable.Rows);
                }
                
                if(userTable.Rows.Count > 0)
                {
                    certificate.User = _userProvider.GetItem<User>(userTable.Rows[0]);
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
                    command.Parameters.Add(new SqlParameter("@Issuer", certificate.Issuer));
                    command.Parameters.Add(new SqlParameter("@Description", certificate.Description));
                    command.Parameters.Add(new SqlParameter("@Hashing", certificate.Hashing));
                    command.Parameters.Add(new SqlParameter("@ViewCount", certificate.ViewCount));
                    command.Parameters.Add(new SqlParameter("@DateOfIssue", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("@DateOfExpiry", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("@SubjectCode", certificate.SubjectCode));
                    command.Parameters.Add(new SqlParameter("@RollNumber", certificate.RollNumber));
                    command.Parameters.Add(new SqlParameter("@FullName", certificate.FullName));
                    command.Parameters.Add(new SqlParameter("@Nationality", certificate.Nationality));
                    command.Parameters.Add(new SqlParameter("@PlaceOfBirth", certificate.PlaceOfBirth));
                    command.Parameters.Add(new SqlParameter("@Curriculum", certificate.Curriculum));
                    command.Parameters.Add(new SqlParameter("@GraduationYear", certificate.GraduationYear == DateTime.MinValue ? (object)DBNull.Value : certificate.GraduationYear));
                    command.Parameters.Add(new SqlParameter("@GraduationGrade", certificate.GraduationGrade));
                    command.Parameters.Add(new SqlParameter("@GraduationDecisionNumber", certificate.GraduationDecisionNumber));
                    command.Parameters.Add(new SqlParameter("@DiplomaNumber", certificate.DiplomaNumber));
                    command.Parameters.Add(new SqlParameter("@OrganizationId", certificate.OrganizationId));
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
       
        public void AddMultipleCertificates(List<Certificate> certificates, int typeImport)
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
                        command.Parameters.Add(new SqlParameter("@Issuer", certificate.Issuer));
                        command.Parameters.Add(new SqlParameter("@Description", certificate.Description));
                        command.Parameters.Add(new SqlParameter("@Hashing", certificate.Hashing));
                        command.Parameters.Add(new SqlParameter("@ViewCount", certificate.ViewCount));
                        command.Parameters.Add(new SqlParameter("@DateOfIssue", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("@DateOfExpiry", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("@SubjectCode", certificate.SubjectCode));
                        command.Parameters.Add(new SqlParameter("@RollNumber", certificate.RollNumber));
                        command.Parameters.Add(new SqlParameter("@FullName", certificate.FullName));
                        command.Parameters.Add(new SqlParameter("@Nationality", certificate.Nationality));
                        command.Parameters.Add(new SqlParameter("@PlaceOfBirth", certificate.PlaceOfBirth));
                        command.Parameters.Add(new SqlParameter("@Curriculum", certificate.Curriculum));
                        command.Parameters.Add(new SqlParameter("@GraduationYear", certificate.GraduationYear == DateTime.MinValue ? (object)DBNull.Value : certificate.GraduationYear));
                        command.Parameters.Add(new SqlParameter("@GraduationGrade", certificate.GraduationGrade));
                        command.Parameters.Add(new SqlParameter("@GraduationDecisionNumber", certificate.GraduationDecisionNumber));
                        command.Parameters.Add(new SqlParameter("@DiplomaNumber", certificate.DiplomaNumber));
                        command.Parameters.Add(new SqlParameter("@OrganizationId", certificate.OrganizationId));
                       

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
        public void DeleteCertificate(int certificateId)
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
                    Console.WriteLine("  Message: {0}", ex.Message);
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
        
        public string GetCertificateContent(int certificateId)
        {
            string content = _certProvider.LIST_STRING("SELECT CONTENT FROM CERTIFICATECONTENT WHERE CERTIFICATEID = @param1 ", new object[] { certificateId }).FirstOrDefault();

            return content;
        }
        //public Certificate GetCertificateByID(int id)
        //{
        //    string query = "SELECT * FROM CERTIFICATES WHERE CERTIFICATEID = @param1 ";
        //    Certificate certificate = _dataProvider.GetListObjects<Certificate>(query, new object[] { id }).FirstOrDefault();
        //    return certificate;

        //}
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
    }
}