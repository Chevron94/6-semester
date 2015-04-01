package sample;

import java.text.SimpleDateFormat;
import java.util.*;

/**
 * Created by Roman on 07.03.2015.
 */

public class Manager
{
    private MilitaryCompany[] militaryCompanies;
    private PassengerCompany[] passengerCompanies;
    private Informer informer;
    private List<Tender> tenders;
    private int totalSum = 0;

    public Manager()
    {
        informer = new Informer();
        int mc = informer.getMilitaryCompaniesCount();
        int pc = informer.getPassengerCompaniesCount();
        militaryCompanies = new MilitaryCompany[mc];
        passengerCompanies = new PassengerCompany[pc];
        for(int i = 0; i<mc; i++ )
        {
            militaryCompanies[i] = new MilitaryCompany("MILITARY_"+String.valueOf(i));
        }

        for(int i = 0; i<pc; i++)
        {
            passengerCompanies[i] = new PassengerCompany("PASSENGER_"+String.valueOf(i));
        }

        tenders = new ArrayList<Tender>();

    }

    public boolean FindCheaperBuy()
    {
        int needMilitary = informer.getCountMilitaryAirplanes();
        int needPasseger = informer.getCountPassengerAirplanes();
        Date timeMilitary = informer.getTimeMilitaryAirplanes();
        Date timePassenger = informer.getTimePassengerAirplanes();

        return  CheaperBuy(needMilitary, timeMilitary, true) && CheaperBuy(needPasseger,timePassenger,false);

    }

    public boolean CheaperBuy(int needBuy, Date timeLimit, boolean isMilitary)
    {
        int totalPlanes = 0;
        int readyPlanes = 0;
        SimpleDateFormat formattedDate = new SimpleDateFormat("dd.MM.yyyy");
        Company[] cmp;
        if (isMilitary)
        {
            cmp = this.militaryCompanies;
            readyPlanes = this.militaryCompanies[0].getReadyPlanes();
        }
        else cmp = this.passengerCompanies;

        cmp = SortByCost(cmp);

        long first = (timeLimit.getTime() - new Date(0).getTime())/ (24 * 60 * 60 * 1000);

        for(int i=0; (i<cmp.length) && (totalPlanes != needBuy); i++)
        {
            if (readyPlanes>= needBuy - totalPlanes)
            {
                Date tmpTime = new Date(System.currentTimeMillis());
                int canBuy = needBuy - totalPlanes;
                int tmpSum = canBuy*cmp[i].getCost();
                totalSum+=tmpSum;
                tenders.add(new Tender(cmp[i].getName(),canBuy,isMilitary,cmp[i].getCost(),tmpSum,formattedDate.format(tmpTime)));
                return true;
            }

            totalPlanes += readyPlanes;
            int tmpSum = readyPlanes * cmp[i].getCost();

            Date tmp = cmp[i].getTime();
            long second = (tmp.getTime()- new Date(0).getTime())/ (24 * 60 * 60 * 1000);
            if (second == 0)
                second =1;
            long canBuy = Math.abs(first / second);
            if (canBuy > 0)
            {
                if (canBuy + totalPlanes > needBuy)
                    canBuy = needBuy - totalPlanes;
                totalPlanes += canBuy;
                tmpSum += (int) canBuy * cmp[i].getCost();
                Date tmpTime = new Date(System.currentTimeMillis()+canBuy * cmp[i].getTime().getTime());
                totalSum += tmpSum;
                tenders.add(new Tender(cmp[i].getName(), (int)canBuy+readyPlanes, isMilitary, cmp[i].getCost (), tmpSum, formattedDate.format(tmpTime)));
            }
        }
        return  (totalPlanes == needBuy);
    }

    public Company[] SortByCost(Company[] cmp)
    {
        for(int i = 0; i < cmp.length - 1; i++)
            for(int j = 0; j < cmp.length - i - 1; j++)
                if(cmp[j].getCost() > cmp[j + 1].getCost())
                {
                    Company tmp = cmp[j];
                    cmp[j] = cmp[j+1];
                    cmp[j+1] = tmp;
                }
        return cmp;
    }
    public List<Tender> getTenders()
    {
        return tenders;
    }

    public Date getTimeLimitPass()
    {
        return new Date( informer.getTimePassengerAirplanes().getTime() + System.currentTimeMillis());
    }

    public int getTotalPlanesPass()
    {
        return informer.getCountPassengerAirplanes();
    }

    public int getTotalSum()
    {
        return totalSum;
    }

    public Date getTimeLimitMil()
    {
        return new Date( informer.getTimeMilitaryAirplanes().getTime() + System.currentTimeMillis());
    }

    public int getTotalPlanesMil()
    {
        return informer.getCountMilitaryAirplanes();
    }
}
