package project.dto;

import javax.validation.constraints.Size;

/**
 * Created by Roman on 01.04.2015.
 */
public class AuthUserDto
{
    @Size(min = 3, message = "Поле \"Login\" должно быть длиннее 3 символов")
    private String login;
    @Size(min = 8, message = "Поле \"Password\" должно быть длиннее 8 символов")
    private String password;

    public AuthUserDto() {
    }

    public String getLogin() {
        return login;
    }

    public void setLogin(String login) {
        this.login = login;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        AuthUserDto that = (AuthUserDto) o;

        if (login != null ? !login.equals(that.login) : that.login != null) return false;
        if (password != null ? !password.equals(that.password) : that.password != null) return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = login != null ? login.hashCode() : 0;
        result = 31 * result + (password != null ? password.hashCode() : 0);
        return result;
    }

    @Override
    public String toString() {
        return "AuthUserDto{" +
                "login='" + login + '\'' +
                ", password='" + password + '\'' +
                '}';
    }
}
