using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Task1.App_Code;
using Task1.models;
using System.Data.SqlClient;

namespace Task1.controllers
{
    public class StudentDAL
    {
        private string GetWhere(int ID_Student, string FIO, int Year, Dictionary<string, object> param)
        {
            bool wasParams = false;
            string executeString = "";
            if (ID_Student != 0 || !string.IsNullOrEmpty(FIO) && FIO != "Все"
                        || Year != 0)
            {
                executeString += " WHERE ";
                if (ID_Student != 0)
                {
                    executeString += "ID_Student = @ID_Student";
                    param.Add("@ID_Student", ID_Student);
                    wasParams = true;
                }
                if (!string.IsNullOrEmpty(FIO) && FIO != "Все")
                {
                    if (wasParams)
                    {
                        executeString += " AND ";
                    }
                    else wasParams = true;
                    executeString += "FIO = @FIO";
                    param.Add("@FIO", FIO);
                }
                if (Year != 0)
                {
                    if (wasParams)
                    {
                        executeString += " AND ";
                    }
                    else wasParams = true;
                    executeString += "Year = @Year";
                    param.Add("@Year", Year);
                }
            }
            return executeString;
        }

        public List<Student> GetFilterByFIO()
        {
            string executeString = "SELECT DISTINCT FIO FROM Students";
            List<Student> res = new List<Student>();
            res.Add(new Student { FIO = "Все" });
            using (DBManager manager = new DBManager())
            {
                SqlDataReader reader = manager.ExecuteQuery(executeString, null);
                while (reader.Read())
                {
                    res.Add(new Student() { FIO = reader.GetString(0) });
                }
            }
            return res;
        }

        public List<Student> GetFilterByYear()
        {
            string executeString = "SELECT DISTINCT Year FROM Students";
            List<Student> res = new List<Student>();
            res.Add(new Student { FIO = "Все" });
            using (DBManager manager = new DBManager())
            {
                SqlDataReader reader = manager.ExecuteQuery(executeString, null);
                while (reader.Read())
                {
                    res.Add(new Student() { Year = reader.GetInt32(0) });
                }
            }
            return res;
        }

        public void Delete(int ID_Student)
        {
            string executeString = "DELETE FROM Students WHERE ID_Student = @ID_Student";
            Dictionary<String, object> args = new Dictionary<string, object>();
            args.Add("ID_Student", ID_Student);
            using (DBManager manager = new DBManager())
            {
                manager.ExecuteNonQuery(executeString, args);
            }
        }

        public void Update(int ID_Student, string FIO, int Year)
        {
            string executeString = "UPDATE Students SET FIO=@FIO, Year=@Year  WHERE ID_Student = @ID_Student";
            Dictionary<String, object> args = new Dictionary<string, object>();
            args.Add("ID_Student", ID_Student);
            args.Add("FIO", FIO);
            args.Add("Year", Year);
            using (DBManager manager = new DBManager())
            {
                manager.ExecuteNonQuery(executeString, args);
            }
        }

        public void Insert(string FIO, int Year)
        {
            string executeString = "INSERT INTO Students (FIO, Year) VALUES (@FIO,  @Year)";
            Dictionary<String, object> args = new Dictionary<string, object>();
            args.Add("FIO", FIO);
            args.Add("Year", Year);
            using (DBManager manager = new DBManager())
            {
                manager.ExecuteNonQuery(executeString, args);
            }
        }

        public List<Student> Select(string FIO, int Year)
        {
            List<Student> result = new List<Student>();
            string executeString = "SELECT ID_Student, FIO, Year FROM Students ";
            Dictionary<string, object> param = new Dictionary<string, object>();

            using (App_Code.DBManager manager = new App_Code.DBManager())
            {
                executeString += GetWhere(0, FIO, Year, param);
                SqlDataReader reader = manager.ExecuteQuery(executeString, param);
                while (reader.Read())
                {
                    result.Add(new Student()
                    {
                        ID_Student = reader.GetInt32(0),
                        FIO = reader.GetString(1),
                        Year = reader.GetInt32(2)
                    });
                }
            }
            return result;
        }
    }
}