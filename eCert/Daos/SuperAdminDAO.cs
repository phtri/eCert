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
  
    public class SuperAdminDAO
    {
        private readonly DataProvider<EducationSystem> _eduSystemProvider;
        private readonly DataProvider<Campus> _campusProvider;
        private readonly DataProvider<UserAcaService> _userAcaProvider;
        private readonly DataProvider<int> _intProvider;
        string connStr = WebConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        public SuperAdminDAO()
        {
            _eduSystemProvider = new DataProvider<EducationSystem>();
            _campusProvider = new DataProvider<Campus>();
            _userAcaProvider = new DataProvider<UserAcaService>();
            _intProvider = new DataProvider<int>();
        }

        //Get all education system
        public List<EducationSystem> GetAllEducationSystem()
        {
            List<EducationSystem> educationSystems = new List<EducationSystem>();

            using (SqlConnection connection = new SqlConnection(connStr))
            {

                //Certificate
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "EducationSystem");
                connection.Open();
                SqlCommand command = null;

                command = new SqlCommand("SELECT * FROM EDUCATIONSYSTEM", connection);
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command;

                //Fill data set
                DataSet dataSet = new DataSet("EducationSystem");
                adapter.Fill(dataSet);
                connection.Close();
                DataTable eduSystemTable = dataSet.Tables["EducationSystem"];
                educationSystems = _eduSystemProvider.GetListObjects<EducationSystem>(eduSystemTable.Rows);
            }
            return educationSystems;
        }

        public List<Campus> GetListCampusById(int eduSystemId)
        {
            List<Campus> campuses = new List<Campus>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                //Certificate
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "Campus");
                connection.Open();
                SqlCommand command = null;
                command = new SqlCommand("SELECT * FROM CAMPUS WHERE EDUCATIONSYSTEMID = @PARAM1", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PARAM1", eduSystemId);

                adapter.SelectCommand = command;
                //Fill data set
                DataSet dataSet = new DataSet("Campus");
                adapter.Fill(dataSet);


                connection.Close();

                DataTable certTable = dataSet.Tables["Campus"];
                campuses = _campusProvider.GetListObjects<Campus>(certTable.Rows);

            }
            return campuses;

        }

        //Add education system
        public void AddEducationSystem(EducationSystem educationSystem)
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
                    //Insert to table [EducationSystem]
                    command.CommandText = "sp_Insert_EducationSystem";
                    command.Parameters.AddWithValue("@EducationName", educationSystem.EducationName);
                    command.Parameters.AddWithValue("@LogoImage", educationSystem.LogoImage);
                    //command.Parameters.Add(new SqlParameter("@EducationName", educationSystem.EducationName));
                    //command.Parameters.Add(new SqlParameter("@LogoImage", educationSystem.LogoImage));
                    //Get id of new certificate inserted to the database
                    int insertedEducationSystemId = Int32.Parse(command.ExecuteScalar().ToString());
                    //Insert to table [Campus]
                    //Change command store procedure name & parameters
                    //if (educationSystem.Campuses != null && educationSystem.Campuses.Count > 0)
                    //{
                    //    command.CommandText = "sp_Insert_Campus";
                    //    foreach (Campus campus in educationSystem.Campuses)
                    //    {
                    //        //Remove old parameters
                    //        command.Parameters.Clear();
                    //        command.Parameters.AddWithValue("@CampusName", campus.CampusName);
                    //        command.Parameters.AddWithValue("@EducationSystemId", insertedEducationSystemId);
                    //        command.ExecuteNonQuery();
                    //    }
                    //}
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

        public void AddSignature(Signature signature)
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
                    //Insert to table [Signature]
                    command.CommandText = "sp_Insert_Signature";
                    command.Parameters.AddWithValue("@FullName", signature.FullName);
                    command.Parameters.AddWithValue("@Position", signature.Position);
                    command.Parameters.AddWithValue("@ImageFile", signature.ImageFile);
                    //Get id of new signature inserted to the database
                    int insertedSignatureId = Int32.Parse(command.ExecuteScalar().ToString());
                    //Insert to table [Signature_EducationSystem]
                    //Change command store procedure name & parameters
                    
                    command.CommandText = "sp_Insert_Signature_EducationSystem";
                    //Remove old parameters
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@SignatureId", insertedSignatureId);
                    command.Parameters.AddWithValue("@EducationSystemId", signature.EducationSystemId);
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
        public Pagination<UserAcaService> GetAcaServicePagination(int pageSize, int pageNumber)
        {
            List<UserAcaService> academicServices = GetAllAcaService();

            Pagination<UserAcaService> pagination = new Pagination<UserAcaService>().GetPagination(academicServices, pageSize, pageNumber);
            return pagination;
        }
        public Pagination<Campus> GetCampusByEduPagination(int pageSize, int pageNumber, int eduSystemId)
        {
            List<Campus> academicServices = GetListCampusById(eduSystemId);

            Pagination<Campus> pagination = new Pagination<Campus>().GetPagination(academicServices, pageSize, pageNumber);
            return pagination;
        }
        public Pagination<UserAcaService> GetAdminPagination(int pageSize, int pageNumber)
        {
            List<UserAcaService> admins = GetAllAdmin();

            Pagination<UserAcaService> pagination = new Pagination<UserAcaService>().GetPagination(admins, pageSize, pageNumber);
            return pagination;
        }
        
        public List<UserAcaService> GetAllAdmin()
        {
            string query = "select [User].*, Campus.CampusId, EducationSystem.EducationName, Campus.CampusName, [Role].RoleId  from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and  Role.RoleName = 'Admin' ";

            List<UserAcaService> listAdmins = _userAcaProvider.GetListObjects<UserAcaService>(query, new object[] { });
            return listAdmins;
        }
        public List<UserAcaService> GetAllAcaService()
        {
            string query = "select [User].*, Campus.CampusId, EducationSystem.EducationName, Campus.CampusName, [Role].RoleId  from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and  Role.RoleName = 'Academic Service' ";

            List<UserAcaService> listAcademicService = _userAcaProvider.GetListObjects<UserAcaService>(query, new object[] { });
            return listAcademicService;
        }
        public int GetCountEduByName(string eduName)
        {
            string query = "select count(*) as [count] from EducationSystem where EducationName = @PARAM1 ";

            List<EducationSystem> listEducationSystem = _eduSystemProvider.GetListObjects<EducationSystem>(query, new object[] { eduName });
            return listEducationSystem.Count;
        }
        public void AddAcademicSerivce(User user, int campusId)
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
                    //Insert new admin
                    if (user.UserId == -1)
                    {
                        //Insert to table [User]
                        command.CommandText = "sp_Insert_AcademicServiceUser";

                        command.Parameters.Add(new SqlParameter("@PhoneNumber", user.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@AcademicEmail", user.AcademicEmail));
                        command.Parameters.Add(new SqlParameter("@CampusId", campusId));

                        command.ExecuteNonQuery();
                        //Commit the transaction
                        transaction.Commit();
                    }
                    //admin is existed, insert new role
                    else
                    {
                        //Insert to table [User]
                        command.CommandText = "sp_Insert_Existed_AcademicServiceUser";
                        command.Parameters.Add(new SqlParameter("@CampusId", campusId));
                        command.Parameters.Add(new SqlParameter("@UserId", user.UserId));
                        command.ExecuteNonQuery();
                        //Commit the transaction
                        transaction.Commit();
                    }

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

        public void AddAdminSerivce(User user, int campusId)
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
                    //Insert new admin
                    if (user.UserId == -1)
                    {
                        //Insert to table [User]
                        command.CommandText = "sp_Insert_AdminUser";

                        command.Parameters.Add(new SqlParameter("@PhoneNumber", user.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@AcademicEmail", user.AcademicEmail));
                        command.Parameters.Add(new SqlParameter("@CampusId", campusId));

                        command.ExecuteNonQuery();
                        //Commit the transaction
                        transaction.Commit();
                    }
                    //admin is existed, insert new role
                    else
                    {
                        //Insert to table [User]
                        command.CommandText = "sp_Insert_Existed_AdminUser";
                        command.Parameters.Add(new SqlParameter("@CampusId", campusId));
                        command.Parameters.Add(new SqlParameter("@UserId", user.UserId));
                        command.ExecuteNonQuery();
                        //Commit the transaction
                        transaction.Commit();
                    }

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