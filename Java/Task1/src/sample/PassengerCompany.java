package sample;

import java.util.Date;
import java.util.Random;


/**
 * Created by Roman on 07.03.2015.
 */
public class PassengerCompany extends Company
{
    public PassengerCompany(int index)
    {
        Random rnd = new Random();
        this.Cost = rnd.nextInt(2000000);
        this.Time = new Date(rnd.nextInt(2),rnd.nextInt(12),rnd.nextInt(30));
        this.Name = String.valueOf(index);
    }
    public int getCost()
    {
        return this.Cost;
    }

    public Date getTime()
    {
        return this.Time;
    }

    public String getName()
    {
        return this.Name;
    }
}
