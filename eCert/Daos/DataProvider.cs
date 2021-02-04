using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;

namespace eCert.Daos
{
    public class DataProvider<T>
    {
        string connStr = WebConfigurationManager.ConnectionStrings["Database"].ConnectionString;


        /**
         * Get list object
         * Example: DataTable dataTable = dataProvider.GET_LIST_OBJECT( "select * from Ships where name like @param1", new object[] { "%abc%" });
         */
        public List<T> GetListObjects<T>(string query, object[] parameter)
        {
            DataTable dataTable = GET_DATA_TABLE(query, parameter);
            List<T> data = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
       
        

        public DataTable GET_DATA_TABLE(string query, object[] parameter)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            DataTable dataTable = new DataTable();
            try
            {
                con = new SqlConnection(connStr);
                con.Open();
                cmd = new SqlCommand(query, con);
                if (parameter != null)
                {
                    string[] listParam = query.Split(' ');
                    int i = 0;
                    foreach (string item in listParam)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }


        /**
         * Add, update, delete
         * Example: dataProvinder.ADD("INSERT INTO Person VALUES( @param1 , @param2 )", new object[] { tbName, tbAge });
         */
        public void ADD_UPDATE_DELETE(string query, object[] parameter)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            try
            {
                con = new SqlConnection(connStr);
                con.Open();
                cmd = new SqlCommand(query, con);
                if (parameter != null)
                {
                    string[] listParam = query.Split(' ');
                    int i = 0;
                    foreach (string item in listParam)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        private T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo property in temp.GetProperties())
                {
                    if (property.Name == column.ColumnName && dr[column.ColumnName] != DBNull.Value)
                        property.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }



    }
}