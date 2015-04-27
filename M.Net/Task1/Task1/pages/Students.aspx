<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Students.aspx.cs" Inherits="Task1.Pages.Students" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Students</title>
    <style type="text/css">
        .style1
        {
            width: 550px;
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
        <table style="width:100%" align="center">
        <tr>
            <td class="style1">
                <asp:GridView   ID="StudentsView" runat="server" 
                                AutoGenerateColumns="False" 
                                AllowPaging="True" DataKeyNames="ID_Student"
                                ondatabound="StudentsView_DataBound" 
                                onrowcommand="StudentsView_RowCommand" 
                                onrowdeleted="StudentsView_RowDeleted" 
                                onrowupdated="StudentsView_RowUpdated" 
                                DataSourceID="StudentsDataSource" 
                    onselectedindexchanged="StudentsView_SelectedIndexChanged" 
                    onrowdatabound="StudentsView_RowDataBound">
                    <Columns>
                        

                        <asp:BoundField     DataField="ID_Student" 
                                            ReadOnly="true"
                                            HeaderText="ID"
                                            SortExpression="ID_Student"/>

                        <asp:TemplateField HeaderText="ФИО">
                        <ItemTemplate>
                            <asp:Label  ID="LabelFIO" 
                                        Text='<%# DataBinder.Eval(Container.DataItem, "FIO") %>'
                                        runat="server"/>
                        </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox    ID="InputFIO" 
                                                runat="server" 
                                                Text='<%# Bind("FIO") %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="FIO_VALIDATOR" runat="server"
                                            ControlToValidate="InputFIO"
                                            ErrorMessage="*Поле не заполнено!."
                                            ForeColor="Red">
                                 </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Год рождения">
                            <ItemTemplate>
                                <asp:Label  ID="LabelYear" 
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Year") %>'
                                        runat="server"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox    ID="InputYear" 
                                                runat="server" 
                                                Text='<%# Bind("Year") %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator id="YEAR_VALIDATOR" runat="server"
                                            ControlToValidate="InputYear"
                                            ErrorMessage="*Обязательное поле*"
                                            ForeColor="Red">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" 
                                                                        id="IsNumber" 
                                                                        controltovalidate="InputYear" 
                                                                        validationexpression="^[0-9]{1.}$" 
                                                                        errormessage="*Год должен быть целым числом*" 
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
                                            Text="GO"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                <asp:DetailsView    ID="StudentsView1"
                                    runat="server"
                                    autogenerateRows="false"
                                    DataSourceID="StudentsDataSource"
                                    Height="50px"
                                    Width="125px" 
                                    oniteminserted="StudentsView1_ItemInserted">
                    <Fields>

                        <asp:TemplateField HeaderText="ФИО"  >
                            <InsertItemTemplate>
                                <asp:TextBox    ID="InputFIO" 
                                                runat="server" 
                                                Text='<%# Bind("FIO") %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="FIO_VALIDATOR" runat="server"
                                            ControlToValidate="InputFIO"
                                            ErrorMessage="*Поле не заполнено!."
                                            ForeColor="Red">
                                 </asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Год рождения">
                            <InsertItemTemplate>
                                <asp:TextBox    ID="InputYear" 
                                                runat="server" 
                                                Text='<%# Bind("Year") %>'>
                                </asp:TextBox>
                                <asp:RequiredFieldValidator id="YEAR_VALIDATOR" runat="server"
                                            ControlToValidate="InputYear"
                                            ErrorMessage="*Обязательное поле*"
                                            ForeColor="Red">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" 
                                                                        id="IsNumber" 
                                                                        controltovalidate="InputYear" 
                                                                        validationexpression="^[0-9]{4}$" 
                                                                        errormessage="*Год введен неверно!*" 
                                                                        ForeColor="Red" />
                            </InsertItemTemplate>
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
                <asp:DropDownList   ID="StudentsFilterByFIO" 
                                    runat="server" 
                                    AutoPostBack="true" 
                                    DataSourceID="StudentsDataSourceFIO" 
                                    DataTextField="FIO">
                </asp:DropDownList>
                </div>
                <div class="style1">
                <label>Фильтр по году рождения</label>
               <asp:DropDownList   ID="StudentsFilterByYear" 
                                    runat="server" 
                                    AutoPostBack="true" 
                                    DataSourceID="StudentsDataSourceYear" 
                                    DataTextField="Year">
                </asp:DropDownList> 
                </div>
            </td>
        </tr>
        </table>
    </div>
    <asp:ObjectDataSource   ID="StudentsDataSource" 
                            runat="server" 
                            TypeName="Task1.controllers.StudentDAL" 
                            DeleteMethod="Delete" 
                            InsertMethod="Insert" 
                            SelectMethod="Select" 
                            UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="ID_Student" Type="Int32" />
        </DeleteParameters>

        <SelectParameters>
            <asp:ControlParameter   Name="FIO" 
                                    ControlID="StudentsFilterByFIO"
                                    PropertyName="SelectedValue"/>

            <asp:ControlParameter   Name="Year" 
                                    ControlID="StudentsFilterByYear"
                                    PropertyName="SelectedValue"/>     
        </SelectParameters>

        <UpdateParameters>
            <asp:Parameter Name="ID_Student" Type="Int32" />
            <asp:Parameter Name="FIO" Type="String" />
            <asp:Parameter Name="Year" Type="Int32" />
        </UpdateParameters>

        <InsertParameters>
            <asp:Parameter Name="FIO" Type="String" />
            <asp:Parameter Name="Year" Type="Int32" />
        </InsertParameters>

    </asp:ObjectDataSource>

    <asp:ObjectDataSource   ID="StudentsDataSourceFIO" 
                            runat="server" 
                            TypeName="Task1.controllers.StudentDAL"
                            SelectMethod="GetFilterByFIO"> 
    </asp:ObjectDataSource>

        <asp:ObjectDataSource   ID="StudentsDataSourceYear" 
                                runat="server" 
                                TypeName="Task1.controllers.StudentDAL"
                                SelectMethod="GetFilterByYear">
        </asp:ObjectDataSource>

    </form>
</body>
</html>
