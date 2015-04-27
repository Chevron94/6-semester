using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Task1.pages
{
    public partial class Course : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CoursesView_DataBound(object sender, EventArgs e)
        {

        }

        protected void CoursesView_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {

        }

        protected void CoursesView_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {

        }

        protected void CoursesView_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "GO")
            {
                Response.Redirect("StudentsCourses.aspx?Id_Course=" + e.CommandArgument);
            }
            
            
        }

        protected void CoursesView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {

        }

        protected void CoursesView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btn = e.Row.FindControl("BtnGO") as Button;
                if (btn != null)
                {
                    btn.CommandArgument = ((Task1.models.Course)e.Row.DataItem).ID_Course.ToString();
                }
            }
        }
    }
}