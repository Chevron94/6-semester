using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task1.models
{
    public class StudentCourse
    {
        public int ID_Student { get; set; }
        public string FIO { get; set; }
        public int ID_Course { get; set; }
        public string Name { get; set; }
    }
}