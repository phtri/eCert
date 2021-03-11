﻿using eCert.Models.Entity;
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
        public List<T> GetListObjects<T>(DataRowCollection rows)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public T GetItem<T>(DataRow dr)
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

        public T GetObject<T>(string query, object[] parameter)
        {
            DataTable table = GET_DATA_TABLE(query, parameter);
            if(table.Rows.Count == 0)
            {
                return default(T);
            }
            return GetItem<T>(table.Rows[0]);
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
        public int ADD_UPDATE_DELETE(string query, object[] parameter)
        {
            int outputParam = -1;
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
                outputParam = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
                con.Close();
                
            }
            return outputParam;
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
        
        #region Private 
        
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
        #endregion
    }
}