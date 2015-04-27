using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Task1.pages
{
    public partial class StudentsCourses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                string arg_std = Request.Params["Id_Student"];
                string arg_crs = Request.Params["ID_Course"];
                if (arg_crs != null || arg_std != null)
                {
                    StudentCourseViewDataSource.SelectParameters.Clear();
                    if (arg_std != null)
                    {
                        StudentCourseViewDataSource.SelectParameters.Add("ID_Student", arg_std);
                        StudentCourseViewDataSource.SelectParameters.Add("ID_Course", "0");
                    }
                    else
                    {
                        StudentCourseViewDataSource.SelectParameters.Add("ID_Student", "0");
                        StudentCourseViewDataSource.SelectParameters.Add("ID_Course", arg_crs);
                    }
                    StudentCourseViewDataSource.SelectParameters.Add("FIO", "Все");
                    StudentCourseViewDataSource.SelectParameters.Add("Name", "Все");
                    if (StudentCourseViewDataSource.Select() == null)
                    {
                        if (arg_crs != null)
                            Response.Redirect("Courses");
                        else Response.Redirect("Students");
                    }

                }
                else
                {
                    Response.Redirect("StudentCourse");
                }
            
        }

        protected void StudentCourseView_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void StudentCourseView_DataBound(object sender, EventArgs e)
        {

        }

        protected void StudentCourseView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void StudentCourseView_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {

        }

        protected void StudentCourseView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void StudentCourseView_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {

        }
    }
}