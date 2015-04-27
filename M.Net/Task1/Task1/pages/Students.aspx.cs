using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Task1.models;

namespace Task1.Pages
{
    public partial class Students : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void StudentsView_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {

        }

        protected void StudentsView_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {

        }

        protected void StudentsView_DataBound(object sender, EventArgs e)
        {
              
        }

        protected void StudentsView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GO")
            {
                Response.Redirect("StudentsCourses.aspx?Id_Student=" + e.CommandArgument);
            }
        }

        protected void StudentsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {

        }

        protected void StudentsView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void CommandBtn_Click(Object sender, CommandEventArgs e)
        {
            
        }

        protected void StudentsView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btn = e.Row.FindControl("BtnGO") as Button;
                if (btn != null)
                {
                    btn.CommandArgument=((Student)e.Row.DataItem).ID_Student.ToString();
                }
            }
        }
    }
}