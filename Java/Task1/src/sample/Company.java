package sample;

import java.util.Date;

/**
 * Created by Roman on 07.03.2015.
 */
public abstract class Company
{
    protected int Cost;
    protected Date Time;
    protected String Name;

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
