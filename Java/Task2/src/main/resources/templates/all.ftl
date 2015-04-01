<#include "/parts/top.ftl">
<div class="container-fluid" align="center">
    <h3>Все пользователи</h3>
    <div class="row">
        <div class="col-sm-8">
            <table class="table table-striped" align="center">
                <tr>
                    <th>ID</th>
                    <th>Login</th>
                    <th>Password</th>
                    <th>Name</th>
                    <th>Surname</th>
                    <th>Phone</th>
                </tr>
            <#list userList as user>
                <tr>
                    <td>${user.idUser}</td>
                    <td>${user.login}</td>
                    <td>${user.password}</td>
                    <td>${user.name}</td>
                    <td>${user.surname}</td>
                    <td>${user.phone}</td>
                </tr>
            </#list>
            </table>
        </div>
    </div>
    <button name="edit" type="button" class="btn btn-default">Изменить данные</button>
    <button name="delete" type="button" class="btn btn-default">Удалить аккаунт</button>
</div>
<#include "/parts/bottom.ftl">