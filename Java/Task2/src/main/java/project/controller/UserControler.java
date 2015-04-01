package project.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.ModelMap;
import org.springframework.validation.BindingResult;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.annotation.ModelAttribute;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import project.dto.UserDto;
import project.service.HashPassword;
import project.service.UserService;

import javax.validation.Valid;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Roman on 29.03.2015.
 */
@Controller
@RequestMapping("/")
public class UserControler
{
    @Autowired
    UserService userService;
    int delError = 0;
    String oops="";

    @RequestMapping(value = "/", method = RequestMethod.GET)
    public String authentificateform(ModelMap modelMap)
    {
        return "index";
    }

    @RequestMapping(value = "/", method = RequestMethod.POST)
    public String auth(@ModelAttribute("user") @Valid UserDto user, BindingResult bindingResult, ModelMap modelMap)
    {
        if (bindingResult.hasErrors()) {
            List<String> validationErrors = new ArrayList<String>();
            for (FieldError error : bindingResult.getFieldErrors()) {
                validationErrors.add(error.getDefaultMessage());
            }
            modelMap.addAttribute("errors", validationErrors);
            return "index";
        }
        HashPassword hs = new HashPassword();

        user.setPassword(hs.encrypt(user.getPassword()));
        if (userService.Check(user.getLogin(),user.getPassword()))
            return "redirect:/all";
        else
        {
            List<String> validationErrors = new ArrayList<String>();
            validationErrors.add("Неверный логин/пароль!");
            modelMap.addAttribute("errors", validationErrors);
            return "index";
        }
    }

    @RequestMapping("/all")
    public String getAll(ModelMap modelMap) {
        modelMap.addAttribute("userList", userService.getAll());
        modelMap.addAttribute("error",delError);
        delError=0;
        return "all";
    }

    @RequestMapping(value = "/add", method = RequestMethod.GET)
    public String addForm(ModelMap modelMap)
    {
        return "addForm";
    }

    @RequestMapping(value = "/add", method = RequestMethod.POST)
    public String add(@ModelAttribute("user") @Valid UserDto user, BindingResult bindingResult, ModelMap modelMap) {
        if (bindingResult.hasErrors()) {
            List<String> validationErrors = new ArrayList<String>();
            for (FieldError error : bindingResult.getFieldErrors()) {
                validationErrors.add(error.getDefaultMessage());
            }
            modelMap.addAttribute("errors", validationErrors);
            return "addForm";
        }
        if (userService.Check(user.getLogin()))
        {
            List<String> validationErrors = new ArrayList<String>();
            validationErrors.add("Логин уже используется");
            modelMap.addAttribute("errors", validationErrors);
            return "addForm";
        }
        HashPassword hs = new HashPassword();
        user.setPassword(hs.encrypt(user.getPassword()));
        userService.add(user);
        return "redirect:/all";
    }

    @RequestMapping("/delete/{id}")
    public String delete(@PathVariable Integer id) {
        userService.delete(id);
        return "redirect:/user/all";
    }

    @RequestMapping(value = "/update/{id}", method = RequestMethod.GET)
    public String updateForm(@PathVariable Integer id, ModelMap modelMap)
    {
        modelMap.addAttribute("user", userService.getOne(id));
        oops = userService.getOne(id).getPassword();
        return "addForm";
    }

    @RequestMapping(value = "/update/{id}", method = RequestMethod.POST)
    public String update(@PathVariable Integer id, @ModelAttribute("user") @Valid UserDto user, BindingResult bindingResult, ModelMap modelMap){
        if (bindingResult.hasErrors()) {
            List<String> validationErrors = new ArrayList<String>();
            for (FieldError error : bindingResult.getFieldErrors()) {
                validationErrors.add(error.getDefaultMessage());
            }
            modelMap.addAttribute("errors", validationErrors);
            return "addForm";
        }
        if (user.getPassword()!="********")
        {
            userService.update(id, user.getLogin(),(new HashPassword()).encrypt(user.getPassword()),user.getName(),user.getSurname(),user.getPhone());
        }else
        {
            userService.update(id, user.getLogin(), user.getName(),user.getSurname(),user.getPhone());
        }
        return "redirect:/all";
    }

}
