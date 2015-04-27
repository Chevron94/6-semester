<#include "/parts/top.ftl">
<#if errors??>
<div class="alert alert-danger" role="alert">
    <#list errors as error>
        <p>${error}</p>
    </#list>
</div>
</#if>
<!--script scr="resources/assets/js/md5.js"></script-->
<script type="text/javascript">
    function hashPassword(form)
    {
        for(var i = 0; i<form.elements.length;i++)
        {
            if (form.elements[i].name == "password")
                form.elements[i].value = md5(form.elements[i].value)
        }
    }
</script>
<form class="form-horizontal" onsubmit="hashPassword(this)" method="post" name="user">
    <div class="container" align="center">
        <!-- Brand and toggle get grouped for better mobile display -->
        <h3>Авторизация</h3>

        <!-- Collect the nav links, forms, and other content for toggling -->
        <div class="control-group">
            <label class="control-label" for="login">Логин</label>
            <div class="controls">
                <input type="text" id="login" name="login" <#if user?? && user.login??>value="${user.login}"</#if>>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="password">Пароль</label>
            <div class="controls">
                <input type="password" id="password" name="password" <#if user?? && user.password??></#if>>
            </div>
        </div>
        <br>
        <input class="btn btn-primary" type="submit" value="Войти!">
        <br>
        <a href = "/add">Регистрация</a>
    </div><!-- /.container-fluid -->
</form>
<#include "/parts/bottom.ftl">
