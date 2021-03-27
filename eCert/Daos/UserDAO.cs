﻿using eCert.Models.Entity;
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
            _roleProvider = new DataProvider<Role>();
        }
        private readonly DataProvider<User> _userProvider;
        private readonly DataProvider<Role> _roleProvider;
        string connStr = WebConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        public User GetUserByAcademicEmail(string email)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //User
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "User");
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [USER] WHERE ACADEMICEMAIL = @PARAM1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", email);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("User");
                adapter.Fill(dataSet);

                //Role
                SqlDataAdapter roleAdapter = new SqlDataAdapter();
                roleAdapter.TableMappings.Add("Table", "Role");
                SqlCommand roleCommand = new SqlCommand("SELECT * FROM USER_ROLE UR, [ROLE] R, [USER] U  where UR.USERID = U.USERID AND U.ACADEMICEMAIL = @PARAM1 AND UR.RoleID = R.RoleID ", connection);
                roleCommand.Parameters.AddWithValue("@PARAM1", email);
                roleAdapter.SelectCommand = roleCommand;
                roleAdapter.Fill(dataSet);

                //Close connection
                connection.Close();

                DataTable userTable = dataSet.Tables["User"];
                DataTable roleTable = dataSet.Tables["Role"];

                user = _userProvider.GetItem<User>(userTable.Rows[0]);
                user.Role = _roleProvider.GetItem<Role>(roleTable.Rows[0]);


            }
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
                    
                    //Get id of new certificate inserted to the database
                    int insertedUserteId = Int32.Parse(command.ExecuteScalar().ToString());

                    //Insert to table [User_Role]
                    command.Parameters.Clear();
                    command.CommandText = "sp_Insert_User_Role";
                    command.Parameters.Add(new SqlParameter("@UserId", insertedUserteId));
                    command.Parameters.Add(new SqlParameter("@RoleId", user.Role.RoleId));
                    command.ExecuteNonQuery();

                    //Get all inserted certificate of new user


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
        public User GetUserByProvidedEmailAndPass(string email, string password)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //User
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "User");
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [USER] WHERE ACADEMICEMAIL = @PARAM1 AND PASSWORDHASH = @PARAM2", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", email);
                command.Parameters.AddWithValue("@PARAM2", password);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("User");
                adapter.Fill(dataSet);

                //Role
                SqlDataAdapter roleAdapter = new SqlDataAdapter();
                roleAdapter.TableMappings.Add("Table", "Role");
                SqlCommand roleCommand = new SqlCommand("SELECT * FROM USER_ROLE UR, [ROLE] R, [USER] U  where UR.USERID = U.USERID AND U.ACADEMICEMAIL = @PARAM1 AND U.PASSWORDHASH = @PARAM2 AND UR.RoleID = R.RoleID ", connection);
                roleCommand.Parameters.AddWithValue("@PARAM1", email);
                roleCommand.Parameters.AddWithValue("@PARAM2", password);
                roleAdapter.SelectCommand = roleCommand;
                roleAdapter.Fill(dataSet);

                //Close connection
                connection.Close();

                DataTable userTable = dataSet.Tables["User"];
                DataTable roleTable = dataSet.Tables["Role"];
                if(userTable.Rows.Count == 0)
                {
                    return null;
                }

                user = _userProvider.GetItem<User>(userTable.Rows[0]);
                user.Role = _roleProvider.GetItem<Role>(roleTable.Rows[0]);
            }
            return user;
        }
        public User GetUserByRollNumber(string rollNumber)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //User
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "User");
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [USER] WHERE ROLLNUMBER = @PARAM1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", rollNumber);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("User");
                adapter.Fill(dataSet);

                //Role
                SqlDataAdapter roleAdapter = new SqlDataAdapter();
                roleAdapter.TableMappings.Add("Table", "Role");
                SqlCommand roleCommand = new SqlCommand("SELECT * FROM USER_ROLE UR, [ROLE] R, [USER] U  where UR.USERID = U.USERID AND U.ROLLNUMBER = @PARAM1 AND UR.RoleID = R.RoleID ", connection);
                roleCommand.Parameters.AddWithValue("@PARAM1", rollNumber);
                roleAdapter.SelectCommand = roleCommand;
                roleAdapter.Fill(dataSet);

                //Close connection
                connection.Close();

                DataTable userTable = dataSet.Tables["User"];
                DataTable roleTable = dataSet.Tables["Role"];

                user = _userProvider.GetItem<User>(userTable.Rows[0]);
                user.Role = _roleProvider.GetItem<Role>(roleTable.Rows[0]);

               
            }
            return user;
        }
        public User GetUserByUserId(int userId)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //User
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "User");
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [USER] WHERE USERID = @PARAM1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", userId);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("User");
                adapter.Fill(dataSet);

                ////Role
                //SqlDataAdapter roleAdapter = new SqlDataAdapter();
                //roleAdapter.TableMappings.Add("Table", "Role");
                //SqlCommand roleCommand = new SqlCommand("SELECT * FROM USER_ROLE UR, [ROLE] R, [USER] U  where UR.USERID = U.USERID AND U.ROLLNUMBER = @PARAM1 AND UR.RoleID = R.RoleID ", connection);
                //roleCommand.Parameters.AddWithValue("@PARAM1", rollNumber);
                //roleAdapter.SelectCommand = roleCommand;
                //roleAdapter.Fill(dataSet);

                //Close connection
                connection.Close();

                DataTable userTable = dataSet.Tables["User"];
                //DataTable roleTable = dataSet.Tables["Role"];

                user = _userProvider.GetItem<User>(userTable.Rows[0]);
                //user.Role = _roleProvider.GetItem<Role>(roleTable.Rows[0]);


            }
            return user;
        }
        public void DeleteUser(int userId)
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
                    //Delete from table [User_Role]
                    command.CommandText = "sp_Delete_User_Role";
                    command.Parameters.Add(new SqlParameter("@UserId", userId));
                    command.ExecuteNonQuery();

                    //Delete from table [User]
                    command.CommandText = "sp_Delete_User";
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

        public void UpdateUser(User user)
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
                    command.CommandText = "sp_Update_User";
                    command.Parameters.Add(new SqlParameter("@UserId", user.UserId));
                    command.Parameters.Add(new SqlParameter("@PasswordHash", user.PasswordHash));
                    command.Parameters.Add(new SqlParameter("@PasswordSalt", user.PasswordSalt));
                    command.Parameters.Add(new SqlParameter("@Gender", user.Gender));
                    command.Parameters.Add(new SqlParameter("@DOB", user.DOB));
                    command.Parameters.Add(new SqlParameter("@PhoneNumber", user.PhoneNumber));
                    command.Parameters.Add(new SqlParameter("@PersonalEmail", user.PersonalEmail));
                    command.Parameters.Add(new SqlParameter("@AcademicEmail", user.AcademicEmail));
                    command.Parameters.Add(new SqlParameter("@RollNumber", user.RollNumber));
                    command.Parameters.Add(new SqlParameter("@Ethnicity", user.Ethnicity));


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