package project.repository;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import project.entity.User;
import org.springframework.data.repository.query.Param;

/**
 * Created by Roman on 29.03.2015.
 */
public interface UserRepository extends CrudRepository<User,Integer>
{
    @Query(value = "select count (*) from user where user.login =:login and user.password =:password",nativeQuery = true)
    public int checkUser(@Param("login") String login, @Param("password") String password);

    @Query(value = "select count (*) from user where user.login =:login",nativeQuery = true)
    public int unique(@Param("login") String login);
}