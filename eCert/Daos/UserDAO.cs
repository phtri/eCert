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
        private readonly DataProvider<Staff> _userAcaProvider;
        private readonly DataProvider<User> _userProvider;
        private readonly DataProvider<Role> _roleProvider;
        string connStr = WebConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        public UserDAO()
        {
            _userProvider = new DataProvider<User>();
            _roleProvider = new DataProvider<Role>();
            _userAcaProvider = new DataProvider<Staff>();
        }
       
        public User GetAcaServiceByCampusId(int campusId)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //User
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "User");
                connection.Open();
                SqlCommand command = new SqlCommand("select [User].* from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and Campus.CampusId = @PARAM1 and Role.RoleName = 'Academic Service'", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", campusId);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("User");
                adapter.Fill(dataSet);

                //Close connection
                connection.Close();
                DataTable userTable = dataSet.Tables["User"];
                if(userTable.Rows.Count != 0)
                {
                    user = _userProvider.GetItem<User>(userTable.Rows[0]);
                }
                else
                {
                    return null;
                }
               
            }
            return user;
        }
        public User GetAdminByCampusId(int campusId)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //User
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "User");
                connection.Open();
                SqlCommand command = new SqlCommand("select [User].* from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and Campus.CampusId = @PARAM1 and Role.RoleName = 'Admin'", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", campusId);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("User");
                adapter.Fill(dataSet);

                //Close connection
                connection.Close();
                DataTable userTable = dataSet.Tables["User"];
                if (userTable.Rows.Count != 0)
                {
                    user = _userProvider.GetItem<User>(userTable.Rows[0]);
                }
                else
                {
                    return null;
                }

            }
            return user;
        }
        public User GetActiveAdminByCampusId(int campusId)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //User
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "User");
                connection.Open();
                SqlCommand command = new SqlCommand("select [User].* from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and Campus.CampusId = @PARAM1 and Role.RoleName = 'Admin' and [User_Role].IsActive = 1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", campusId);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("User");
                adapter.Fill(dataSet);

                //Close connection
                connection.Close();
                DataTable userTable = dataSet.Tables["User"];
                if (userTable.Rows.Count != 0)
                {
                    user = _userProvider.GetItem<User>(userTable.Rows[0]);
                }
                else
                {
                    return null;
                }

            }
            return user;
        }
        public User GetActiveAcaServiceByCampusId(int campusId)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //User
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "User");
                connection.Open();
                SqlCommand command = new SqlCommand("select [User].* from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and Campus.CampusId = @PARAM1 and Role.RoleName = 'Academic Service' and [User_Role].IsActive = 1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", campusId);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("User");
                adapter.Fill(dataSet);

                //Close connection
                connection.Close();
                DataTable userTable = dataSet.Tables["User"];
                if (userTable.Rows.Count != 0)
                {
                    user = _userProvider.GetItem<User>(userTable.Rows[0]);
                }
                else
                {
                    return null;
                }

            }
            return user;
        }
        public User GetAdminByUserId(int userId)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //User
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "User");
                connection.Open();
                SqlCommand command = new SqlCommand("select [User].* from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and Role.RoleName = 'Admin' and [User_Role].IsActive = 1 and [User].UserId = @PARAM1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", userId);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("User");
                adapter.Fill(dataSet);

                //Close connection
                connection.Close();
                DataTable userTable = dataSet.Tables["User"];
                if (userTable.Rows.Count != 0)
                {
                    user = _userProvider.GetItem<User>(userTable.Rows[0]);
                }
                else
                {
                    return null;
                }

            }
            return user;
        }
        public User GetAcaServiceByUserId(int userId)
        {
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //User
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "User");
                connection.Open();
                SqlCommand command = new SqlCommand("select [User].* from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and Role.RoleName = 'Academic Service' and [User_Role].IsActive = 1 and [User].UserId = @PARAM1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", userId);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("User");
                adapter.Fill(dataSet);

                //Close connection
                connection.Close();
                DataTable userTable = dataSet.Tables["User"];
                if (userTable.Rows.Count != 0)
                {
                    user = _userProvider.GetItem<User>(userTable.Rows[0]);
                }
                else
                {
                    return null;
                }

            }
            return user;
        }
        public User GetUserByMemberCode(string memberCode)
        {
            User user = null;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //User
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "User");
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [USER] WHERE MEMBERCODE = @PARAM1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", memberCode);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("User");
                adapter.Fill(dataSet);
                //Role
                SqlDataAdapter roleAdapter = new SqlDataAdapter();
                roleAdapter.TableMappings.Add("Table", "Role");
                SqlCommand roleCommand = new SqlCommand("SELECT * FROM USER_ROLE UR, [ROLE] R, [USER] U  where UR.USERID = U.USERID AND U.MEMBERCODE = @PARAM1 AND UR.RoleID = R.RoleID ", connection);
                roleCommand.Parameters.AddWithValue("@PARAM1", memberCode);
                roleAdapter.SelectCommand = roleCommand;
                roleAdapter.Fill(dataSet);
                //Close connection
                connection.Close();
                DataTable userTable = dataSet.Tables["User"];
                DataTable roleTable = dataSet.Tables["Role"];
                if (userTable.Rows.Count != 0)
                {
                    user = _userProvider.GetItem<User>(userTable.Rows[0]);
                    user.Role = _roleProvider.GetItem<Role>(roleTable.Rows[0]);
                }
            }
            return user;
        }
        public User GetUserByAcademicEmail(string email)
        {
            User user = null;
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
                if(userTable.Rows.Count != 0)
                {
                    user = _userProvider.GetItem<User>(userTable.Rows[0]);
                    user.Role = _roleProvider.GetItem<Role>(roleTable.Rows[0]);
                }
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
                    command.Parameters.Add(new SqlParameter("@Gender", user.Gender));
                    command.Parameters.Add(new SqlParameter("@DOB", user.DOB == DateTime.MinValue ? (object)DBNull.Value : user.DOB));
                    command.Parameters.Add(new SqlParameter("@PhoneNumber", user.PhoneNumber));
                    command.Parameters.Add(new SqlParameter("@PersonalEmail", user.PersonalEmail));
                    command.Parameters.Add(new SqlParameter("@AcademicEmail", user.AcademicEmail));
                    command.Parameters.Add(new SqlParameter("@RollNumber", user.RollNumber));
                    command.Parameters.Add(new SqlParameter("@MemberCode", user.MemberCode));
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
                if(userTable.Rows.Count != 0)
                {
                    user = _userProvider.GetItem<User>(userTable.Rows[0]);
                    user.Role = _roleProvider.GetItem<Role>(roleTable.Rows[0]);
                }
                else
                {
                    return null;
                }
                

               
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
        public int GetNumberOfAcaRole(int userId)
        {
            string query = "select [User].*, Campus.CampusId, EducationSystem.EducationName, Campus.CampusName  from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and Role.RoleName = 'Academic Service' and [User].UserId = @PARAM1";

            List<Staff> listAcademicService = _userAcaProvider.GetListObjects<Staff>(query, new object[] { userId });
            return listAcademicService.Count;
        }
        public void DeleteUserAcademicService(int userId, int campusId, int roleId)
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
                int numOfAcaServiceRole = GetNumberOfAcaRole(userId);
                try
                {
                    //Delete from table [User_Role]
                    command.CommandText = "sp_Delete_User_Role";
                    command.Parameters.Add(new SqlParameter("@UserId", userId));
                    command.Parameters.Add(new SqlParameter("@RoleId", roleId));
                    command.ExecuteNonQuery();
                    
                    //Delete from [Role]
                    command.Parameters.Clear();
                    command.CommandText = "sp_Delete_Role_AcademicService";
                    command.Parameters.Add(new SqlParameter("@CampusId", campusId));
                    command.ExecuteNonQuery();

                    if (numOfAcaServiceRole == 1)
                    {
                        //Delete from table [User]
                        command.Parameters.Clear();
                        command.CommandText = "sp_Delete_User";
                        command.Parameters.Add(new SqlParameter("@UserId", userId));
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
        public int GetNumberOfAdminRole(int userId)
        {
            string query = "select [User].*, Campus.CampusId, EducationSystem.EducationName, Campus.CampusName  from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and Role.RoleName = 'Admin' and [User].UserId = @PARAM1";

            List<Staff> listAdmins = _userAcaProvider.GetListObjects<Staff>(query, new object[] { userId });
            return listAdmins.Count;
        }
        public void DeleteAdmin(int userId, int campusId, int roleId)
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

                int numberOfUserRole = GetNumberOfAdminRole(userId);
                try
                {
                    //Delete from table [User_Role]
                    command.Parameters.Clear();
                    command.CommandText = "sp_Delete_User_Role";
                    command.Parameters.Add(new SqlParameter("@UserId", userId));
                    command.Parameters.Add(new SqlParameter("@RoleId", roleId));
                    command.ExecuteNonQuery();

                    //Delete from [Role]
                    command.Parameters.Clear();
                    command.CommandText = "[sp_Delete_Role_Admin]";
                    command.Parameters.Add(new SqlParameter("@CampusId", campusId));
                    command.ExecuteNonQuery();

                    if(numberOfUserRole == 1)
                    {
                        //Delete from table [User]
                        command.Parameters.Clear();
                        command.CommandText = "sp_Delete_User";
                        command.Parameters.Add(new SqlParameter("@UserId", userId));
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
                    command.Parameters.Add(new SqlParameter("@Gender", user.Gender));
                    command.Parameters.Add(new SqlParameter("@DOB", user.DOB == DateTime.MinValue ? (object)DBNull.Value : user.DOB));
                    command.Parameters.Add(new SqlParameter("@PhoneNumber", user.PhoneNumber));
                    command.Parameters.Add(new SqlParameter("@PersonalEmail", user.PersonalEmail));
                    command.Parameters.Add(new SqlParameter("@AcademicEmail", user.AcademicEmail));
                    command.Parameters.Add(new SqlParameter("@RollNumber", user.RollNumber));
                    command.Parameters.Add(new SqlParameter("@MemberCode", user.MemberCode));
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
        public Role GetRoleByRoleName(string roleName)
        {
            Role role = new Role();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //User
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Role");
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [ROLE] WHERE ROLENAME = @PARAM1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", roleName);
                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("User");
                adapter.Fill(dataSet);
                //Close connection
                connection.Close();
                DataTable roleTable = dataSet.Tables["Role"];
                role = _roleProvider.GetItem<Role>(roleTable.Rows[0]);
            }
            return role;
        }
        public void UpdateUserRole(UserRole userRole)
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
                    command.CommandText = "sp_Update_User_Role";
                    command.Parameters.Add(new SqlParameter("@UserId", userRole.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", userRole.RoleId));
                    command.Parameters.Add(new SqlParameter("@IsActive", userRole.IsActive));

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