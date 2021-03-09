using eCert.Models.Entity;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Daos
{
    public class UserDAO
    {
        public UserDAO()
        {
            _userProvider = new DataProvider<User>();
        }
        private readonly DataProvider<User> _userProvider;
        public User GetUserByAcademicEmail(string email)
        {
            string query = "SELECT * FROM [User] where AcademicEmail = @param1";
            User user = _userProvider.GetObject<User>(query, new object[] { email });
            return user;
        }

        //public void AddUser(User user)
        //{
        //    using (SqlConnection connection = new SqlConnection(connStr))
        //    {
        //        connection.Open();
        //        SqlCommand command = connection.CreateCommand();
        //        SqlTransaction transaction;
        //        transaction = connection.BeginTransaction("eCert_Transaction");
        //        command.Connection = connection;
        //        command.Transaction = transaction;
        //        command.CommandType = CommandType.StoredProcedure;
        //        try
        //        {
        //            //Insert to table [Certificates]
        //            command.CommandText = "sp_Insert_Certificate";
        //            command.Parameters.Add(new SqlParameter("@CertificateName", certificate.CertificateName));
        //            command.Parameters.Add(new SqlParameter("@VerifyCode", certificate.VerifyCode));
        //            command.Parameters.Add(new SqlParameter("Url", certificate.Url));
        //            command.Parameters.Add(new SqlParameter("@Issuer", certificate.Issuer));
        //            command.Parameters.Add(new SqlParameter("@Description", certificate.Description));
        //            command.Parameters.Add(new SqlParameter("@Hashing", certificate.Hashing));
        //            command.Parameters.Add(new SqlParameter("@ViewCount", certificate.ViewCount));
        //            command.Parameters.Add(new SqlParameter("@DateOfIssue", DateTime.Now));
        //            command.Parameters.Add(new SqlParameter("@DateOfExpiry", DateTime.Now));
        //            command.Parameters.Add(new SqlParameter("@SubjectCode", certificate.SubjectCode));
        //            command.Parameters.Add(new SqlParameter("@RollNumber", certificate.RollNumber));
        //            command.Parameters.Add(new SqlParameter("@FullName", certificate.FullName));
        //            command.Parameters.Add(new SqlParameter("@Nationality", certificate.Nationality));
        //            command.Parameters.Add(new SqlParameter("@PlaceOfBirth", certificate.PlaceOfBirth));
        //            command.Parameters.Add(new SqlParameter("@Curriculum", certificate.Curriculum));
        //            command.Parameters.Add(new SqlParameter("@GraduationYear", certificate.GraduationYear == DateTime.MinValue ? (object)DBNull.Value : certificate.GraduationYear));
        //            command.Parameters.Add(new SqlParameter("@GraduationGrade", certificate.GraduationGrade));
        //            command.Parameters.Add(new SqlParameter("@GraduationDecisionNumber", certificate.GraduationDecisionNumber));
        //            command.Parameters.Add(new SqlParameter("@DiplomaNumber", certificate.DiplomaNumber));
        //            command.Parameters.Add(new SqlParameter("@OrganizationId", certificate.OrganizationId));
        //            //Get id of new certificate inserted to the database
        //            int insertedCertificateId = Int32.Parse(command.ExecuteScalar().ToString());


        //            //Insert to table [CertificateContents]
        //            //Change command store procedure name & parameters
        //            command.CommandText = "sp_Insert_CertificateContent";
        //            foreach (CertificateContents content in certificate.CertificateContents)
        //            {
        //                //Remove old parameters
        //                command.Parameters.Clear();
        //                command.Parameters.Add(new SqlParameter("@Content", content.Content));
        //                command.Parameters.Add(new SqlParameter("@CertificateFormat", content.CertificateFormat));
        //                command.Parameters.Add(new SqlParameter("@CertificateId", insertedCertificateId));
        //                command.ExecuteNonQuery();
        //            }

        //            //Insert to table [Certificate_User]
        //            command.Parameters.Clear();
        //            command.CommandText = "sp_Insert_Certificate_User";
        //            command.Parameters.Add(new SqlParameter("@UserId", certificate.User.UserId));
        //            command.Parameters.Add(new SqlParameter("@CertificateId", insertedCertificateId));
        //            command.ExecuteNonQuery();

        //            //Commit the transaction
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
        //            Console.WriteLine("  Message: {0}", ex.Message);

        //            transaction.Rollback();
        //            throw new Exception();
        //        }

        //    }
        //}


    }
}