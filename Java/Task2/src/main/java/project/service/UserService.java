package project.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import project.dto.UserDto;
import project.entity.User;
import project.repository.UserRepository;

import javax.transaction.Transactional;
import java.io.File;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Roman on 29.03.2015.
 */
@Service
public class UserService
{
    @Autowired
    UserRepository userRepository;

    @Transactional
    public User add(User user)
    {
        return userRepository.save(user);
    }

    @Transactional
    public User add(UserDto User){
        User newUser = new User();
        newUser.setName(User.getName());
        newUser.setSurname(User.getSurname());
        newUser.setPhone(User.getPhone());
        newUser.setLogin(User.getLogin());
        newUser.setPassword(User.getPassword());
        return userRepository.save(newUser);
    }


    @Transactional
    public void delete(Integer id)
    {
        userRepository.delete(id);
    }

    @Transactional
    public User getOne(Integer id)
    {
        return userRepository.findOne(id);
    }

    @Transactional
    public int Check(String login, String password)
    {
        int id;
        try
        {
           id = userRepository.checkUser(login, password);
        }
        catch (Exception e)
        {
            id = -1;
        }
        return id;
    }
    public boolean Check(String login)
    {
        return userRepository.unique(login) == 1;
    }
    @Transactional
    public List<User> getAll()
    {
        List<User> users = new ArrayList<User>();
        for(User user : userRepository.findAll())
            users.add(user);
        return users;
    }

    @Transactional
    public User update(Integer id, String login, String password, String name, String surname, String phone)
    {
        User updUser = userRepository.findOne(id);
        updUser.setLogin(login);
        updUser.setPassword(password);
        updUser.setName(name);
        updUser.setSurname(surname);
        updUser.setPhone(phone);
        return userRepository.save(updUser);
    }
    @Transactional
    public User update(Integer id, String login, String name, String surname, String phone)
    {
        User updUser = userRepository.findOne(id);
        updUser.setLogin(login);
        updUser.setName(name);
        updUser.setSurname(surname);
        updUser.setPhone(phone);
        return userRepository.save(updUser);
    }
}
