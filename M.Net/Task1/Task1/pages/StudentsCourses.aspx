<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentsCourses.aspx.cs" Inherits="Task1.pages.StudentsCourses" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <table border="5" align="center">
        <tr>
            <td>
                <a href="Students.aspx">Cтуденты</a>
            </td>
            <td>
                <a href="Course.aspx">Курсы</a>
            </td>
            <td>
                <a href="StudentCourse.aspx">Сводная таблица</a>
            </td>
        </tr>
    </table>
    <form id="form1" runat="server">
    <div>
        <table style="width:100%">
        <tr>
            <td class="style1">
                <asp:GridView   ID="StudentCourseView" runat="server" 
                                AutoGenerateColumns="False" 
                                AllowPaging="True"
                                DataKeyNames="ID_Student,ID_Course"
                                DataSourceID="StudentCourseViewDataSource" 
                    ondatabound="StudentCourseView_DataBound" 
                    onrowcommand="StudentCourseView_RowCommand" 
                    onrowdatabound="StudentCourseView_RowDataBound" 
                    onrowdeleted="StudentCourseView_RowDeleted" 
                    onrowupdated="StudentCourseView_RowUpdated">
                    <Columns>

                        <asp:BoundField     DataField="FIO"
                                            HeaderText="ФИО"
                                            SortExpression="FIO"/>

                        <asp:BoundField     DataField="Name"
                                            HeaderText="Название курса"
                                            SortExpression="Name"/>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        </table>
    </div>
    <asp:ObjectDataSource   ID="StudentCourseViewDataSource" 
                            runat="server" 
                            TypeName="Task1.controllers.StudentCourseDAL"  
                            SelectMethod="Select">

        <SelectParameters>
            <asp:Parameter Name="ID_Student" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="ID_Course" Type="Int32" DefaultValue="0" />
            <asp:ControlParameter   Name="FIO" 
                                    ControlID="StudentCourseFilterByFIO"
                                    PropertyName="SelectedValue"/>

            <asp:ControlParameter   Name="Name" 
                                    ControlID="StudentCourseFilterByName"
                                    PropertyName="SelectedValue"/>     
        </SelectParameters>

    </asp:ObjectDataSource>
    </form>
</body>
</html>
