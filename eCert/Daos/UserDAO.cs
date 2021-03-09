using eCert.Models.Entity;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace eCert.Daos
{
    public class UserDAO
    {
        public UserDAO()
        {
            _userProvider = new DataProvider<User>();
        }
        private readonly DataProvider<User> _userProvider;
        string connStr = WebConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        public User GetUserByAcademicEmail(string email)
        {
            string query = "SELECT * FROM [User] where AcademicEmail = @param1";
            User user = _userProvider.GetObject<User>(query, new object[] { email });
            return user;
        }

        public void AddUser(User user)
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
                    command.Parameters.Add(new SqlParameter("@PasswordHash", user.PasswordHash));
                    command.Parameters.Add(new SqlParameter("@PasswordSalt", user.PasswordSalt));
                    command.Parameters.Add(new SqlParameter("@Gender", user.Gender));
                    command.Parameters.Add(new SqlParameter("@DOB", user.DOB.Date));
                    command.Parameters.Add(new SqlParameter("@PhoneNumber", user.PhoneNumber));
                    command.Parameters.Add(new SqlParameter("@PersonalEmail", user.PersonalEmail));
                    command.Parameters.Add(new SqlParameter("@AcademicEmail", user.AcademicEmail));
                    command.Parameters.Add(new SqlParameter("@RollNumber", user.RollNumber));
                    command.Parameters.Add(new SqlParameter("@Ethnicity", user.Ethnicity));
                    command.Parameters.Add(new SqlParameter("@RoleId", user.RoleId));
                    //Get id of new certificate inserted to the database
                    int insertedUserteId = Int32.Parse(command.ExecuteScalar().ToString());

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