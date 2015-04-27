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
import project.dto.AuthUserDto;
import project.dto.UserDto;
import project.service.UserService;

import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.validation.Valid;
import java.io.File;
import java.io.PrintWriter;
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
    public String authentificateForm(ModelMap modelMap, HttpServletRequest request)
    {
        Cookie cookie[] = request.getCookies();
        boolean flag = false;
        for(int i = 0; i<cookie.length && !flag; i++) {
           if( cookie[i].getName().compareTo("id") == 0)
           {
               flag = true;
           }
        }

        if (!flag)
            return "index";
        return "redirect:/all";
    }

    @RequestMapping(value = "/", method = RequestMethod.POST)
    public String auth(@ModelAttribute("user") @Valid AuthUserDto user, BindingResult bindingResult, ModelMap modelMap, HttpServletResponse response)
    {
        if (bindingResult.hasErrors()) {
            List<String> validationErrors = new ArrayList<String>();
            for (FieldError error : bindingResult.getFieldErrors()) {
                validationErrors.add(error.getDefaultMessage());
            }
            modelMap.addAttribute("errors", validationErrors);
            return "index";
        }
        try {
            File f = new File("D://tmp.txt");
            f.createNewFile();
            PrintWriter fw = new PrintWriter(f.getAbsoluteFile());
            fw.println(user.getPassword());
            fw.close();

        }catch(Exception e)
        {

        }
        //HashPassword hs = new HashPassword();

        //user.setPassword(hs.encrypt(user.getPassword()));
        int id = userService.Check(user.getLogin(),user.getPassword());
        if (id != -1)
        {
            Cookie cookie = new Cookie("id", String.valueOf(id));
            response.addCookie(cookie);
            return "redirect:/all";
        }
        else
        {
            List<String> validationErrors = new ArrayList<String>();
            validationErrors.add("Неверный логин/пароль!");
            modelMap.addAttribute("errors", validationErrors);
            return "index";
        }
    }

    @RequestMapping("/all")
    public String getAll(ModelMap modelMap, HttpServletRequest request)
    {
        Cookie cookie[] = request.getCookies();
        boolean flag = false;
        String id = "";
        for(int i = 0; i<cookie.length && !flag; i++)
        {
            if( cookie[i].getName().compareTo("id") == 0)
            {
                flag = true;
                id = cookie[i].getValue();
            }
        }
        if (!flag)
            return "redirect:/";
        modelMap.addAttribute("userList", userService.getAll());
        modelMap.addAttribute("error",delError);
        modelMap.addAttribute("id",id);
        delError=0;
        return "all";
    }

    @RequestMapping(value = "/add", method = RequestMethod.GET)
    public String addForm()
    {
        return "addForm";
    }

    @RequestMapping(value = "/add", method = RequestMethod.POST)
    public String add(@ModelAttribute("user") @Valid UserDto user, BindingResult bindingResult, ModelMap modelMap, HttpServletResponse response) {
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
       // HashPassword hs = new HashPassword();
       // user.setPassword(hs.encrypt(user.getPassword()));
        userService.add(user);
        int id = userService.Check(user.getLogin(),user.getPassword());
        Cookie cookie = new Cookie("id", String.valueOf(id));
        response.addCookie(cookie);
        return "redirect:/all";
    }

    @RequestMapping("/delete/{id}")
    public String delete(@PathVariable Integer id, HttpServletResponse response, HttpServletRequest request, ModelMap modelMap) {

        Cookie cookies[] = request.getCookies();
        boolean flag = false;
        String id_user = "";
        for(int i = 0; i<cookies.length && !flag; i++)
        {
            if( cookies[i].getName().compareTo("id") == 0)
            {
                flag = true;
                id_user = cookies[i].getValue();
            }
        }
        if (!flag || id_user.compareTo(id.toString()) != 0)
        {
            return "redirect:/all";
        }

        Cookie killCookie = new Cookie("id", null);
        killCookie.setMaxAge(0);
        killCookie.setPath("/");
        response.addCookie(killCookie);
        userService.delete(id);
        return "redirect:/";
    }

    @RequestMapping(value = "/update/{id}", method = RequestMethod.GET)
    public String updateForm(@PathVariable Integer id, ModelMap modelMap, HttpServletRequest request)
    {
        Cookie cookies[] = request.getCookies();
        boolean flag = false;
        String id_user = "";
        for(int i = 0; i<cookies.length && !flag; i++)
        {
            if( cookies[i].getName().compareTo("id") == 0)
            {
                flag = true;
                id_user = cookies[i].getValue();
            }
        }
        if (!flag || id_user.compareTo(id.toString()) != 0)
        {
            return "redirect:/all";
        }


        modelMap.addAttribute("user", userService.getOne(id));
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
        userService.update(id, user.getLogin(),user.getPassword(),user.getName(),user.getSurname(),user.getPhone());

        return "redirect:/all";
    }

    @RequestMapping("/logout")
    public String getAll(HttpServletResponse response) {
        Cookie killCookie = new Cookie("id", null);
        killCookie.setMaxAge(0);
        killCookie.setPath("/");
        response.addCookie(killCookie);
        return "redirect:/";
    }

}
