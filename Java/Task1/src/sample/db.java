package sample;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Roman on 09.03.2015.
 */
public class db
{
    Connection c = null;
    Statement stmt = null;
    ResultSet res = null;

    public boolean connect()
    {
        try

        {
            Class.forName("org.sqlite.JDBC");
            c = DriverManager.getConnection("jdbc:sqlite:D:/Labs/3-course/2-semester/Java/Task1/tenders.s3db");
            stmt = c.createStatement();
        } catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public List<Tender> getData()
    {
        List<Tender> result = new ArrayList<Tender>();
        String sql = "SELECT * FROM TENDERS_HISTORY;";
        try
        {            res = stmt.executeQuery(sql);
            while(res.next())
            {
                Tender tmp = new Tender(res.getString("companyName"),
                                        res.getInt("numberAirplanes"),
                                        res.getBoolean("isMilitary"),
                                        res.getInt("price"),
                                        res.getInt("totalCost"),
                                        res.getString("timeToConstruction"));
                result.add(tmp);
            }
            return result;
        }
        catch (SQLException e)
        {
            return null;
        }
    }

    public boolean insertData(Tender tender)
    {
        String sql = "INSERT INTO TENDERS_HISTORY (companyName, numberAirplanes, isMilitary, price, totalCost, timeToConstruction)"+
                " VALUES ('"+tender.getCompanyNumber()+"', "+tender.getCountAirplanes()+", "+(tender.getIsMilitary() ? 1 : 0)+", "+tender.getCostByOne()+", "+
                tender.getCostByAll()+", '"+tender.getTimeToBuild()+"');";
        try
        {
            stmt.execute(sql);
        }
        catch (SQLException e)
        {
            return false;
        }
        return true;
    }

    public void closeDB()
    {
        try
        {
            c.close();
            stmt.close();
            res.close();
        }
        catch (SQLException e)
        {
            return;
        }
    }
}



