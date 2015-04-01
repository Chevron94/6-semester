<#include "/parts/top.ftl">
<#if errors??>
<div class="alert alert-danger" role="alert">
    <#list errors as error>
        <p>${error}</p>
    </#list>
</div>
</#if>
<form method="post" action="<#if user?? && user.idUser??>/update/${user.idUser}<#else>/add</#if>" name="user">
    <div class="form-group">
        <label >Логин</label>
        <input type="text" class="form-control" name="login"  <#if user?? && user.login??>value="${user.login}"></#if>
    </div>
    <div class="form-group">
        <label >Пароль</label>
        <input type="password" class="form-control" name="password" <#if user?? && user.password??>placeholder="********"></#if>
    </div>
    <div class="form-group">
        <label >Имя</label>
        <input type="text" class="form-control" name="name" <#if user?? && user.name??>value="${user.name}"></#if>
    </div>
    <div class="form-group">
        <label>Фамилия</label>
        <input type="text" class="form-control" name="surname" <#if user?? && user.surname??>value="${user.surname}"></#if>
    </div>
    <div class="form-group">
        <label >Телефон</label>
        <input type="text" class="form-control" name="phone" <#if user?? && user.phone??>value="${user.phone}"</#if> pattern="\d*">
    </div>
    <input class="btn btn-primary" type="submit" value="Подтвердить">
</form>

<#include "/parts/bottom.ftl">