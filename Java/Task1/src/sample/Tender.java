package sample;

/**
 * Created by Roman on 08.03.2015.
 */
public class Tender
{
    private String companyNumber;
    private int costByAll;
    private int costByOne;
    private int countAirplanes;
    private String timeToBuild;
    private boolean isMilitary;

    public Tender(String _company, int _countAirplanes, boolean _isMilitary, int _costByOne, int _costByAll, String _timeToBuild)
    {
        companyNumber = _company;
        costByAll = _costByAll;
        costByOne = _costByOne;
        countAirplanes = _countAirplanes;
        timeToBuild = _timeToBuild;
        isMilitary = _isMilitary;
    }

    public String getCompanyNumber()
    {
        return companyNumber;
    }

    public int getCostByAll()
    {
        return costByAll;
    }

    public int getCostByOne()
    {
        return costByOne;
    }

    public int getCountAirplanes()
    {
        return countAirplanes;
    }

    public String getTimeToBuild()
    {
        return timeToBuild;
    }

    public boolean getIsMilitary()
    {
        return isMilitary;
    }
}
