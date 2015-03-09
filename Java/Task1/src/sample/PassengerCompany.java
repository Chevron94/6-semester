package sample;

import java.util.Date;
import java.util.Random;


/**
 * Created by Roman on 07.03.2015.
 */
public class PassengerCompany extends Company
{
    public PassengerCompany(String _name)
    {
        Random rnd = new Random();
        this.Cost = rnd.nextInt(2000000);
        this.Time = new Date(12*(long)Math.abs(rnd.nextInt()));
        this.Name = _name;
    }

}
