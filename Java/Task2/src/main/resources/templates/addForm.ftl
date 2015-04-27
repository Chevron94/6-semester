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
<form onsubmit="hashPassword(this)" method="post" action="<#if user?? && user.idUser??>/update/${user.idUser}<#else>/add</#if>" name="user">
    <div class="container" align="center">
        <div class="form-group">
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
        <div class="form-group">
            <label class="control-label" for="name">Имя</label>
            <div class="controls">
                <input type="text" id="name" name="name" <#if user?? && user.name??>value="${user.name}"</#if>>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label" for="surname">Фамилия</label>
            <div class="controls">
                <input type="text" id="surname" name="surname" <#if user?? && user.surname??>value="${user.surname}"</#if>>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label" for="phone">Телефон</label>
            <div class="controls">
                <input type="text" name="phone" <#if user?? && user.phone??>value="${user.phone}"</#if> pattern="\d*">
            </div>
        </div>
        <input class="btn btn-primary" type="submit" value="Подтвердить">
    </div>
</form>

<#include "/parts/bottom.ftl">