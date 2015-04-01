package project.entity;

import com.sun.xml.internal.bind.v2.model.core.ID;

import javax.persistence.*;

/**
 * Created by Roman on 29.03.2015.
 */
@Entity
public class User
{
    @Id
    @Column(name="ID_USER")
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer idUser;
    private String login;
    private String password;
    private String name;
    private String surname;
    private String phone;



    public User()
    {
    }
    public User(String log, String pass, String _name, String _surname, String _phone)
    {
        login = log;
        password = pass;
        name = _name;
        surname = _surname;
        phone = _phone;
    }

    public Integer getIdUser() {
        return idUser;
    }

    public void setIdUser(Integer idUser) {
        this.idUser = idUser;
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

    public String getName() {
        return name;
    }

    public String getPhone() {
        return phone;
    }

    public String getSurname() {
        return surname;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        User user = (User) o;

        if (!idUser.equals(user.idUser)) return false;
        if (!login.equals(user.login)) return false;
        if (!name.equals(user.name)) return false;
        if (!password.equals(user.password)) return false;
        if (!phone.equals(user.phone)) return false;
        if (!surname.equals(user.surname)) return false;

        return true;
    }

    @Override
    public String toString() {
        return "User{" +
                "idUser=" + idUser +
                ", login='" + login + '\'' +
                ", password='" + password + '\'' +
                ", name='" + name + '\'' +
                ", surname='" + surname + '\'' +
                ", phone='" + phone + '\'' +
                '}';
    }

    @Override
    public int hashCode() {
        int result = idUser.hashCode();
        result = 31 * result + login.hashCode();
        result = 31 * result + password.hashCode();
        result = 31 * result + name.hashCode();
        result = 31 * result + surname.hashCode();
        result = 31 * result + phone.hashCode();
        return result;
    }

    public void setName(String name) {

        this.name = name;
    }

    public void setSurname(String surname) {
        this.surname = surname;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

}
