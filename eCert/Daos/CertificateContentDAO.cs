using eCert.Models.Entity;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace eCert.Daos
{
    public class CertificateContentDAO
    {
        private readonly DataProvider<CertificateContents> _dataProvider;

        public CertificateContentDAO()
        {
            _dataProvider = new DataProvider<CertificateContents>();
        }

        public int CreateACertificateContent(CertificateContents certificateContents)
        {

            StoreProcedureOption insertCertificateContent = new StoreProcedureOption()
            {
                ProcedureName = "sp_Insert_CertificateContents",
                Parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Content", certificateContents.Content),
                    new SqlParameter("@Format", certificateContents.Format),
                    new SqlParameter("@CertificateId", certificateContents.CertificateId),
                    new SqlParameter("@created_at", certificateContents.created_at),
                    new SqlParameter("@updated_at", certificateContents.updated_at)
                }
            };


            return _dataProvider.ExecuteSqlTransaction(insertCertificateContent);
        }

    }
}