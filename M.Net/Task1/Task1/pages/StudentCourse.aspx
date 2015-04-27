<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentCourse.aspx.cs" Inherits="Task1.pages.StudentCourse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                                onrowdeleted="StudentCourseView_RowDeleted" 
                                onrowupdated="StudentCourseView_RowUpdated">
                    <Columns>

                        <asp:BoundField     DataField="FIO"
                                            HeaderText="ФИО"
                                            SortExpression="FIO"/>

                        <asp:BoundField     DataField="Name"
                                            HeaderText="Название курса"
                                            SortExpression="Name"/>

                        <asp:CommandField   ShowDeleteButton="true" 
                                            HeaderText="Удалить"/>
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                <asp:DetailsView    ID="StudentCourseView1"
                                    runat="server"
                                    autogenerateRows="false"
                                    DataSourceID="StudentCourseViewDataSource"
                                    DataKeyNames="ID_Student,ID_Course"
                                    Height="50px"
                                    Width="125px" oniteminserted="CoursesView1_ItemInserted" oniteminserting="StudentCourseView1_ItemInserting" 
                                    >
                    <Fields>

                        <asp:TemplateField HeaderText="Студент">
 
		                    <InsertItemTemplate>
			                    <asp:DropDownList id="StudentList" datasourceid="StudentCourseFIO"
				                    datatextfield="FIO" DataValueField="ID_Student"  
				                    SelectedValue='<%# Bind("ID_Student") %>' runat="server"/>
		                    </InsertItemTemplate>
		                    <EditItemTemplate>
			                    <asp:DropDownList id="StudentList" datasourceid="StudentCourseFIO"
			                        datatextfield="FIO" DataValueField="ID_Student"  
			                        SelectedValue='<%# Bind("ID_Student") %>'   runat="server"/>
	                        </EditItemTemplate>
	                    </asp:TemplateField>

                        <asp:TemplateField HeaderText="Предмет">

		                    <InsertItemTemplate>
			                    <asp:DropDownList id="CourseList" datasourceid="StudentCourseName"
				                    datatextfield="Name" DataValueField="ID_Course"  
				                    SelectedValue='<%# Bind("ID_Course") %>' runat="server"/>
		                    </InsertItemTemplate>
		                    <EditItemTemplate>
			                    <asp:DropDownList id="CourseList" datasourceid="StudentCourseName"
			                        datatextfield="Name" DataValueField="ID_Course"  
			                        SelectedValue='<%# Bind("ID_Course") %>'   runat="server"/>
	                        </EditItemTemplate>
	                    </asp:TemplateField>

                        <asp:CommandField ShowInsertButton="true" />
                    </Fields>
                </asp:DetailsView>
            </td>
        </tr>
        <tr>
            <td class="style1">
                <div class="style1">
                <label>Фильтр по фамилии</label>
                <asp:DropDownList   ID="StudentCourseFilterByFIO" 
                                    runat="server" 
                                    AutoPostBack="true" 
                                    DataSourceID="StudentCourseDataSourceFIO" 
                                    DataTextField="FIO" 
                        ondatabound="StudentCourseFilterByFIO_DataBound">
                </asp:DropDownList>
                </div>
                <div class="style1">
                <label>Фильтр по курсу</label>
               <asp:DropDownList   ID="StudentCourseFilterByName" 
                                    runat="server" 
                                    AutoPostBack="true" 
                                    DataSourceID="StudentCourseDataSourceName" 
                                    DataTextField="Name">
                </asp:DropDownList> 
                </div>
            </td>
        </tr>
        </table>
    </div>
    <asp:ObjectDataSource   ID="StudentCourseViewDataSource" 
                            runat="server" 
                            TypeName="Task1.controllers.StudentCourseDAL" 
                            DeleteMethod="Delete" 
                            InsertMethod="Insert" 
                            SelectMethod="Select">

        <DeleteParameters>
            <asp:Parameter Name="ID_Student" Type="Int32" />
            <asp:Parameter Name="ID_Course" Type="Int32" />
        </DeleteParameters>

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

        <InsertParameters>
            <asp:ControlParameter   Name="ID_Student" 
                                    ControlID="StudentCourseView1$StudentList"
                                    PropertyName="SelectedValue"/>
            <asp:ControlParameter   Name="ID_Course" 
                                    ControlID="StudentCourseView1$CourseList"
                                    PropertyName="SelectedValue"/>
        </InsertParameters>

    </asp:ObjectDataSource>

    <asp:ObjectDataSource   ID="StudentCourseDataSourceFIO" 
                            runat="server" 
                            TypeName="Task1.controllers.StudentCourseDAL"
                            SelectMethod="GetFilterByFIO"> 
    </asp:ObjectDataSource>

    <asp:ObjectDataSource   ID="StudentCourseDataSourceName" 
                            runat="server" 
                            TypeName="Task1.controllers.StudentCourseDAL"
                            SelectMethod="GetFilterByName">
    </asp:ObjectDataSource>

    <asp:ObjectDataSource   ID="StudentCourseFIO" 
                            runat="server" 
                            TypeName="Task1.controllers.StudentCourseDAL"
                            SelectMethod="GetStudentsFIO"> 
    </asp:ObjectDataSource>

    <asp:ObjectDataSource   ID="StudentCourseName" 
                            runat="server" 
                            TypeName="Task1.controllers.StudentCourseDAL"
                            SelectMethod="GetSubjectNames"> 
    </asp:ObjectDataSource>
    </form>
</body>
</html>
