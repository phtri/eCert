using eCert.Utilities;
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
        public List<string> LIST_STRING(string query, object[] parameter)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                List<string> list = new List<string>();
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
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(dr[0].ToString());
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
                
            }
            finally
            {
                dr.Close();
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
        /**
          * Get object
          * Example: DataTable dataTable = dataProvider.GET_OBJECT( "select * from Ships where name like @param1", new object[] { "%abc%" });
          */
        public DataTable GET_OBJECT(string query, object[] parameter = null)
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
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable;
                }
                return null;
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

        public void ExecuteSqlTransaction(List<StoreProcedureOption> storeProcedureOptions)
        {
            
            using(SqlConnection connection = new SqlConnection(connStr))
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
                    //Loop thourgh each store procedure
                    foreach (StoreProcedureOption procedureOption in storeProcedureOptions)
                    {
                        //Clear all parameters of command before add new parameters
                        command.Parameters.Clear();
                        command.CommandText = procedureOption.ProcedureName;
                        //Add parameters to command
                        foreach (SqlParameter parameter in procedureOption.Parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                        command.ExecuteNonQuery();
                        
                    }
                    //Commit the transaction
                    transaction.Commit();
                }catch(Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                    
                    try
                    {
                        //Rollback when transtraction failed
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }

            }
        }

        #region Private 
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
        private DataTable GET_DATA_TABLE(string query, object[] parameter)
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
        
        #endregion




    }
}