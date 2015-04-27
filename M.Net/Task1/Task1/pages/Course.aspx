<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Course.aspx.cs" Inherits="Task1.pages.Course" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 866px;
        }
        .style2
        {
            width: 80px;
        }
    </style>
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
            <td class="style2">
                <asp:GridView   ID="CoursesView" runat="server" 
                                AutoGenerateColumns="False" 
                                AllowPaging="True" DataKeyNames="ID_Course"
                                DataSourceID="CoursesDataSource" 
                                ondatabound="CoursesView_DataBound" 
                                onrowcommand="CoursesView_RowCommand" 
                                onrowdeleted="CoursesView_RowDeleted" 
                                onrowupdated="CoursesView_RowUpdated" 
                    onrowdatabound="CoursesView_RowDataBound">
                    <Columns>
                        

                        <asp:BoundField     DataField="ID_Course" ReadOnly="false"
                                            HeaderText="ID"
                                            SortExpression="ID_Course"/>

                        <asp:TemplateField HeaderText="Название"  >
                            <ItemTemplate>
                                <asp:Label  ID="LabelNamer" 
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'
                                        runat="server"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox    ID="InputName" 
                                                runat="server" 
                                                Text='<%# Bind("Name") %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="NAME_VALIDATOR" runat="server"
                                            ControlToValidate="InputName"
                                            ErrorMessage="*Поле не заполнено!*"
                                            ForeColor="Red">
                                 </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Преподаватель"  >
                            <ItemTemplate>
                                <asp:Label  ID="LabelTeacherr" 
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Teacher") %>'
                                        runat="server"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox    ID="InputTeacher" 
                                                runat="server" 
                                                Text='<%# Bind("Teacher") %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="TEACHER_VALIDATOR" runat="server"
                                            ControlToValidate="InputTeacher"
                                            ErrorMessage="*Поле не заполнено!*"
                                            ForeColor="Red">
                                 </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Часы">
                            <ItemTemplate>
                                <asp:Label  ID="LabelHours" 
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Hours") %>'
                                        runat="server"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox    ID="InputHours" 
                                                runat="server" 
                                                Text='<%# Bind("Hours") %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator id="HOURS_VALIDATOR" runat="server"
                                            ControlToValidate="InputHours"
                                            ErrorMessage="*Обязательное поле*"
                                            ForeColor="Red">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" 
                                                                        id="IsNumber" 
                                                                        controltovalidate="InputHours" 
                                                                        validationexpression="^[0-9]{1,}$" 
                                                                        errormessage="*Введите целое число!*" 
                                                                        ForeColor="Red" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:CommandField   ShowEditButton="true" 
                                            HeaderText="Изменить"/>

                        <asp:CommandField   ShowDeleteButton="true" 
                                            HeaderText="Удалить"/>

                        <asp:TemplateField HeaderText="GO">
                            <ItemTemplate>
                                <asp:Button ID="BtnGO"
                                            runat="server"
                                            CommandName="GO"
                                            Text="GO" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                <asp:DetailsView    ID="CoursesView1"
                                    runat="server"
                                    autogenerateRows="false"
                                    DataSourceID="CoursesDataSource"
                                    Height="50px"
                                    Width="125px" oniteminserted="CoursesView1_ItemInserted" 
                                    >
                    <Fields>

                        <asp:TemplateField HeaderText="Название"  >
                            <InsertItemTemplate>
                                <asp:TextBox    ID="InputName" 
                                                runat="server" 
                                                Text='<%# Bind("Name") %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="NAME_VALIDATOR" runat="server"
                                            ControlToValidate="InputName"
                                            ErrorMessage="*Поле не заполнено!*"
                                            ForeColor="Red">
                                 </asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Преподаватель"  >
                            <InsertItemTemplate>
                                <asp:TextBox    ID="InputTeacher" 
                                                runat="server" 
                                                Text='<%# Bind("Teacher") %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="TEACHER_VALIDATOR" runat="server"
                                            ControlToValidate="InputTeacher"
                                            ErrorMessage="*Поле не заполнено!*"
                                            ForeColor="Red">
                                 </asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Часы">
                            <InsertItemTemplate>
                                <asp:TextBox    ID="InputHours" 
                                                runat="server" 
                                                Text='<%# Bind("Hours") %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator id="HOURS_VALIDATOR" runat="server"
                                            ControlToValidate="InputHours"
                                            ErrorMessage="*Обязательное поле*"
                                            ForeColor="Red">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" 
                                                                        id="IsNumber" 
                                                                        controltovalidate="InputHours" 
                                                                        validationexpression="^[0-9]{1,}$" 
                                                                        errormessage="*Введите целое число!*" 
                                                                        ForeColor="Red" />
                            </InsertItemTemplate>
                        </asp:TemplateField>

                        <asp:CommandField ShowInsertButton="true" />
                    </Fields>
                </asp:DetailsView>
            </td>
        </tr>
        <tr>
            <td class="style2">
                <div class="style1">
                <label>Фильтр по названию</label>
                <asp:DropDownList   ID="CoursesFilterByName" 
                                    runat="server" 
                                    AutoPostBack="true" 
                                    DataSourceID="CoursesDataSourceName" 
                                    DataTextField="Name">
                </asp:DropDownList>
                </div>

                <div class="style1">
                <label>Фильтр по преподавателю</label>
               <asp:DropDownList   ID="CoursesFilterByTeacher" 
                                    runat="server" 
                                    AutoPostBack="true" 
                                    DataSourceID="CoursesDataSourceTeacher" 
                                    DataTextField="Teacher">
                </asp:DropDownList> 
                </div>

                <div class="style1">
                <label>Фильтр по часам</label>
               <asp:DropDownList   ID="CoursesFilterByHours" 
                                    runat="server" 
                                    AutoPostBack="true" 
                                    DataSourceID="CoursesDataSourceHours" 
                                    DataTextField="Hours">
                </asp:DropDownList> 
                </div>
            </td>
        </tr>
        </table>
    </div>
    <asp:ObjectDataSource   ID="CoursesDataSource" 
                            runat="server" 
                            TypeName="Task1.controllers.CourseDAL" 
                            DeleteMethod="Delete" 
                            InsertMethod="Insert" 
                            SelectMethod="Select" 
                            UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="ID_Course" Type="Int32" />
        </DeleteParameters>

        <SelectParameters>
            <asp:ControlParameter   Name="Name" 
                                    ControlID="CoursesFilterByName"
                                    PropertyName="SelectedValue"/>

            <asp:ControlParameter   Name="Teacher" 
                                    ControlID="CoursesFilterByTeacher"
                                    PropertyName="SelectedValue"/>     

            <asp:ControlParameter   Name="Hours" 
                                    ControlID="CoursesFilterByHours"
                                    PropertyName="SelectedValue"/>    
        </SelectParameters>

        <UpdateParameters>
            <asp:Parameter Name="ID_Course" Type="Int32" />
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="Teacher" Type="String" />
            <asp:Parameter Name="Hours" Type="Int32" />
        </UpdateParameters>

        <InsertParameters>
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="Teacher" Type="String" />
            <asp:Parameter Name="Hours" Type="Int32" />
        </InsertParameters>

    </asp:ObjectDataSource>

    <asp:ObjectDataSource   ID="CoursesDataSourceName" 
                            runat="server" 
                            TypeName="Task1.controllers.CourseDAL"
                            SelectMethod="GetFilterByName"> 
    </asp:ObjectDataSource>

    <asp:ObjectDataSource   ID="CoursesDataSourceTeacher" 
                            runat="server" 
                            TypeName="Task1.controllers.CourseDAL"
                            SelectMethod="GetFilterByTeacher">
    </asp:ObjectDataSource>

    <asp:ObjectDataSource   ID="CoursesDataSourceHours" 
                            runat="server" 
                            TypeName="Task1.controllers.CourseDAL"
                            SelectMethod="GetFilterByHours">
    </asp:ObjectDataSource>
    </form>
</body>
</html>
