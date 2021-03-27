﻿using eCert.Models.Entity;
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
        string connStr = WebConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        public SuperAdminDAO()
        {
            _eduSystemProvider = new DataProvider<EducationSystem>();
            _campusProvider = new DataProvider<Campus>();
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

                //Get certificate content
                foreach (EducationSystem educationSystem in educationSystems)
                {
                    SqlDataAdapter campusAdapter = new SqlDataAdapter();
                    campusAdapter.TableMappings.Add("Table", "Campus");
                    SqlCommand campusCommand = new SqlCommand("SELECT * FROM CAMPUS WHERE EDUCATIONSYSTEMID = @PARAM1", connection);
                    campusCommand.Parameters.AddWithValue("@PARAM1", educationSystem.EducationSystemId);
                    campusAdapter.SelectCommand = campusCommand;
                    campusAdapter.Fill(dataSet);
                    DataTable campusTable = dataSet.Tables["Campus"];
                    educationSystem.Campus = _campusProvider.GetListObjects<Campus>(campusTable.Rows);
                    campusTable.Clear();
                }
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
                    if (educationSystem.Campuses != null && educationSystem.Campuses.Count > 0)
                    {
                        command.CommandText = "sp_Insert_Campus";
                        foreach (Campus campus in educationSystem.Campuses)
                        {
                            //Remove old parameters
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@CampusName", campus.CampusName);
                            command.Parameters.AddWithValue("@EducationSystemId", insertedEducationSystemId);
                            command.ExecuteNonQuery();
                        }
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
       
    }

    
}