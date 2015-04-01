<#include "/parts/top.ftl">
<#if errors??>
<div class="alert alert-danger" role="alert">
    <#list errors as error>
        <p>${error}</p>
    </#list>
</div>
</#if>
<form method="post" name="user">
    <div class="container-fluid" align="center">
        <!-- Brand and toggle get grouped for better mobile display -->
        <h3>Авторизация</h3>

        <!-- Collect the nav links, forms, and other content for toggling -->
        <div class="form-group">
            <label >Логин</label>
                <input type="text" class="form-control" name="login" <#if user?? && user.login??>value="${user.login}"></#if>
        </div>
        <div class="form-group">
            <label >Пароль</label>
                <input type="password" class="form-control" name="password" <#if user?? && user.password??>></#if>
        </div>
        <br>
        <input class="btn btn-primary" type="submit" value="Войти!">
    </div><!-- /.container-fluid -->
</form>
<#include "/parts/bottom.ftl">
