using eCert.Models.Entity;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace eCert.Daos
{
    public class CertificateDAO
    {
        private readonly DataProvider<Certificate> _dataProvider;

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

        /**
         * Add, update, delete
         * Example: dataProvinder.ADD("INSERT INTO Person VALUES( @param1 , @param2 )", new object[] { tbName, tbAge });
         */
        public void CreateACertificate(Certificate certificate)
        {
            StoreProcedureOption procedureOption = new StoreProcedureOption()
            {
                ProcedureName = "sp_Insert_Certificates",
                Parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@CertificateName", certificate.CertificateName),
                    new SqlParameter("@VerifyCode", certificate.VerifyCode),
                    new SqlParameter("@Issuer", certificate.Issuer),
                    new SqlParameter("@Format", certificate.Format),
                    new SqlParameter("@Description", certificate.Description),
                    new SqlParameter("@Hashing", certificate.Hashing),
                    new SqlParameter("@ViewCount", certificate.ViewCount),
                    new SqlParameter("@UserId", certificate.UserId),
                    new SqlParameter("@OrganizationId", certificate.OrganizationId),
                    new SqlParameter("@created_at", certificate.created_at),
                    new SqlParameter("@updated_at", certificate.updated_at)
                }
            };


            _dataProvider.ExecuteSqlTransaction(new List<StoreProcedureOption>() { procedureOption });
        }

        public Pagination<Certificate> GetCertificatesPagination(int userId, int pageSize, int pageNumber)
        {
            List<Certificate> certificates = GetAllCertificates(userId);

            Pagination<Certificate> pagination = new Pagination<Certificate>().GetPagination(certificates, pageSize, pageNumber);
            return pagination;
        }

        public void DeleteCertificate(int certificateId)
        {
            string query = "DELETE FROM CERTIFICATES WHERE CERTIFICATEID = @param1";
            _dataProvider.ADD_UPDATE_DELETE(query, new object[] { certificateId });
        }

        public string GetCertificateFileName(int certificateId)
        {
            string fileName = _dataProvider.LIST_STRING("SELECT CONTENT FROM CERTIFICATES WHERE CERTIFICATEID = @param1 ", new object[] { certificateId }).FirstOrDefault();

            return fileName;
        }

        public Certificate GetCertificateByID(int id)
        {
            string query = "SELECT * FROM CERTIFICATES WHERE CERTIFICATEID = @param1 ";
            Certificate certificate = _dataProvider.GetListObjects<Certificate>(query, new object[] { id }).FirstOrDefault();
            return certificate;

        }

        //public void EditCertificate(Certificate cert)
        //{
        //    string query = "UPDATE CERTIFICATES SET CERTIFICATENAME = @param1 , FORMAT = @param2 , DESCRIPTION = @param3 , CONTENT = @param4 WHERE CERTIFICATEID = @param5";
        //    _dataProvider.ADD_UPDATE_DELETE(query, new object[] { cert.CertificateName, cert.Format, cert.Description, cert.Content, cert.CertificateID });
        //}

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
            _dataProvider.ExecuteSqlTransaction(new List<StoreProcedureOption>() { procedureOption, procedureOption2 });
        }
    }
}