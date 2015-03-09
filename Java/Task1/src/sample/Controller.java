package sample;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import java.text.SimpleDateFormat;


public class Controller
{
    private ObservableList<Tender> tenderData = FXCollections.observableArrayList();
    @FXML
    TextField ShowPlanesCountPass;
    @FXML
    TextField ShowPlanesCost;
    @FXML
    TextField ShowPlanesTimeLimitPass;
    @FXML
    TextField ShowPlanesCountMil;
    @FXML
    TextField ShowPlanesTimeLimitMil;
    @FXML
    Button TestButton;
    @FXML
    TableView<Tender> tenderTable;
    @FXML
    TableColumn<Tender,String> companyNumber;
    @FXML
    TableColumn<Tender,Integer> countAirplanes;
    @FXML
    TableColumn<Tender,Integer> costByOne;
    @FXML
    TableColumn<Tender,Integer> totalCost;
    @FXML
    TableColumn<Tender,String> timeToBuild;
    @FXML
    TableColumn<Tender,Boolean> isMilitary;

    @FXML
    void onClick()
    {
        tenderData.clear();
        ShowPlanesCost.setText("");
        Manager mng = new Manager();
        if (mng.FindCheaperBuy())
        {
            ShowPlanesCost.setText(String.valueOf(mng.getTotalSum()));
            for (int i = 0; i < mng.getTenders().size(); i++) {
                tenderData.add(mng.getTenders().get(i));
            }
        }
        SimpleDateFormat formattedDate = new SimpleDateFormat("dd.MM.yyyy");

        ShowPlanesTimeLimitPass.setText(formattedDate.format(mng.getTimeLimitPass()));
        ShowPlanesCountPass.setText(String.valueOf(mng.getTotalPlanesPass()));

        ShowPlanesTimeLimitMil.setText(formattedDate.format(mng.getTimeLimitMil()));
        ShowPlanesCountMil.setText(String.valueOf(mng.getTotalPlanesMil()));

        companyNumber.setCellValueFactory(new PropertyValueFactory<Tender, String>("companyNumber"));
        countAirplanes.setCellValueFactory(new PropertyValueFactory<Tender, Integer>("countAirplanes"));
        costByOne.setCellValueFactory(new PropertyValueFactory<Tender, Integer>("costByOne"));
        totalCost.setCellValueFactory(new PropertyValueFactory<Tender, Integer>("costByAll"));
        timeToBuild.setCellValueFactory(new PropertyValueFactory<Tender, String>("timeToBuild"));
        isMilitary.setCellValueFactory(new PropertyValueFactory<Tender, Boolean>("isMilitary"));

        tenderTable.setItems(tenderData);
        db db = new db();
        if (db.connect())
        {
            for (int i = 0; i < mng.getTenders().size(); i++)
                db.insertData(mng.getTenders().get(i));
            db.getData();
        }
        db.closeDB();
    }
}
