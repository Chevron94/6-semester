using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task1.models
{
    public class Course
    {
        public int ID_Course { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public int Hours { get; set; }
    }
}