<#include "/parts/top.ftl">
<#if errors??>
<div class="alert alert-danger" role="alert">
    <#list errors as error>
        <p>${error}</p>
    </#list>
</div>
</#if>
<style>
    table {
        border: 5px solid #000000; /* Рамка вокруг таблицы */
    }
</style>
<div  align="center" class="container" width="768px">
    <h3>Все пользователи</h3>
    <div class="row">
        <div class="table-responsive">
            <table class="table table-striped">
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

    <a class="btn btn-primary" href = "/update/${id}">Изменить данные</a>
    <a class="btn btn-primary" href = "/delete/${id}">Удалить аккаунт</a>
    <a class="btn btn-primary" href = "/logout">Выйти</a>
</div>
<#include "/parts/bottom.ftl">