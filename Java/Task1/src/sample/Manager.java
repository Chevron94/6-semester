package sample;

import java.util.Date;
import java.util.concurrent.atomic.AtomicInteger;

/**
 * Created by Roman on 07.03.2015.
 */
public class Manager
{
    private MilitaryCompany[] MinitaryCompanies;
    private PassengerCompany[] PassengerCompanies;
    private Informer informer;

    public Manager()
    {
        informer = new Informer();
        int mc = informer.getCountMilitaryAirplanes();
        int pc = informer.getCountPassengerAirplanes();
        MinitaryCompanies = new MilitaryCompany[mc];
        PassengerCompanies = new PassengerCompany[pc];
        for(int i = 0; i<mc; i++ )
        {
            MinitaryCompanies[i] = new MilitaryCompany(i);
        }

        for(int i = 0; i<pc; i++)
        {
            PassengerCompanies[i] = new PassengerCompany(i);
        }
    }

    public void FindCheaperBuy()
    {
        int needMilitary = informer.getCountMilitaryAirplanes();
        int needPasseger = informer.getCountPassengerAirplanes();
        Date timeMilitary = informer.getTimeMilitaryAirplanes();
        Date timePassenger = informer.getTimePassengerAirplanes();

    }

    public String CheaperBy(int cmpcnt, int need, Date time, boolean isMilitary)
    {
        String result = "";
        int finds;
        Date TotalTime;
        int totalsum;

        for(int i=0; i<cmpcnt; i++)
        {

        }

        return result;
    }


}
