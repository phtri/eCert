using eCert.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Daos
{
    public class CertificateDAO
    {
        private readonly DataProvider _dataProvider;
        
        public CertificateDAO()
        {
            _dataProvider = new DataProvider();
        }

        /**
         * Add, update, delete
         * Example: dataProvinder.ADD("INSERT INTO Person VALUES( @param1 , @param2 )", new object[] { tbName, tbAge });
         */
        public void CreateCertificate(Certificate c)
        {
            _dataProvider.ADD_UPDATE_DELETE("INSERT INTO Certificates VALUES( @param1 , @param2 , @param3 , @param4 , @param5 , @param5 , @param6 , @param7 , @param8 , @param9 , @param10 , @param11 , @param12 )", new object[] { c.CertificateName, c.VerifyCode, c.FileName, c.Type, c.Format, c.Description, c.Content, c.Hashing, c.UserId, c.OrganizationId, c.created_at, c.updated_at });
        }

        //data.ADD_UPDATE_DELETE("INSERT INTO [User] VALUES( @param1 , @param2 , @param3 , @param4 , @param5 , @param6 )",
               // new object[] { code, name, sex, dob, email, roleid });


    }
}