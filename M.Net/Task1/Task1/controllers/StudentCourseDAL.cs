using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Task1.models;
using Task1.App_Code;
using System.Data.SqlClient;

namespace Task1.controllers
{
    public class StudentCourseDAL
    {
        public List<StudentCourse> GetWhere(Dictionary<string,object> args)
        {
            bool isparams = false;
            List<StudentCourse> result = new List<StudentCourse>();
            String executeString = "Select S.ID_Student, S.FIO, C.ID_Course, C.Name " +
                                    "FROM Courses AS C, Students AS S, StudentCourse AS SC " +
                                    "WHERE C.ID_Course = SC.ID_Course AND S.ID_Student = SC.ID_Student";

            foreach (var arg in args)
            {
                if (arg.Value != null)
                {
                    isparams = true;
                    if (arg.Key == "ID_Student")
                        executeString += " AND SC.ID_Student = @ID_Student";
                    else if (arg.Key == "ID_Course")
                        executeString += " AND SC.ID_Course = @ID_Course";
                    else if (arg.Key == "Name")
                        executeString += " AND C.Name = @Name";
                    else executeString += " AND S.FIO = @FIO";
                }
            }
            using (DBManager db = new DBManager())
            {
                SqlDataReader reader;
                if (isparams)
                    reader = db.ExecuteQuery(executeString, args);
                else reader = db.ExecuteQuery(executeString, null);
                while (reader.Read())
                {
                    result.Add(new StudentCourse()
                                {   ID_Student = reader.GetInt32(0),
                                    FIO = reader.GetString(1),
                                    ID_Course = reader.GetInt32(2),
                                    Name = reader.GetString(3),
                                });
                }
            }
            return result;
        }
        
        public List<StudentCourse> GetFilterByFIO()
        {
            string executeString = "SELECT DISTINCT S.ID_Student, S.FIO FROM Students AS S";
            List<StudentCourse> res = new List<StudentCourse>();
            res.Add(new StudentCourse { FIO = "Все" });
            using (DBManager db = new DBManager())
            {
                SqlDataReader reader = db.ExecuteQuery(executeString,null);
                while (reader.Read())
                {
                    res.Add(new StudentCourse() {   ID_Student = reader.GetInt32(0), 
                                                    FIO = reader.GetString(1) });
                }
            }
            return res;
        }

        public List<StudentCourse> GetStudentsFIO()
        {
            List<StudentCourse> res = GetFilterByFIO();
            res.RemoveAt(0);
            return res;
        }

        public List<StudentCourse> GetFilterByName()
        {
            string executeString = "SELECT DISTINCT C.ID_Course, C.Name FROM Courses AS C";
            List<StudentCourse> res = new List<StudentCourse>();
            res.Add(new StudentCourse { Name = "Все" });
            using (DBManager db = new DBManager())
            {
                SqlDataReader reader = db.ExecuteQuery(executeString, null);
                while (reader.Read())
                {
                    res.Add(new StudentCourse() { 
                        ID_Course = reader.GetInt32(0),
                        Name = reader.GetString(1) });
                }
            }
            return res;
        }

        public List<StudentCourse> GetSubjectNames()
        {
            List<StudentCourse> res = GetFilterByName();
            res.RemoveAt(0);
            return res;
        }

        public List<StudentCourse> Select(int ID_Student, int ID_Course, string FIO, string Name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            if (FIO != "Все")
                args.Add("FIO", FIO);
            if (Name != "Все")
                args.Add("Name", Name);
            if (ID_Student != 0)
                args.Add("ID_Student", ID_Student);
            if (ID_Course != 0)
                args.Add("ID_Course", ID_Course);
            return GetWhere(args);      
        }

        public void Delete(int ID_Student, int ID_Course)
        {
            string executeString = "DELETE FROM StudentCourse WHERE ID_Student = @ID_Student AND ID_Course = @ID_Course";
            Dictionary<String, object> args = new Dictionary<string, object>();
            args.Add("ID_Student", ID_Student);
            args.Add("ID_Course", ID_Course);
            using (DBManager manager = new DBManager())
            {
                manager.ExecuteNonQuery(executeString, args);
            }
        }

        public void Insert(int ID_Student, int ID_Course)
        {
            string executeString = "INSERT INTO StudentCourse (ID_Student, ID_Course) VALUES (@ID_Student,  @ID_Course)";
            Dictionary<String, object> args = new Dictionary<string, object>();
            args.Add("ID_Student", ID_Student);
            args.Add("ID_Course", ID_Course);
            using (DBManager manager = new DBManager())
            {
                manager.ExecuteNonQuery(executeString, args);
            }
        }
    }
}