package project.dto;

import javax.validation.constraints.NotNull;
import javax.validation.constraints.Size;

/**
 * Created by Roman on 29.03.2015.
 */
public class UserDto
{
    @Size(min = 3, message = "Поле \"Login\" должно быть длиннее 3 символов")
    private String login;
    @Size(min = 8, message = "Поле \"Password\" должно быть длиннее 8 символов")
    private String password;
//    @Size(min = 1, message = "Поле \"Name\" не заполнено")
    private String name;
//    @Size(min = 1, message = "Поле \"Surname\" не заполнено")
    private String surname;
 //   @Size(min = 7, message = "Поле \"Phone\" не заполнено")
    private String phone;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getSurname() {
        return surname;
    }

    public void setSurname(String surname) {
        this.surname = surname;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

    public UserDto()
    {
        name = "";
        surname = "";
        phone = "";
        login = "";
        password = "";
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
    public String toString() {
        return "UserDto{" +
                "login='" + login + '\'' +
                ", password='" + password + '\'' +
                ", name='" + name + '\'' +
                ", surname='" + surname + '\'' +
                ", phone='" + phone + '\'' +
                '}';
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        UserDto userDto = (UserDto) o;

        if (!login.equals(userDto.login)) return false;
        if (!name.equals(userDto.name)) return false;
        if (!password.equals(userDto.password)) return false;
        if (!phone.equals(userDto.phone)) return false;
        if (!surname.equals(userDto.surname)) return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = login.hashCode();
        result = 31 * result + password.hashCode();
        result = 31 * result + name.hashCode();
        result = 31 * result + surname.hashCode();
        result = 31 * result + phone.hashCode();
        return result;
    }

}
