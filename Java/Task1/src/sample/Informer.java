package sample;

import java.util.Date;
import java.util.Random;

/**
 * Created by Roman on 07.03.2015.
 */
public class Informer
{
    private int MilitaryCompaniesCount;
    private int PassengerCompaniesCount;
    private int CountPassengerAirplanes;
    private int CountMilitaryAirplanes;
    private Date TimePassengerAirplanes;
    private Date TimeMilitaryAirplanes;

    public Informer()
    {
        Random rnd = new Random();
        MilitaryCompaniesCount = rnd.nextInt(20);
        PassengerCompaniesCount = rnd.nextInt(20);
        CountMilitaryAirplanes = rnd.nextInt(100);
        CountPassengerAirplanes = rnd.nextInt(100);
        TimeMilitaryAirplanes = new Date(Math.abs(5*(long)rnd.nextInt()*CountMilitaryAirplanes));
        TimePassengerAirplanes = new Date(Math.abs(5*(long)rnd.nextInt()*CountPassengerAirplanes));
    }

    public int getMilitaryCompaniesCount()
    {
        return MilitaryCompaniesCount;
    }

    public int getPassengerCompaniesCount()
    {
        return PassengerCompaniesCount;
    }

    public int getCountPassengerAirplanes()
    {
        return CountPassengerAirplanes;
    }

    public int getCountMilitaryAirplanes()
    {
        return CountMilitaryAirplanes;
    }

    public Date getTimePassengerAirplanes()
    {
        return TimePassengerAirplanes;
    }

    public Date getTimeMilitaryAirplanes()
    {
        return TimeMilitaryAirplanes;
    }
}
