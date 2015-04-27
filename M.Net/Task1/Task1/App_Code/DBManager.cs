using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Task1.App_Code
{
    public class DBManager : IDisposable
    {
        public static string connectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Labs\3-course\2-semester\M.Net\Task1\Task1\App_Data\University.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private SqlConnection connection;
        
        public DBManager()
        {
            connection = new SqlConnection(connectionString);
            Open();
        }

        public SqlDataReader ExecuteQuery(string command, Dictionary<string, object> args)
        {
            SqlCommand com = connection.CreateCommand();
            com.CommandText = command;
            if (args != null)
            {
                foreach (var arg in args)
                    com.Parameters.Add(arg.Key, arg.Value);  
            }
            return com.ExecuteReader();
        }

        public void ExecuteNonQuery(string command, Dictionary<string, object> args)
        {
            SqlCommand com = connection.CreateCommand();
            com.CommandText = command;
            if (args != null)
            {
                foreach (var arg in args)
                    com.Parameters.Add(arg.Key, arg.Value);
            }
            try
            {
                com.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                
            }
        }

        public void Open()
        {
            if (connection != null)
                connection.Open();
        }

        public void Close()
        {
            if (connection != null)
                connection.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }
}