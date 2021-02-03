﻿using eCert.Models;
using System.Collections.Generic;

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
        public void CreateACertificate(Certificate c)
        {
            string sqlCommand = "INSERT INTO CERTIFICATES VALUES( @param1 , @param2 , @param3 , @param4 , @param5 , @param6 , @param7 , @param8 , @param9 , @param10 , @param11 , @param12 )";
            _dataProvider.ADD_UPDATE_DELETE(sqlCommand, new object[] { c.CertificateName, c.VerifyCode, c.FileName, c.Type, c.Format, c.Description, c.Content, c.Hashing, c.UserId, c.OrganizationId, c.created_at, c.updated_at });
        }

        

    }
}