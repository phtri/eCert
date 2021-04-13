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
        private readonly DataProvider<Staff> _userAcaProvider;
        private readonly DataProvider<Certificate> _certificateProvider;
        private readonly DataProvider<Role> _roleProvider;
        private readonly DataProvider<User_Role> _userRoleProvider;
        private readonly DataProvider<Signature_EducationSystem> _signatureEduProvider;
        string connStr = WebConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        public SuperAdminDAO()
        {
            _eduSystemProvider = new DataProvider<EducationSystem>();
            _campusProvider = new DataProvider<Campus>();
            _userAcaProvider = new DataProvider<Staff>();
            _certificateProvider = new DataProvider<Certificate>();
            _roleProvider = new DataProvider<Role>();
            _userRoleProvider = new DataProvider<User_Role>();
            _signatureEduProvider = new DataProvider<Signature_EducationSystem>();
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
        public Pagination<Staff> GetAcaServicePagination(int pageSize, int pageNumber)
        {
            List<Staff> academicServices = GetAllAcaService();

            Pagination<Staff> pagination = new Pagination<Staff>().GetPagination(academicServices, pageSize, pageNumber);
            return pagination;
        }
        public Pagination<EducationSystem> GetEduSystemPagination(int pageSize, int pageNumber)
        {
            List<EducationSystem> listEdySystem = GetAllEducationSystem();

            Pagination<EducationSystem> pagination = new Pagination<EducationSystem>().GetPagination(listEdySystem, pageSize, pageNumber);
            return pagination;
        }
        public Pagination<Campus> GetCampusByEduPagination(int pageSize, int pageNumber, int eduSystemId)
        {
            List<Campus> academicServices = GetListCampusById(eduSystemId);

            Pagination<Campus> pagination = new Pagination<Campus>().GetPagination(academicServices, pageSize, pageNumber);
            return pagination;
        }
        public Pagination<Staff> GetAdminPagination(int pageSize, int pageNumber)
        {
            List<Staff> admins = GetAllAdmin();

            Pagination<Staff> pagination = new Pagination<Staff>().GetPagination(admins, pageSize, pageNumber);
            return pagination;
        }
        
        public List<Staff> GetAllAdmin()
        {
            string query = "select [User].*, Campus.CampusId, EducationSystem.EducationName, Campus.CampusName, [Role].RoleId  from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and  Role.RoleName = 'Admin' ";

            List<Staff> listAdmins = _userAcaProvider.GetListObjects<Staff>(query, new object[] { });
            return listAdmins;
        }
        public List<Staff> GetAllAcaService()
        {
            string query = "select [User].*, Campus.CampusId, EducationSystem.EducationName, Campus.CampusName, [Role].RoleId  from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and  Role.RoleName = 'Academic Service' ";

            List<Staff> listAcademicService = _userAcaProvider.GetListObjects<Staff>(query, new object[] { });
            return listAcademicService;
        }
        public int GetCountEduByName(string eduName)
        {
            string query = "select * from EducationSystem where EducationName = @PARAM1";

            List<EducationSystem> listEducationSystem = _eduSystemProvider.GetListObjects<EducationSystem>(query, new object[] { eduName });
            return listEducationSystem.Count;
        }
        public int GetCountCertificateByEdu(int eduSystemId)
        {
            string query = "select Certificate.* from Campus, Certificate, EducationSystem where Campus.CampusId = Certificate.CampusId and EducationSystem.EducationSystemId = Campus.EducationSystemId and EducationSystem.EducationSystemId = @PARAM1";

            List<Certificate> listCertificate = _certificateProvider.GetListObjects<Certificate>(query, new object[] { eduSystemId });
            return listCertificate.Count;
        }
        public int GetCountCertificateByCampus(int campusId)
        {
            string query = "select Certificate.* from Campus, Certificate where Campus.CampusId = Certificate.CampusId and Certificate.CampusId = @PARAM1";

            List<Certificate> listCertificate = _certificateProvider.GetListObjects<Certificate>(query, new object[] { campusId });
            return listCertificate.Count;
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
        public List<User_Role> GetUserRoleByListRole(List<Role> listRole)
        {
            string query = String.Empty;
            if(listRole.Count != 0)
            {
                query += "select * from User_Role where ";
                for(int i=0; i<listRole.Count; i++)
                {
                    if(i != listRole.Count - 1)
                    {
                        query += "RoleId = " + listRole[i].RoleId + " or ";
                    }
                    else
                    {
                        query += "RoleId = " + listRole[i].RoleId;
                    }
                    
                }
                
            }
            List<User_Role> listUserRole = _userRoleProvider.GetListObjects<User_Role>(query, new object[] { });
            return listUserRole;
        }
        public List<Role> GetRoleByCampusId(int campusId)
        {
            string query = "select * from Role where CampusId = @PARAM1 ";

            List<Role> listRole = _roleProvider.GetListObjects<Role>(query, new object[] { campusId });
            return listRole;
        }
        public int GetNumberOfUser(int userId)
        {
            string query = "select [User].*, Campus.CampusId, EducationSystem.EducationName, Campus.CampusName  from [User], [User_Role], [Role], Campus, EducationSystem where [User].UserId = [User_Role].UserId and [User_Role].RoleId = [Role].RoleId and [Role].CampusId = Campus.CampusId and Campus.EducationSystemId = EducationSystem.EducationSystemId and [User].UserId = @PARAM1";

            List<Staff> listAdmins = _userAcaProvider.GetListObjects<Staff>(query, new object[] { userId });
            return listAdmins.Count;
        }
        public void DeleteCampus(int campusId)
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
                List<Role> listRole = GetRoleByCampusId(campusId);
                List<User_Role> listUserRole = GetUserRoleByListRole(listRole);
                try
                {
                    foreach(User_Role userRole in listUserRole)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "sp_Delete_User_Role";
                        command.Parameters.Add(new SqlParameter("@UserId", userRole.UserId));
                        command.Parameters.Add(new SqlParameter("@RoleId", userRole.RoleId));    
                        command.ExecuteNonQuery();

                        int numOfUser = GetNumberOfUser(userRole.UserId);
                        if(numOfUser == 1)
                        {
                            //Delete from table [User]
                            command.Parameters.Clear();
                            command.CommandText = "sp_Delete_User";
                            command.Parameters.Add(new SqlParameter("@UserId", userRole.UserId));
                            command.ExecuteNonQuery();
                        }
                    }

                    command.Parameters.Clear();
                    command.CommandText = "sp_Delete_Campus";
                    command.Parameters.Add(new SqlParameter("@CampusId", campusId));
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
        public List<Signature_EducationSystem> GetSignatureEduByeduSystemId(int eduSystemId)
        {
            string query = "select * from Signature_EducationSystem where EducationSystemId = @PARAM1 ";

            List<Signature_EducationSystem> listSignatureEdu = _roleProvider.GetListObjects<Signature_EducationSystem>(query, new object[] { eduSystemId });
            return listSignatureEdu;
        }
        public int CountNumOfSignature(int signatureId)
        {
            string query = "select * from Signature_EducationSystem where SignatureId = @PARAM1 ";

            List<Signature_EducationSystem> listSignatureEdu = _roleProvider.GetListObjects<Signature_EducationSystem>(query, new object[] { signatureId });
            return listSignatureEdu.Count;
        }
        public void DeleteEducation(int eduSystemId)
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
                List<Campus> campuses = GetListCampusById(eduSystemId);
                List<Signature_EducationSystem> listSignatureEdu = GetSignatureEduByeduSystemId(eduSystemId);
                try
                {
                   foreach(Campus campus in campuses)
                   {
                        DeleteCampus(campus.CampusId);
                   }
                   foreach(Signature_EducationSystem signature_EducationSystem in listSignatureEdu)
                   {
                        command.Parameters.Clear();
                        command.CommandText = "sp_Delete_Signature_Education";
                        command.Parameters.Add(new SqlParameter("@EducationSystemId", eduSystemId));
                        command.ExecuteNonQuery();

                        int numOfSig = CountNumOfSignature(signature_EducationSystem.SignatureId);
                        if(numOfSig == 0)
                        {
                            command.Parameters.Clear();
                            command.CommandText = "sp_Delete_Signature";
                            command.Parameters.Add(new SqlParameter("@SignatureId", signature_EducationSystem.SignatureId));
                            command.ExecuteNonQuery();
                        }
                   }
                    command.Parameters.Clear();
                    command.CommandText = "sp_Delete_EducationSystem";
                    command.Parameters.Add(new SqlParameter("@EducationSystemId", eduSystemId));
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