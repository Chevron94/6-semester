using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Task1.models;
using Task1.App_Code;
using System.Data.SqlClient;

namespace Task1.controllers
{
    public class CourseDAL
    {
        private string GetWhere(int ID_Course, string Name, string Teacher, int Hours, Dictionary<string, object> param)
        {
            bool wasParams = false;
            string executeString = "";
            if (ID_Course != 0 || !string.IsNullOrEmpty(Name) && Name != "Все"
                        || !string.IsNullOrEmpty(Teacher) && Teacher != "Все" 
                        || Hours != 0)
            {
                executeString += " WHERE ";
                if (ID_Course != 0)
                {
                    executeString += "ID_Course = @ID_Course";
                    param.Add("@ID_Course", ID_Course);
                    wasParams = true;
                }
                if (!string.IsNullOrEmpty(Name) && Name != "Все")
                {
                    if (wasParams)
                    {
                        executeString += " AND ";
                    }
                    else wasParams = true;
                    executeString += "Name = @Name";
                    param.Add("@Name", Name);
                }
                if (!string.IsNullOrEmpty(Teacher) && Teacher != "Все")
                {
                    if (wasParams)
                    {
                        executeString += " AND ";
                    }
                    else wasParams = true;
                    executeString += "Teacher = @Teacher";
                    param.Add("@Teacher", Teacher);
                }
                if (Hours != 0)
                {
                    if (wasParams)
                    {
                        executeString += " AND ";
                    }
                    else wasParams = true;
                    executeString += "Hours = @Hours";
                    param.Add("@Hours", Hours);
                }
            }
            return executeString;
        }

        public List<Course> GetFilterByName()
        {
            string executeString = "SELECT DISTINCT Name FROM Courses";
            List<Course> res = new List<Course>();
            res.Add(new Course{Name = "Все"});
            using (DBManager manager = new DBManager())
            {
                SqlDataReader reader = manager.ExecuteQuery(executeString, null);
                while (reader.Read())
                {
                    res.Add(new models.Course() { Name = reader.GetString(0) });
                }
            }
            return res;
        }

        public List<Course> GetFilterByTeacher()
        {
            string executeString = "SELECT DISTINCT Teacher FROM Courses";
            List<Course> res = new List<Course>();
            res.Add(new Course { Teacher = "Все" });
            using (DBManager manager = new DBManager())
            {
                SqlDataReader reader = manager.ExecuteQuery(executeString, null);
                while (reader.Read())
                {
                    res.Add(new models.Course() { Teacher = reader.GetString(0) });
                }
            }
            return res;
        }

        public List<Course> GetFilterByHours()
        {
            string executeString = "SELECT DISTINCT Hours FROM Courses";
            List<Course> res = new List<Course>();
            res.Add(new Course { Hours = 0 });
            using (DBManager manager = new DBManager())
            {
                SqlDataReader reader = manager.ExecuteQuery(executeString, null);
                while (reader.Read())
                {
                    res.Add(new models.Course() { Hours = reader.GetInt32(0) });
                }
            }
            return res;
        }

        public void Delete(int ID_Course)
        {
            string executeString = "DELETE FROM Courses WHERE ID_Course = @ID_Course";
            Dictionary<String, object> args = new Dictionary<string, object>();
            args.Add("ID_Course", ID_Course);
            using(DBManager manager = new DBManager())
            {
                manager.ExecuteNonQuery(executeString,args);
            }
        }

        public void Update(int ID_Course, string Name, string Teacher, int Hours)
        {
            string executeString = "UPDATE Courses SET Name=@Name, Teacher=@Teacher, Hours=@Hours  WHERE ID_Course = @ID_Course";
            Dictionary<String, object> args = new Dictionary<string, object>();
            args.Add("ID_Course", ID_Course);
            args.Add("Name", Name);
            args.Add("Teacher", Teacher);
            args.Add("Hours", Hours);
            using (DBManager manager = new DBManager())
            {
                manager.ExecuteNonQuery(executeString, args);
            }
        }

        public void Insert(string Name, string Teacher, int Hours)
        {
            string executeString = "INSERT INTO Courses (Name, Teacher, Hours) VALUES (@Name, @Teacher, @Hours)";
            Dictionary<String, object> args = new Dictionary<string, object>();
            args.Add("Name", Name);
            args.Add("Teacher", Teacher);
            args.Add("Hours", Hours);
            using (DBManager manager = new DBManager())
            {
                manager.ExecuteNonQuery(executeString, args);
            }
        }

        public List<Course> Select(string Name, string Teacher, int Hours)
        {
            List<Course> result = new List<Course>();
            string executeString = "SELECT ID_Course, Name, Teacher, Hours FROM Courses ";
            Dictionary<string, object> param = new Dictionary<string, object>();

            using (App_Code.DBManager manager = new App_Code.DBManager())
            {
                executeString += GetWhere(0, Name, Teacher, Hours, param);
                SqlDataReader reader = manager.ExecuteQuery(executeString, param);
                while (reader.Read())
                {
                    result.Add(new Course()
                    {
                        ID_Course = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Teacher = reader.GetString(2),
                        Hours = reader.GetInt32(3)
                    });
                }
            }
            return result;
        }
    }
}