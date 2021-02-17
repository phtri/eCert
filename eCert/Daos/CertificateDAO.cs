﻿using eCert.Models.Entity;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;

namespace eCert.Daos
{
    public class CertificateDAO
    {
        private readonly DataProvider<Certificate> _dataProvider;
        string connStr = WebConfigurationManager.ConnectionStrings["Database"].ConnectionString;

        public CertificateDAO()
        {
            _dataProvider = new DataProvider<Certificate>();
        }
        //Get all certificates of a user
        public List<Certificate> GetAllCertificates(int userId)
        {
            string query = "SELECT * FROM CERTIFICATES WHERE USERID = @PARAM1";
            List<Certificate> listCertificate = _dataProvider.GetListObjects<Certificate>(query, new object[] { userId });
            return listCertificate;
        }
        public void AddCertificate(Certificate certificate, List<CertificateContents> contents)
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
                    command.CommandText = "sp_Insert_Certificates";
                    command.Parameters.Add(new SqlParameter("@CertificateName", certificate.CertificateName));
                    command.Parameters.Add(new SqlParameter("@VerifyCode", certificate.VerifyCode));
                    command.Parameters.Add(new SqlParameter("@Issuer", certificate.Issuer));
                    command.Parameters.Add(new SqlParameter("@Description", certificate.Description));
                    command.Parameters.Add(new SqlParameter("@Hashing", certificate.Hashing));
                    command.Parameters.Add(new SqlParameter("@ViewCount", certificate.ViewCount));
                    command.Parameters.Add(new SqlParameter("@DateOfIssue", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("@DateOfExpiry", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("@UserId", certificate.UserId));
                    command.Parameters.Add(new SqlParameter("@OrganizationId", certificate.OrganizationId));
                    command.Parameters.Add(new SqlParameter("@created_at", certificate.created_at));
                    command.Parameters.Add(new SqlParameter("@updated_at", certificate.updated_at));

                    //Get id of new certificate inserted to the database
                    int insertedCertificateId = Int32.Parse(command.ExecuteScalar().ToString());
                    string x = "hello world";

                    //Insert to table [CertificateContents]
                    //Change command store procedure name & parameters
                    command.CommandText = "sp_Insert_CertificateContents";
                    foreach (CertificateContents content in contents)
                    {
                        //Remove old parameters
                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@Content", content.Content));
                        command.Parameters.Add(new SqlParameter("@Format", content.Format));
                        command.Parameters.Add(new SqlParameter("@CertificateId", insertedCertificateId));
                        command.Parameters.Add(new SqlParameter("@created_at", content.created_at));
                        command.Parameters.Add(new SqlParameter("@updated_at", content.updated_at));
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
                    command.CommandText = "sp_Delete_CertificateContents";
                    command.Parameters.Add(new SqlParameter("@CertificateId", certificateId));
                    command.ExecuteNonQuery();

                    //Delete from table [Certificates]
                    command.CommandText = "sp_Delete_Certificates";
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

        public Pagination<Certificate> GetCertificatesPagination(int userId, int pageSize, int pageNumber)
        {
            List<Certificate> certificates = GetAllCertificates(userId);

            Pagination<Certificate> pagination = new Pagination<Certificate>().GetPagination(certificates, pageSize, pageNumber);
            return pagination;
        }
        
        public string GetCertificateContent(int certificateId)
        {
            string content = _dataProvider.LIST_STRING("SELECT CONTENT FROM CERTIFICATECONTENTS WHERE CERTIFICATEID = @param1 ", new object[] { certificateId }).FirstOrDefault();

            return content;
        }
        public Certificate GetCertificateByID(int id)
        {
            string query = "SELECT * FROM CERTIFICATES WHERE CERTIFICATEID = @param1 ";
            Certificate certificate = _dataProvider.GetListObjects<Certificate>(query, new object[] { id }).FirstOrDefault();
            return certificate;
        }
        public CertificateContents GetCertificateContentsByID(int id)
        {
            string query = "SELECT * FROM CERTIFICATESCONTENTS WHERE CERTIFICATEID = @param1 ";
            Certificate certificate = _dataProvider.GetListObjects<Certificate>(query, new object[] { id }).FirstOrDefault();
            return certificate;
        }
        //Test + demo purpose
        public void Test()
        {
            StoreProcedureOption procedureOption = new StoreProcedureOption()
            {
                ProcedureName = "sp_Insert_Organizations",
                Parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new SqlParameter("@OrganizationName", "Quay len anh em oi 7"),
                    new SqlParameter("@LogoImage", "An choi Ha Noi 7"),
                }
            };

            StoreProcedureOption procedureOption2 = new StoreProcedureOption()
            {
                ProcedureName = "sp_Insert_Organizations",
                Parameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new SqlParameter("@OrganizationName", "Quay len anh em oi 7!!!!"),
                    new SqlParameter("@LogoImage", "An choi Ha Noi 7!!!"),
                }
            };
            
        }
    }
}