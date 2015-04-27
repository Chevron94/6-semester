package project.app;

import org.springframework.boot.SpringApplication;
import org.springframework.context.ApplicationContext;
import project.entity.User;
import project.repository.UserRepository;
import project.service.HashPassword;

public class Launcher {
    public static void main(String[] args) {
        ApplicationContext context = SpringApplication.run(MainConfig.class, args);

    }
}
