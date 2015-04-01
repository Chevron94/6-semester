package project.app;

import org.springframework.boot.SpringApplication;
import org.springframework.context.ApplicationContext;
import project.entity.User;
import project.repository.UserRepository;
import project.service.HashPassword;

public class Launcher {
    public static void main(String[] args) {
        ApplicationContext context = SpringApplication.run(MainConfig.class, args);
        UserRepository usr = context.getBean(UserRepository.class);
        HashPassword hs = new HashPassword();
        User usr1 = new User("root","12345678","Admin", "Ivanov","12345678");
        User usr2 = new User("Sasha_anime","anime_the_best","Sasha","Martynov","987654321");
        User usr3 = new User("test","testtest","test","test","7894561232");
        usr1.setPassword(hs.encrypt(usr1.getPassword()));
        usr2.setPassword(hs.encrypt(usr2.getPassword()));
        usr3.setPassword(hs.encrypt(usr3.getPassword()));
        usr1 = usr.save(usr1);
        usr2 = usr.save(usr2);
        usr3 = usr.save(usr3);
    }
}
