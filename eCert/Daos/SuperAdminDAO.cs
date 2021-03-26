using eCert.Models.Entity;
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

    }

    
}